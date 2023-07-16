using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    public float antiRoll = 5000.0f;
    public WheelCollider wheelLFront;
    public WheelCollider wheelRFront;
    public WheelCollider wheelLBack;
    public WheelCollider wheelRBack;
    public GameObject COM;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = COM.transform.localPosition;
    }

    void GroundWheels(WheelCollider WL, WheelCollider WR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        // work out the proportion of distance that the hit point is along the suspencion
        // take the hit point or the negative of the hit point, which is going to be on the opposite side of the 
        // collider. Take away the radius, find that distance, and divide it by the suspencion distance. Wheel that 
        // hass the greater value of these grounded values is one that's actually slid further along its suspencion rod
        bool groundedL = WL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WL.transform.InverseTransformPoint(hit.point).y - WL.radius) / WL.suspensionDistance;

        bool groundedR = WR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WR.transform.InverseTransformPoint(hit.point).y - WR.radius) / WR.suspensionDistance;

        // take the proportion of the distance travelled the wheels have along the suspencion, multiply that by the antiroll,
        // then apply that particular force to the wheel that needs the force added
        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            rb.AddForceAtPosition(WL.transform.up * -antiRollForce, WL.transform.position);

        if (groundedR)
            rb.AddForceAtPosition(WR.transform.up * antiRollForce, WR.transform.position);

    }

    void FixedUpdate()
    {
        GroundWheels(wheelLFront, wheelRFront);
        GroundWheels(wheelLBack, wheelRBack);
    }
}
