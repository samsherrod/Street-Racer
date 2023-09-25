using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.udemy.com/course/kart-racing/learn/lecture/16232580#overview
public class CheckPointManager : MonoBehaviour
{
    public int lap = 0;
    public int checkPoint = 1;
    int checkPointCount;
    int nextCheckPoint;
    public GameObject lastCheckpoint;

    [SerializeField] UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("checkpoint");
        checkPointCount = checkpoints.Length;
        foreach (GameObject c in checkpoints)
        {
            if (c.name == "0")
            {
                lastCheckpoint = c;
                break;
            }
        }
        checkPointCount = GameObject.FindGameObjectsWithTag("checkpoint").Length;
        if (uIManager) uIManager.SetLap(lap);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("checkpoint"))
        {
            int thisCPNumber = int.Parse(collider.gameObject.name);
            if (thisCPNumber == nextCheckPoint)
            {
                lastCheckpoint = collider.gameObject;
                checkPoint = thisCPNumber;
                if (checkPoint == 0)
                {
                    lap++;
                    if (uIManager) uIManager.SetLap(lap);
                }

                nextCheckPoint++;
                if(nextCheckPoint >= checkPointCount)
                {
                    nextCheckPoint = 0;
                }
            }
        }
    }
}
