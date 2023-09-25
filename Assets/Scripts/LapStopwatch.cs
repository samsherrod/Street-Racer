using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//https://www.youtube.com/watch?v=HLz_k6DSQvU
public class LapStopwatch : MonoBehaviour
{
    bool stopwatchActive = false;
    float currentTime;
    public TextMeshProUGUI currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        StartStopWatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopwatchActive == true)
        {
            currentTime += Time.deltaTime;
        }
        // check System time and increment the variable time - displays minutes and seconds
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
    }

    public void StartStopWatch()
    {
        stopwatchActive = true;
    }

    public void StopStopwatch()
    {
        stopwatchActive = false;
    }
}
