using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float waitTime = 1f;

    private Transform currentTarget;
    private int currentPointIndex = 0;

    private void Start()
    {
        currentTarget = points[0];
    }

    private void Update()
    {
        if (transform.position == currentTarget.position)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;
            currentTarget = points[currentPointIndex];
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }

    //private IEnumerator Wait(int seconds, Action predicate)
    //{
    //    yield return new WaitForSeconds(seconds);
    //}
}
