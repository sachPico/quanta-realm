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
        leftBottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z))) + new Vector3(1,1,0)*debugModifyBorder;
        rightUpperBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(Camera.main.transform.position.z))) - new Vector3(1,1,0)*debugModifyBorder;
    }
    void Update()
    {
        //transform.position += transform.right * 5 * Time.fixedDeltaTime;
    }
}
