using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    // [HideInInspector]
    public float debugModifyBorder;
    public Vector3 leftBottomBorder, rightUpperBorder;
    public Transform mainCamPivot;
    void Start()
    {
        // Matrix4x4 projection = Camera.main.projectionMatrix;
        // // projection.m00 = 0.99f;
        // // projection.m11 = 0.99f;
        // for(int i=0; i<4; i++)
        // {
        //     for(int j=0; j<4; j++)
        //     {
        //         Debug.Log(i+", "+j+":\t"+projection[i, j]);
        //     }
        // }

        // projection.m23 = 0;
        // Camera.main.projectionMatrix = projection;
        leftBottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z))) + new Vector3(1,1,0)*debugModifyBorder;
        rightUpperBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(Camera.main.transform.position.z))) - new Vector3(1,1,0)*debugModifyBorder;
    }
    void Update()
    {
        //transform.position += transform.right * 5 * Time.fixedDeltaTime;
    }
}
