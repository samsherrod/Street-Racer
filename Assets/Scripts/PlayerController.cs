using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Drive ds;
    float lastTimeMoving = 0;
    Vector3 lastPosition;
    Quaternion lastRotation;
    [SerializeField] private GameObject carBody;

    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        ds = GetComponent<Drive>();
    }

    // Update is called once per frame
    void Update()
    {

        // use the up and down arrow keys to add accelleration to the wheel collider
        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        if (ds.rb.velocity.magnitude > 1 || !RaceMonitor.racing)
        {
            lastTimeMoving = Time.time;
        }

        CheckForGroundType();

        if (!RaceMonitor.racing) a = 0;

        ds.Go(a, s, b);

        ds.CheckForSkid();
        ds.CalculateEngineSound();
    }

    // TODO - Check other ground types to change friction such as on grass vs track
    private void CheckForGroundType()
    {
        // check if ray casting downward at length 10 hit something
        RaycastHit hit;
        if (Physics.Raycast(ds.rb.gameObject.transform.position, -Vector3.up, out hit, 10))
        {
            // if ray is not hitting track layer, ignore the collision
            if (hit.collider.gameObject.layer != 9)
            {
                //Physics.IgnoreCollision(hit.collider, carBody.GetComponent<Collider>());
                Debug.Log("Car game object is colliding with " + hit.collider.gameObject.name + " on layer " + hit.collider.gameObject.layer);
            }
            // if ray is hitting track layer
            if (hit.collider.gameObject.layer == 9)
            {
                Debug.Log("Car game object is colliding with " + hit.collider.gameObject.name + " on layer " + hit.collider.gameObject.layer);
            }
        }
        Debug.DrawRay(ds.rb.gameObject.transform.position, -Vector3.up, Color.red);
    }
}
