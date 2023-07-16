﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public GameObject[] waypoints;

    private void OnDrawGizmos()
    {
        OnDrawGizmos(false);
    }

    private void OnDrawGizmosSelected()
    {
        OnDrawGizmos(true);
    }

    private void OnDrawGizmos(bool selected)
    {
        if (selected == false) return;
        if (waypoints.Length > 1)
        {
            Vector3 prev = waypoints[0].transform.position;
            for (int i = 1; i < waypoints.Length; i++)
            {
                Vector3 next = waypoints[i].transform.position;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
            Gizmos.DrawLine(prev, waypoints[0].transform.position);
        }
    }
}
