using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayfieldObject : MonoBehaviour
{
    [HideInInspector]
    public Vector3 input;
    public Vector3 relativePos;

    protected virtual void UpdateRelativePos()
    {
        transform.position = Playfield.instance.playerPivot.TransformPoint(relativePos);
    }
}
