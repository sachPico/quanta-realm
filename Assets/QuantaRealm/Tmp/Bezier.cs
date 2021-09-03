using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bezier : MonoBehaviour
{
    public Vector3[] anchorPoints;
}


[CustomEditor(typeof(Bezier)), CanEditMultipleObjects]
public class BezierEditor : Editor
{
    Bezier dst;

    private void OnSceneGUI()
    {
        Draw();
    }

    private void OnEnable()
    {
        dst = (Bezier)target;
    }

    void Draw()
    {
        for(int i=0; i<dst.anchorPoints.Length; i++)
        {
            Handles.color = Color.white;
            dst.anchorPoints[i] = Handles.FreeMoveHandle(dst.anchorPoints[i], Quaternion.identity, .2f, Vector3.zero, Handles.SphereHandleCap);
            Handles.color = Color.green;
            if(i!=dst.anchorPoints.Length-1)
            {
                Handles.DrawLine(dst.anchorPoints[i], dst.anchorPoints[i + 1]);
            }
        }
    }
}