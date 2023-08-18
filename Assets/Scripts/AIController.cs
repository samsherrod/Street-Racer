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
    float lookAhead = 10;

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
        //Vector3 nextLocalTarget = drive.rb.gameObject.transform.InverseTransformPoint(nextTarget);
        //float distanceToTarget = Vector3.Distance(target, drive.rb.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        //float nextTargetAngle = Mathf.Atan2(nextLocalTarget.x, nextLocalTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, - 1, 1) * Mathf.Sign(drive.currentSpeed);
        float brake = 0;
        float accel = 0.5f;
        //float distanceFactor = distanceToTarget / totalDistanceToTarget;
        //float speedFactor = drive.currentSpeed / drive.maxSpeed;

        //float accel = Mathf.Lerp(accelSensitivity, 1, distanceFactor);

        // when distance factor is 0, car has no braking
        // when distance factor is 1, cas has full braking
        // if distance factor is between 0 and 1, it will be somewhere between 
        //float brake = Mathf.Lerp((-1 - Mathf.Abs(nextTargetAngle)) * brakingSensitivity, 1 + speedFactor, 1 - distanceFactor);

        //if (distanceToTarget < 8 && Mathf.Abs(nextTargetAngle) > 20)
        //{
        //    brake += 0.8f;
        //    accel -= 0.8f;
        //}

        //if (isJumping)
        //{
        //    accel = 1;
        //    brake = 0;
        //    Debug.Log("Jumping");
        //}

        Debug.Log("Brake" + brake + ", Accel: " + accel + ", Speed: " + drive.rb.velocity.magnitude + ", Time: " + Mathf.Round(Time.time) + " seconds");

        //if (distanceToTarget < 5)
        //{
        //    brake = 0.8f; accel = 0.1f;
        //}

        // the vehicle game object moves drives on its own
        drive.Go(accel, steer, brake);

        // have a small threshold
        // make larger if car starts to circle waypoint
        //if (distanceToTarget < 4)
        //{
        //    currentWP++;
        //    if (currentWP >= circuit.waypoints.Length)
        //        currentWP = 0;
        //    target = circuit.waypoints[currentWP].transform.position;
        //    if (currentWP == circuit.waypoints.Length - 1)
        //        nextTarget = circuit.waypoints[0].transform.position;
        //    else
        //        nextTarget = circuit.waypoints[currentWP + 1].transform.position;
        //    totalDistanceToTarget = Vector3.Distance(target, drive.rb.gameObject.transform.position);

        //    if (drive.rb.gameObject.transform.InverseTransformPoint(target).y > 5)
        //    {
        //        isJumping = true;
        //    }
        //    else isJumping = false;
        //}

        drive.CheckForSkid();
        drive.CalculateEngineSound();
    }
}
