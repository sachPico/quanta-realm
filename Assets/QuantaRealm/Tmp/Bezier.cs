using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct BezierPoints
{
    public List<Vector3> point;
}

public class Bezier : MonoBehaviour
{
    [SerializeField]
    public BezierPoints bezierPoints;

    public bool visualize;

    public Vector3 GetPoint (float t)
    {
        int segmentIndex = (int)t;
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;

        return
            oneMinusT * oneMinusT * oneMinusT * bezierPoints.point[0+segmentIndex] +
            3f * oneMinusT * oneMinusT * t * bezierPoints.point[1 + segmentIndex] +
            3f * oneMinusT * t * t * bezierPoints.point[2 + segmentIndex] +
            t * t * t * bezierPoints.point[3 + segmentIndex];
    }

    public static Vector3 GetPoint(List<Vector3> bezierCurvePoints, float t)
    {
        int segmentIndex = (int)t;
        t = t-segmentIndex;
        float oneMinusT = 1f - t;

        return
            oneMinusT * oneMinusT * oneMinusT * bezierCurvePoints[0 + segmentIndex*3] +
            3f * oneMinusT * oneMinusT * t * bezierCurvePoints[1 + segmentIndex*3] +
            3f * oneMinusT * t * t * bezierCurvePoints[2 + segmentIndex*3] +
            t * t * t * bezierCurvePoints[3 + segmentIndex*3];
    }
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
        SceneViewUtility.DrawBezier(dst.bezierPoints.point, true);
    }
}