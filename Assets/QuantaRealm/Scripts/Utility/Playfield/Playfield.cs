using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;
    // [HideInInspector]
    public float debugModifyBorder;
    public Vector3 leftBottomBorder, rightUpperBorder;
    public Transform tertiaryPivot;
    public Transform qCamPivot;
    public Transform playerPivot;
    public float camPivotRotateRange;

    Vector3 tmp;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    void Start()
    {
        // Debug.Log(tmp);
        leftBottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z))) + new Vector3(1,1,0)*debugModifyBorder;
        rightUpperBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(Camera.main.transform.position.z))) - new Vector3(1,1,0)*debugModifyBorder;
    }
}
