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

    private float radius;
    private Vector3 redLocalPos;
    private Vector3 blueLocalPos;
    private Vector3 initialLocalPos;

    private void OnEnable()
    {
        squaredRadius = 64;
        radius = Mathf.Sqrt(squaredRadius);
        center = transform.localPosition.x + radius;
        initialLocalPos = transform.localPosition;
        //Debug.Break();
    }

    private void FixedUpdate()
    {
        Debug.Log(transform.localPosition.x - center+":"+radius);
        if (transform.localPosition.x - center >= radius)
        {
            squaredRadius = 16;
            radius = Mathf.Sqrt(squaredRadius);
            center = transform.localPosition.x + radius;
            initialLocalPos = transform.localPosition;
            Debug.Log("Hubla");
            //Debug.Break();
        }

        redLocalPos.y = Mathf.Sqrt(Mathf.Abs(squaredRadius - Mathf.Pow(Mathf.Abs(transform.localPosition.x - center),2)));
        //Debug.Log(redLocalPos.y);
        blueLocalPos.y = -redLocalPos.y;

        redLaser.transform.localPosition = redLocalPos;
        blueLaser.transform.localPosition = blueLocalPos;
        debug.transform.position = transform.parent.TransformPoint(initialLocalPos+Vector3.right*radius);// - new Vector3(center,0,0));
    }

    private void OnDrawGizmos()
    {
    }
}
