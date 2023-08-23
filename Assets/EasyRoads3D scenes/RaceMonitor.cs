using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;
    public static bool racing = false;

    /// <summary>
    /// Itnitially sets each countdown item inactive, and then calls the
    /// IEnumerator PlayCountDown
    /// </summary>
    void Start()
    {
        foreach(GameObject g in countDownItems)
        {
            g.SetActive(false);
        }
        StartCoroutine(PlayCountDown());
    }

    /// <summary>
    /// Displays countdown. Waits 2 seconds, then displays each countdown item
    /// for 1 second and turns each off. Then allows AI to drive by setting 
    /// static racing variable to true.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayCountDown()
    {
        yield return new WaitForSeconds(2);
        foreach(GameObject g in countDownItems)
        {
            g.SetActive(true);
            yield return new WaitForSeconds(1);
            g.SetActive(false);
        }
        racing = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
