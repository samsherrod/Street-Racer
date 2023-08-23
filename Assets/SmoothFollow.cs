using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 8.0f;
    public float height = 1.5f;
    public float heightOffset = 1.0f;
    public float heightDamping = 4.0f;
    public float rotationDamping = 2.0f;
    public float fpLookAtDistance = 3;
    public float fpPosOffset = 0.4f;

    // firstPlayer perspective is an int and not bool because PlayerPrefs does 
    // not have a SetBool method, but a SetInt method
    int firstPlayer = -1;

    void Start()
    {
        // if "FirstPlayer" key exists in PlayerPrefs
        if (PlayerPrefs.HasKey("FirstPlayer"))
        {
            // sets firstPlayer to whatever is saved in PlayerPrefs
            firstPlayer = PlayerPrefs.GetInt("FirstPlayer");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
            return;
        
        // toggle first player camera perspective
        if (firstPlayer == 1)
        {
            // move camera 0.4 meters behind the target's position on the z axis and 1 meter up
            transform.position = target.position - target.forward * fpPosOffset + target.up;
            // keeps the camera looking straight and ahead
            transform.LookAt(target.position + target.forward * fpLookAtDistance);
        }
        // toggle third player camera perspective
        else
        {
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            transform.position = new Vector3(transform.position.x,
                                    currentHeight + heightOffset,
                                    transform.position.z);

            transform.LookAt(target);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            firstPlayer *= -1;
            PlayerPrefs.SetInt("FirstPlayer", firstPlayer);
        }
    }
}
