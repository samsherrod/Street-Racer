using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    public WheelCollider[] WCs;
    public GameObject[] Wheels;
    public float torque = 200; // rotational force
    public float maxSteerAngle = 30; // the maximum angle the wheels can rotate
    public float maxBrakeTorque = 500; // force acting in the opposite direction of the forward thrust

    public AudioSource skidSound;
    public AudioSource highAccel;

    public Transform skidTrailPrefab;
    Transform[] skidTrails = new Transform[4];

    public ParticleSystem smokePrefab;
    ParticleSystem[] skidSmoke = new ParticleSystem[4];

    public GameObject brakeLight;
    
    public Rigidbody rb;
    public float gearLength = 3;
    public float currentSpeed { get { return rb.velocity.magnitude * gearLength;  } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public int numGears = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public float maxSpeed = 200;

    public GameObject playerNamePrefab;

    /// <summary>
    /// Creates a skid effect that intatiates the skidTrailPrefab. Its position is at the base of the wheel
    /// and is childed to its wheel collider
    /// </summary>
    /// <param name="i"></param>
    public void StartSkidTrail(int i)
    {
        if (skidTrails[i] == null) skidTrails[i] = Instantiate(skidTrailPrefab);

        skidTrails[i].parent = WCs[i].transform;
        skidTrails[i].localRotation = Quaternion.Euler(90, 0, 0);
        skidTrails[i].localPosition = -Vector3.up * WCs[i].radius;
    }

    public void EndSkidTrail(int i)
    {
        if (skidTrails[i] == null) return;
        Transform holder = skidTrails[i];
        skidTrails[i] = null;
        holder.parent = null;
        holder.rotation = Quaternion.Euler(90, 0, 0);
        Destroy(holder.gameObject, 30);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            skidSmoke[i] = Instantiate(smokePrefab);
            skidSmoke[i].Stop();
        }

        brakeLight.SetActive(false);

        // Instantiate the playerName text UI prefab with the appropriate text above each car
        GameObject playerName = Instantiate(playerNamePrefab);
        playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;
        playerName.GetComponent<Text>().text = "Player Name";
    }

    public void CalculateEngineSound()
    {
        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
                                                   Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);

        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * currentGear;

        if (currentGear > 0 && speedPercentage < downGearMax)
            currentGear--;

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1)))
            currentGear++;

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;
    }

    /// <summary>
    /// clamping the values accel values between -1 and 1 and then multiplies it by the torque
    /// to get the wheel collider's motor torque    /// </summary>
    /// <param name="accel"></param>
    public void Go(float accel, float steer, float brake)
    {
        // acts as a kind of percentage amount of the full torque that can be added
        // the longer the up and down arrows are holed to down, the more the accelleration
        // will get closer to -1 or 1, which will then multipy the torque to get the actual
        // torque that gets put on the wheel

        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        if (brake != 0)
            brakeLight.SetActive(true);
        else 
            brakeLight.SetActive(false);

        // thrustTorque is what is actually speeding up the vehicle therefore if the car's
        // current speed is less than the max speed, speed the car up
        float thrustTorque = 0;
        if (currentSpeed < maxSpeed)
            thrustTorque =  accel * torque;

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

    public void CheckForSkid()
    {
        int numSkidding = 0;
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            WCs[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4 || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {
                numSkidding++;
                if (!skidSound.isPlaying)
                    skidSound.Play();

                //StartSkidTrail(i);

                // positions smoke particle effects
                skidSmoke[i].transform.position = WCs[i].transform.position - WCs[i].transform.up * WCs[i].radius;
                skidSmoke[i].Emit(1);
            }
            //else
                //EndSkidTrail(i);
        }

        if (numSkidding == 0 && skidSound.isPlaying)
        {
            skidSound.Stop();
        }
    }
}
