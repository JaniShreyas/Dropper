using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteraction : MonoBehaviour
{
    [SerializeField] string obstacleTag = "Obstacle";
    [SerializeField] string checkPointTag = "CheckPoint";

    private Vector3 lastCheckPoint;

    private void Awake()
    {
        lastCheckPoint = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacleTag))
        {
            print("Hit obstacle");
            ResetToLastCheckpoint();
        }
        if(other.CompareTag(checkPointTag))
        {
            print("CheckPoint");
            lastCheckPoint = other.transform.GetChild(0).position;
        }
    }

    private void ResetToLastCheckpoint()
    {
        transform.position = lastCheckPoint;
    }
}
