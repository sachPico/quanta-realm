using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirToAirLaserV2 : MonoBehaviour
{
    [Range(2,45)]
    public int details;
    public float defaultRadius;
    public GameObject redLaser;
    public GameObject blueLaser;
    public GameObject debug;

    private float diameter;
    private float distance;
    private float maxDiameter;
    private float powRadius;
    private float radius;
    private float vOffset;
    private int multiplier = 1;

    private Vector3 tmp = Vector3.zero;

    private float[] powDistances;
    
    private Vector3[] offset;

    private void Start()
    {
        //Precompute();
    }

    private void OnEnable()
    {
        radius = defaultRadius;
        powRadius = Mathf.Pow(radius, 2);
        diameter = 2 * radius;
        maxDiameter = diameter;
    }

    private void FixedUpdate()
    {
        if (debug.transform.localPosition.x >= maxDiameter)
        {
            radius = defaultRadius * 0.5f;
            diameter = radius * 2;
            powRadius = Mathf.Pow(radius, 2);
            multiplier *= -1;
            maxDiameter += diameter;
        }

        distance = Mathf.Abs(debug.transform.localPosition.x % diameter - radius);
        vOffset = Mathf.Sqrt(powRadius - Mathf.Pow(distance, 2));

        tmp.x = debug.transform.localPosition.x;
        tmp.y = vOffset * multiplier;

        redLaser.transform.localPosition = tmp;
        tmp.y *= -1;
        blueLaser.transform.localPosition = tmp;
        //Debug.Log("Radius: " + radius+", diameter: "+diameter + ", debug local X: " + debug.transform.localPosition.x + ", debug mod local x: " + debug.transform.localPosition.x % diameter + ", distance: " + distance + ", offset: " + vOffset);
    }

}