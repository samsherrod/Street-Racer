using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidtime = 0;
    public float wanderDistance = 4; // avoiding distance
    public float avoidLength = 1; // 1 second

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("car"))
        {
            avoidtime = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("car"))
        {
            return;
        }

        Rigidbody otherCar = collision.rigidbody;
        avoidtime = Time.time + avoidLength;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
        avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
    }
}
