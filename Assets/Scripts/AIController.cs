using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Circuit circuit;
    Drive drive;
    public float steeringSensitivity = 0.01f;
    Vector3 target;
    int currentWP = 0;

    // Start is called before the first frame update
    void Start()
    {
        drive = this.GetComponent<Drive>();
        target = circuit.waypoints[currentWP].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // translates the target's coordinates into the space of the vehicle
        // the vehicle's rigid body becomes the origin and assigns it to the localTarget
        Vector3 localTarget = drive.rb.gameObject.transform.InverseTransformPoint(target);
        float distanceToTarget = Vector3.Distance(target, drive.rb.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, - 1, 1) * Mathf.Sign(drive.currentSpeed);
        float accel = 1f;
        float brake = 0;

        //if (distanceToTarget < 5)
        //{
        //    brake = 0.8f; accel = 0.1f;
        //}

        // the vehicle game object moves drives on its own
        drive.Go(accel, steer, brake);

        // have a small threshold
        // make larger if car starts to circle waypoint
        if (distanceToTarget < 4)
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Length)
                currentWP = 0;
            target = circuit.waypoints[currentWP].transform.position;
        }

        drive.CheckForSkid();
        drive.CalculateEngineSound();
    }
}
