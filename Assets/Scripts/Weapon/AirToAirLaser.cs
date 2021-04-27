using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirToAirLaser : MonoBehaviour
{
    [Range(0,45)]
    public int details;
    public float radius;
    public GameObject redLaser;
    public GameObject blueLaser;
    public GameObject debug;

    private int index = 0;
    private int iterator = 1;
    private Vector3 blueLaserPos;
    public float[] direction;
    public Vector3[] offset;

    private void Precompute()
    {
        float initialDirection;
        float interval;
        float offsetDist;
        float tmp;

        interval = 180f / details;
        initialDirection = (180f - interval) / 2f;

        offsetDist = GetOffsetDist(interval);

        offset = new Vector3[details];
        direction = new float[details];
        for (int i = 0; i < details; i++)
        {
            direction[i] = Mathf.Deg2Rad * (initialDirection - interval * i);
            offset[i] = new Vector3(Mathf.Cos(direction[i]), Mathf.Sin(direction[i]), 0) * offsetDist;// * Time.fixedDeltaTime;
        }
    }

    private float GetOffsetDist(float intervalRad)
    {
        Vector3 a, b;
        a = new Vector3(Mathf.Cos(0), Mathf.Sin(0), 0) * radius;
        b = new Vector3(Mathf.Cos(intervalRad), Mathf.Sin(intervalRad), 0) * radius;
        return Vector3.Distance(a,b);
    }

    private void Start()
    {
        Precompute();
    }

    private void OnEnable()
    {
        redLaser.transform.localPosition = Vector3.zero;
        blueLaser.transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        redLaser.transform.localPosition += offset[index];
        blueLaserPos = redLaser.transform.localPosition;
        blueLaserPos.y *= -1;
        blueLaser.transform.localPosition = blueLaserPos;

        index += iterator;
        if(index >= details -1 || index <= 0)
        {
            iterator *= -1;
        }
    }

}
