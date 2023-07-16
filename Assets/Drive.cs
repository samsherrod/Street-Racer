using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public WheelCollider[] WCs;
    public GameObject[] Wheels;
    public float torque = 200; // rotational force
    public float maxSteerAngle = 30; // the maximum angle the wheels can rotate
    public float maxBrakeTorque = 500; // force acting in the opposition direction of the forward thrust

    public AudioSource skidSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// clamping the values accel values between -1 and 1 and then multiplies it by the torque
    /// to get the wheel collider's motor torque    /// </summary>
    /// <param name="accel"></param>
    void Go(float accel, float steer, float brake)
    {
        // acts as a kind of percentage amount of the full torque that can be added
        // the longer the up and down arrows are holed to down, the more the accelleration
        // will get closer to -1 or 1, which will then multipy the torque to get the actual
        // torque that gets put on the wheel

        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        float thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {
            WCs[i].motorTorque = thrustTorque;

            // front wheels steer, back wheels break
            if (i < 2) WCs[i].steerAngle = steer; // front wheels
            else WCs[i].brakeTorque = brake; // back wheels

            Quaternion quat;
            Vector3 position;
            WCs[i].GetWorldPose(out position, out quat);
            Wheels[i].transform.position = position;
            Wheels[i].transform.rotation = quat;

            // optimized version - use later
            //WCs[i].GetWorldPose(out Vector3 position, out Quaternion quat);
            //Wheels[i].transform.SetPositionAndRotation(position, quat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // use the up and down arrow keys to add accelleration to the wheel collider
        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        Go(a, s, b);
    }
}
