using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirToAirLaser : MonoBehaviour
{
    public float squaredRadius;
    public GameObject redLaser;
    public GameObject blueLaser;

    private float center;

    private float radius;
    private Vector3 redLocalPos;
    private Vector3 blueLocalPos;

    private void OnEnable()
    {
        radius = Mathf.Sqrt(squaredRadius);
        center = transform.localPosition.x + radius;
    }

    private void FixedUpdate()
    {
        Debug.Log(redLocalPos.x - center+":"+radius);
        if (transform.localPosition.x - center >= radius)
        {
            radius = squaredRadius / 4;
            center = transform.localPosition.x + radius;
            Debug.Log("Hubla");
        }

        redLocalPos.y = Mathf.Sqrt(Mathf.Abs(squaredRadius - Mathf.Pow(Mathf.Abs(transform.localPosition.x - center),2)));
        blueLocalPos.y = -redLocalPos.y;

        redLaser.transform.localPosition = redLocalPos;
        blueLaser.transform.localPosition = blueLocalPos;
    }
}
