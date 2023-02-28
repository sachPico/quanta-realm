using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayfieldObject : MonoBehaviour
{
    [HideInInspector]
    public Vector3 input;

    private Vector3 relativePos;
    private Vector3 relativeVelocity;

    Vector3 calcRelativeForward1;
    Vector3 calcRelativeForward2;

    public Vector3 RelativeForward
    {
        get
        {
            calcRelativeForward1 = transform.position;
            calcRelativeForward2 = transform.position + transform.right;

            calcRelativeForward1 = Playfield.instance.playerPivot.InverseTransformPoint(calcRelativeForward1);
            calcRelativeForward2 = Playfield.instance.playerPivot.InverseTransformPoint(calcRelativeForward2);

            return (calcRelativeForward2 - calcRelativeForward1).normalized;
        }
    }

    public Vector3 RelativePos
    {
        get
        {
            //return relativePos;
            return Playfield.instance.playerPivot.InverseTransformPoint(transform.position);
        }
        set
        {
            relativePos = value;
            if (Playfield.instance != null)
                transform.position = Playfield.instance.playerPivot.TransformPoint(value);
            else
                transform.position = relativePos;
        }
    }

    public Vector3 RelativeVelocity
    {
        get
        {
            return relativeVelocity;
        }
        set
        {
            relativeVelocity = value;
        }
    }

    private void FixedUpdate()
    {
        if (relativeVelocity == Vector3.zero)
        {
            relativePos = Playfield.instance.playerPivot.InverseTransformPoint(transform.position);
        }
    }
}
