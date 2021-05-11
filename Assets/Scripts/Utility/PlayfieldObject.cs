using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldObject : MonoBehaviour
{
    public Vector3 relativePos;

    protected virtual void UpdateRelativePos()
    {
        transform.position = Playfield.instance.playerPivot.TransformPoint(relativePos);
    }
}
