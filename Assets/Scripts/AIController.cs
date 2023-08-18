using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Circuit circuit;
    public float brakingSensitivity = 3f;
    Drive drive;
    public float steeringSensitivity = 0.01f;
    public float accelSensitivity = 0.3f;
    Vector3 target;
    Vector3 nextTarget;
    int currentWP = 0;
    float totalDistanceToTarget;

    GameObject tracker;
    int currentTrackerWP = 0;
    public float lookAhead = 10;

    // Start is called before the first frame update
    void Start()
    {
        drive = this.GetComponent<Drive>();
        target = circuit.waypoints[currentWP].transform.position;
        nextTarget = circuit.waypoints[currentWP + 1].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, drive.rb.gameObject.transform.position);

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.transform.position = drive.rb.gameObject.transform.position;
        tracker.transform.rotation = drive.rb.gameObject.transform.rotation;
    }

    /// <summary>
    /// Moves tracker around track
    /// </summary>
    void ProgressTracker()
    {
        Debug.DrawLine(drive.rb.gameObject.transform.position, tracker.transform.position);

        if (Vector3.Distance(drive.rb.gameObject.transform.position, tracker.transform.position) > lookAhead)
        {
            return;
        }

        tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, 1.0f); //speed of tracker

        if (Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.waypoints.Length)
            {
                currentTrackerWP = 0;
            }
        }
    }

    // Update is called once per frame
    bool isJumping = false;
    void Update()
    {
        ProgressTracker();
        // translates the target's coordinates into the space of the vehicle
        // the vehicle's rigid body becomes the origin and assigns it to the localTarget
        Vector3 localTarget = drive.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, - 1, 1) * Mathf.Sign(drive.currentSpeed);
        float speedFactor = drive.currentSpeed / drive.maxSpeed;

        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90.0f;

        float brake = 0;
        if (corner > 10 && speedFactor > 0.1f)
        {
            brake = Mathf.Lerp(0, 1 + speedFactor * brakingSensitivity, cornerFactor);
        }

        float accel = 1f;
        if (corner > 20 && speedFactor > 0.2f)
        {
            accel = Mathf.Lerp(0, 1 * accelSensitivity, 1 - cornerFactor);
        }

        Debug.Log("Brake" + brake + ", Accel: " + accel + ", Speed: " + drive.rb.velocity.magnitude + ", Time: " + Mathf.Round(Time.time) + " seconds");

        drive.Go(accel, steer, brake);

        drive.CheckForSkid();
        drive.CalculateEngineSound();
    }
}
