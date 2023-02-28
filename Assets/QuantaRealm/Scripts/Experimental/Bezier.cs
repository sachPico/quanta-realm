using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;

[System.Serializable]
public class BezierCurve
{
    public List<Vector3> points;
    [HideInInspector] public Vector3[] evenlySpacedPoints;
    [HideInInspector] public float[] LUT;
    [HideInInspector] public float[] LUTPerSegment;
    [HideInInspector] public float approximatedLUT;

    public int CurveCount
    {
        get
        {
            return (points.Count - 1) / 3;
        }
    }

    public void Init()
    {
        points = new List<Vector3>();

        points.Add(Vector3.zero);
        points.Add(Vector3.right);
        points.Add(Vector3.right * 2);
        points.Add(Vector3.right * 3);
    }

    public void CalculateTotalLUTBySegments(int subdivision)
    {
        float dT = 1f / subdivision;
        float previousLength = 0f;

        LUT = new float[subdivision * CurveCount];
        LUTPerSegment = new float[CurveCount];

        for (int i = 0; i < CurveCount; i++)
        {
            for (int j = 0; j < subdivision; j++)
            {
                LUT[i*subdivision + j] = previousLength + (GetPoint(i, (j + 1) * dT) - GetPoint(i, j * dT)).magnitude;
                previousLength = LUT[i * subdivision + j];
            }

            LUTPerSegment[i] = previousLength;
        }

        approximatedLUT = LUT[LUT.Length - 1];
    }

    public float ArcLengthToTime(int curveIndex, float _distance, int subdivision)
    {
        if (_distance == 0f)
        {
            return 0;
        }

        float arcLength = LUTPerSegment[LUTPerSegment.Length - 1];
        float min;
        float max;

        int startIndex = Mathf.Clamp((curveIndex * subdivision) - 1, 0, curveIndex*subdivision);

        if (curveIndex!=0)
        {
            min = LUT[startIndex];
            max = LUT[startIndex+1];
            startIndex++;
        }
        else
        {
            min = 0f;
            max = LUT[0];
        }

        for(int i = 0; i<subdivision; i++)
        {
            if (_distance.Within(min, max))
            {
                return _distance.Remap(min, max, i / (float)subdivision, (i + 1) / (float)subdivision);
            }

            startIndex++;

            min = max;
            max = LUT[startIndex];
        }

        return _distance / arcLength;
    }

    public Vector3 GetPoint(int curveIndex, float t)
    {
        float oneMinusT = 1f - t;
        
        return oneMinusT * oneMinusT * oneMinusT * points[0 + curveIndex*3] +
            3 * oneMinusT * oneMinusT * t * points[1 + curveIndex*3] +
            3 * oneMinusT * t * t * points[2 + curveIndex*3] +
            t * t * t * points[3 + curveIndex*3];
    }

    public void SetPoint(int curveIndex, float t)
    {

    }

    public Vector3[] EvenlySpacedPointByDistance(float _distance, int subdivision)
    {
        if (_distance <= 0) return new Vector3[] { Vector3.zero };

        CalculateTotalLUTBySegments(subdivision);

        float t;
        float ds = _distance;
        float distance = ds;
        float excess = 0f;
        float max;
        float min;

        int index = 0;
        int cuts = (int)(LUT[LUT.Length - 1] / _distance);

        Vector3[] result = new Vector3[cuts];
        
        for(int i=0; i<cuts; i++)
        {
            max = LUTPerSegment[0];
            min = 0f;

            index = 0;

            for (int j = 0; j<CurveCount; j++)
            {
                if(distance.Within(min, max))
                {
                    t = ArcLengthToTime(j, distance, subdivision);
                    result[i] = GetPoint(j, t);

                    break;
                }

                index = Mathf.Clamp(index+1, 0, LUTPerSegment.Length);
                min = max;
                max = LUTPerSegment[index];
            }

            distance += ds;
        }

        evenlySpacedPoints = result;

        return result;
    }

    public void AddSegment(Vector3 newAnchorPos)
    {
        if (1 + (points.Count - 1) / 3 < 499)
        {
            Vector3 controlVector = points[points.Count - 1] - points[points.Count - 2];
            Vector3 p;

            p = points[points.Count - 1] + controlVector;
            points.Add(p);
            p = newAnchorPos;
            points.Add(p);
            p = newAnchorPos + controlVector;
            points.Add(p);
        }
    }

    public void DeleteSegment(int index)
    {
        points.Remove(points[points.Count - 1]);
        points.Remove(points[points.Count - 1]);
        points.Remove(points[points.Count - 1]);
    }
}

public class Bezier : MonoBehaviour
{
    [SerializeField] public BezierCurve curve;

    public Vector3 activeAnchorPosition;
    public Vector3 activeControlPoint1;
    [ConditionalField("hasTwoControlPoints", false)] public Vector3 activeControlPoint2;

    [Space(8f)]

    public bool lockAnchors;
    public int subdivision;
    public float perSegmentLength;

    [HideInInspector] public int activeAnchorIndex;
    [HideInInspector] public bool isModifying;
    [HideInInspector] public bool hasTwoControlPoints;

    [Space(8f)]

    public bool showCurve;
    public bool showRasterizedCurve;
    public float gizmosSphereRadius;

    public int ActiveAnchorIndex
    {
        get
        {
            return Mathf.Clamp(activeAnchorIndex, 0, (curve.points.Count - 1));
        }
        set
        {
            ActiveAnchorIndex = value;
        }
    }

    private void Reset()
    {
        curve.Init();
    }

    #region Editor

    public void OnDrawGizmos()
    {
        Vector3[] evenlySpacedPoints = curve.EvenlySpacedPointByDistance(perSegmentLength, subdivision);

        for (int i = 0; i < evenlySpacedPoints.Length; i++)
        {
            Gizmos.color = new Color(0f, 1f, 0f);
            Gizmos.DrawSphere(evenlySpacedPoints[i], gizmosSphereRadius);
        }

        if (showRasterizedCurve)
        {
            for (int i = 0; i < evenlySpacedPoints.Length - 1; i++)
            {
                Gizmos.color = new Color(0f, 0f, 1f);
                Gizmos.DrawLine(evenlySpacedPoints[i], evenlySpacedPoints[i + 1]);
            }
        }
    }

    public void OnValidate()
    {
        if(!isModifying)
        {
            if (lockAnchors)
            {
                curve.points[ActiveAnchorIndex] = activeAnchorPosition;

                hasTwoControlPoints = ActiveAnchorIndex == 0 || ActiveAnchorIndex == (curve.points.Count - 1) ? false : true;

                if (ActiveAnchorIndex == 0)
                {
                    curve.points[1] = activeControlPoint1;
                }
                else if (ActiveAnchorIndex == curve.points.Count - 1)
                {
                    curve.points[ActiveAnchorIndex - 1] = activeControlPoint2;
                }
            }
        }

        subdivision = Mathf.Clamp(subdivision, 1, 128);
        perSegmentLength = Mathf.Clamp(perSegmentLength, 0f, 100f);
        gizmosSphereRadius = Mathf.Clamp(gizmosSphereRadius, 0f, 100f);
    }

    #endregion
}


[CustomEditor(typeof(Bezier)), CanEditMultipleObjects]
public class BezierEditor : Editor
{
    Bezier dst;

    private void OnSceneGUI()
    {
        if (dst.showCurve)
        {
            Draw();
        }
    }

    private void OnEnable()
    {
        dst = (Bezier)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Remove Segment"))
        {
            dst.curve.DeleteSegment((dst.curve.points.Count - 1) / 3);
        }
    }

    void Draw()
    {
        SceneViewUtility.UpdateBezier(dst.curve, true, ref dst.isModifying, ref dst.activeAnchorIndex, dst.gizmosSphereRadius);

        if (!dst.isModifying)
        {
            dst.curve.points[dst.ActiveAnchorIndex] = dst.activeAnchorPosition;

            dst.hasTwoControlPoints = dst.ActiveAnchorIndex == 0 || dst.ActiveAnchorIndex == (dst.curve.points.Count - 1) ? false : true;

            if (dst.ActiveAnchorIndex == 0)
            {
                dst.curve.points[1] = dst.activeControlPoint1;
            }
            else if (dst.ActiveAnchorIndex == dst.curve.points.Count - 1)
            {
                dst.curve.points[dst.ActiveAnchorIndex - 1] = dst.activeControlPoint1;
            }

            if(dst.hasTwoControlPoints)
            {
                dst.curve.points[dst.ActiveAnchorIndex - 1] = dst.activeControlPoint1;
                dst.curve.points[dst.ActiveAnchorIndex + 1] = dst.activeControlPoint2;
            }
        }
        else
        {
            dst.activeAnchorPosition = dst.curve.points[dst.ActiveAnchorIndex];

            dst.hasTwoControlPoints = dst.ActiveAnchorIndex == 0 || dst.ActiveAnchorIndex == (dst.curve.points.Count - 1) ? false : true;

            if (dst.ActiveAnchorIndex == 0)
            {
                dst.activeControlPoint1 = dst.curve.points[1];
            }
            else if (dst.ActiveAnchorIndex == dst.curve.points.Count - 1)
            {
                dst.activeControlPoint1 = dst.curve.points[dst.ActiveAnchorIndex - 1];
            }

            if (dst.hasTwoControlPoints)
            {
                dst.activeControlPoint1 = dst.curve.points[dst.ActiveAnchorIndex - 1];
                dst.activeControlPoint2 = dst.curve.points[dst.ActiveAnchorIndex + 1];
            }
        }
    }
}