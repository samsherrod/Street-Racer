using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] int coinCount = 0;
    [SerializeField] UIManager uIManager;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(this.name + " collided with " + other.name);
        coinCount++;
        uIManager.SetScoreCount(coinCount);

        Debug.Log("CoinCount: " + coinCount);
    }
}
