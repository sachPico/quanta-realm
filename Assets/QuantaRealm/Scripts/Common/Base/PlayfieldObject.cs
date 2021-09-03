using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayfieldObject : MonoBehaviour
{
    [HideInInspector]
    public Vector3 input;
    public Vector3 relativePos;

    public Vector3 RelativePos
    {
        get
        {
            return relativePos;
        }
        set
        {
            relativePos = value;
            transform.position = Playfield.instance.playerPivot.TransformPoint(value);
        }
    }
}
