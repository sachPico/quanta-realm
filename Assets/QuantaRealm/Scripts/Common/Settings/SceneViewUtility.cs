using PathFollower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SceneViewUtility
{
    static Event guiEvent = Event.current;

    static int anchorIDOffset = 1000;

    public static void UpdateBezier(BezierCurve curve, bool allowModifyFirstPoint, ref bool isModifying, ref int activeAnchorIndex, float sphereCapRadius)
    {
        ModifyBezier(curve, allowModifyFirstPoint, ref isModifying, ref activeAnchorIndex, sphereCapRadius);
        DrawBezier(curve, allowModifyFirstPoint, ref isModifying, ref activeAnchorIndex);
    }

    private static void DrawBezier(BezierCurve curve, bool allowModifyFirstPoint, ref bool isModifying, ref int activeAnchorIndex)
    {
        /*if (points.Length >= 4 && dst.visualize)
        {*/
            int loops;
            loops = (curve.points.Count - 1) / 3;
            for (int i = 0; i < loops; i++)
            {
                int startAnchorIndex = i * 3;
                int endAnchorIndex = i * 3 + 3;
                Handles.DrawBezier(curve.points[startAnchorIndex], curve.points[endAnchorIndex], curve.points[startAnchorIndex + 1], curve.points[startAnchorIndex + 2], Color.white, null, 2f);
            }
        /*}*/

        /*Handles.color = Color.blue;
        Handles.FreeMoveHandle(dst.GetPoint(.5f), Quaternion.identity, .1f, Vector3.zero, Handles.SphereHandleCap);*/
    }

    private static void ModifyBezier(BezierCurve curve, bool allowModifyFirstPoint, ref bool isModifying, ref int activeAnchorIndex, float sphereCapRadius)
    {
        #region Add Bezier Points

        if (guiEvent.type == EventType.MouseDown && guiEvent.control)
        {
            curve.AddSegment(HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin);
        }

        #endregion

        #region Moving Bezier Points
        for (int i = 0; i < curve.points.Count; i++)
        {
            Handles.color = Color.green;
            if (i != curve.points.Count - 1 && (i - 1) % 3 != 0)
            {
                Handles.DrawLine(curve.points[i], curve.points[i + 1]);
            }

            if (!allowModifyFirstPoint)
            {
                if (i == 0)
                {
                    Handles.color = Color.blue;
                    Handles.FreeMoveHandle(curve.points[i], Quaternion.identity, sphereCapRadius, Vector3.zero, Handles.SphereHandleCap);
                    continue;
                }
            }

            if (i % 3 == 0)
            {
                Handles.color = Color.white;
                Vector3 newPos = Handles.FreeMoveHandle(i / 3 + anchorIDOffset, curve.points[i], Quaternion.identity, sphereCapRadius, Vector3.zero, Handles.SphereHandleCap);
                Vector3 dist = newPos - curve.points[i];

                curve.points[i] = newPos;
                if (i != curve.points.Count - 1)
                {
                    curve.points[i + 1] += dist;
                }
                if (i != 0)
                {
                    curve.points[i - 1] += dist;
                }
            }
            else
            {
                Handles.color = Color.red;
                Vector3 newPos = Handles.FreeMoveHandle(curve.points[i], Quaternion.identity, sphereCapRadius, Vector3.zero, Handles.SphereHandleCap);
                Vector3 dist = newPos - curve.points[i];
                curve.points[i] = newPos;

                if (i % 3 == 1 && i / 3 > 0)
                {
                    curve.points[i - 2] = 2 * curve.points[i - 1] - curve.points[i];
                }
                if (i % 3 == 2 && i / 3 < (curve.points.Count / 3) - 1)
                {
                    Vector3 toAnchor = curve.points[i] - curve.points[i + 1];
                    curve.points[i + 2] = curve.points[i + 1] - toAnchor;
                }
            }
        }

        if (HandleUtility.nearestControl.Within(999, 1499))
        {
            if (guiEvent.type == EventType.Used)
            {
                isModifying = true;
                activeAnchorIndex = (HandleUtility.nearestControl - 1000) * 3;
            }
            else if (guiEvent.type == EventType.MouseMove)
            {
                isModifying = false;
            }
        }
        #endregion
    }
}
