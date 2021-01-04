using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    // [HideInInspector]
    public float debugModifyBorder;
    public Vector3 leftBottomBorder, rightUpperBorder;
    public Transform qCamPivot;
    public Transform playerPivot;
    public float camPivotRotateRange;

    Vector3 tmp;
    void Start()
    {
        Debug.Log(tmp);
        leftBottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z))) + new Vector3(1,1,0)*debugModifyBorder;
        rightUpperBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(Camera.main.transform.position.z))) - new Vector3(1,1,0)*debugModifyBorder;
    }
}
