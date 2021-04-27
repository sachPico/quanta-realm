using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirToAirLaser : MonoBehaviour
{
    public float squaredRadius;
    public GameObject redLaser;
    public GameObject blueLaser;
    public GameObject debug;

    private float center;

    private float sqrDist;
    private float radius;
    private float offset;
    private float multiplier = 1;
    private ParticleSystem particleSystem;
    private Vector3 redLocalPos;
    private Vector3 blueLocalPos;
    private Vector3 initialLocalPos;

    private void OnEnable()
    {
        squaredRadius = 64;
        radius = Mathf.Sqrt(squaredRadius);
        offset = radius;
        debug.transform.localPosition = new Vector3(offset, 0f, 0f);
        //Debug.Break();
    }

    private void Start()
    {
        //particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        sqrDist = Vector3.SqrMagnitude(transform.localPosition - debug.transform.localPosition);
        if (sqrDist >= squaredRadius)
        {
            squaredRadius = 16;
            offset+=radius;
            radius = Mathf.Sqrt(squaredRadius);
            offset+=radius;
            debug.transform.localPosition = new Vector3(offset, 0f, 0f);
            multiplier *= -1;
            // Debug.Break();
        }

        sqrDist = Vector3.SqrMagnitude(transform.localPosition - debug.transform.localPosition);;
        redLocalPos.y = (Mathf.Sqrt(Mathf.Abs(squaredRadius - sqrDist))) * multiplier;
        blueLocalPos.y = -redLocalPos.y;

        redLaser.transform.localPosition = redLocalPos;
        blueLaser.transform.localPosition = blueLocalPos;
    }

    private void OnDrawGizmos()
    {
    }
}
