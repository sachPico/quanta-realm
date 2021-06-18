using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;
    public Transform qCamPivot;
    public Transform playerPivot;
    public float camPivotRotateRange;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
}
