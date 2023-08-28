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

    float wantedRotationAngle;
    float wantedHeight;
    Quaternion currentRotation;

    // firstPlayer perspective is an int and not bool because PlayerPrefs does 
    // not have a SetBool method, but a SetInt method
    public int firstPlayer = 1;

    void Start()
    {
        //// if "FirstPlayer" key exists in PlayerPrefs
        //if (PlayerPrefs.HasKey("FirstPlayer"))
        //{
        //    // sets firstPlayer to whatever is saved in PlayerPrefs
        //    firstPlayer = PlayerPrefs.GetInt("FirstPlayer");
        //}

        wantedRotationAngle = target.eulerAngles.y;
        wantedHeight = target.position.y + height;
        currentRotation = Quaternion.Euler(0, wantedRotationAngle, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ToggleLerpCameraSwitch();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            firstPlayer += 1;
            //PlayerPrefs.SetInt("FirstPlayer", firstPlayer);
            if (firstPlayer > 3)
            {
                firstPlayer = 1;
            }
            ToggleInstantCameraSwitch();
            Debug.Log("FirstPlayer " + firstPlayer);
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    ToggleInstantCameraSwitch();
        //    firstPlayer = 3;
        //}
        //else firstPlayer = 2;
    }

    void ToggleInstantCameraSwitch()
    {
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;
        Quaternion currentRotation = Quaternion.Euler(0, wantedRotationAngle, 0);

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
        // toggle third person camera perspective (2 is in front, 3 is behind of car)
        else if (firstPlayer == 2 || firstPlayer == 3)
        {
            if (firstPlayer == 2) currentRotation = Quaternion.Euler(0, wantedRotationAngle, 0);
            else if (firstPlayer == 3) currentRotation = Quaternion.Euler(0, wantedRotationAngle + 180f, 0);

            FlipCamera(currentRotation, wantedHeight);
        }
    }

    void ToggleLerpCameraSwitch()
    {
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

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
        // toggle third person camera perspective to look forward
        else if (firstPlayer == 2)
        {
            //currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
            FlipCamera(currentRotation, currentHeight);
        }
        // toggle third player camera perspective to look behind car
        else if (firstPlayer == 3)
        {
            //wantedRotationAngle = target.eulerAngles.y + 180f;
            //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle + 180f, rotationDamping * Time.deltaTime);
            //currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
            currentRotation = Quaternion.Euler(0, wantedRotationAngle + 180f, 0);
            FlipCamera(currentRotation, currentHeight);
        }
    }

    void FlipCamera(Quaternion currentRotation, float currentHeight)
    {
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x,
                                currentHeight + heightOffset,
                                transform.position.z);

        transform.LookAt(target);
    }
}
