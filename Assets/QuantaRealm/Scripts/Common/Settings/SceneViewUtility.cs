using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SceneViewUtility
{
    public static void DrawBezier(List<Vector3> points, bool allowModifyFirstPoint)
    {
        for (int i = 0; i < points.Count; i++)
        {
            Handles.color = Color.green;
            if (i != points.Count - 1 && (i - 1) % 3 != 0)
            {
                Handles.DrawLine(points[i], points[i + 1]);
            }

            if (!allowModifyFirstPoint)
            {
                if (i == 0)
                {
                    Handles.color = Color.blue;
                    Handles.FreeMoveHandle(points[i], Quaternion.identity, 2f, Vector3.zero, Handles.SphereHandleCap);
                    continue;
                }
            }

            if (i % 3 == 0)
            {
                Handles.color = Color.white;
                Vector3 newPos = Handles.FreeMoveHandle(points[i], Quaternion.identity, 2f, Vector3.zero, Handles.SphereHandleCap);
                Vector3 dist = newPos - points[i];
                points[i] = newPos;
                if (i != points.Count - 1)
                {
                    points[i + 1] += dist;
                }
                if (i != 0)
                {
                    points[i - 1] += dist;
                }
            }
            else
            {
                Handles.color = Color.red;
                Vector3 newPos = Handles.FreeMoveHandle(points[i], Quaternion.identity, 2f, Vector3.zero, Handles.SphereHandleCap);
                Vector3 dist = newPos - points[i];
                points[i] = newPos;
                if (i % 3 == 1 && i / 3 > 0)
                {
                    points[i - 2] = 2 * points[i - 1] - points[i];
                }
                if (i % 3 == 2 && i / 3 < (points.Count / 3) - 1)
                {
                    Vector3 toAnchor = points[i] - points[i + 1];
                    points[i + 2] = points[i + 1] - toAnchor;
                }
            }
        }

        /*if (points.Length >= 4 && dst.visualize)
        {*/
            int loops = 0;
            loops = (points.Count - 1) / 3;
            for (int i = 0; i < loops; i++)
            {
                int startAnchorIndex = i * 3;
                int endAnchorIndex = i * 3 + 3;
                Handles.DrawBezier(points[startAnchorIndex], points[endAnchorIndex], points[startAnchorIndex + 1], points[startAnchorIndex + 2], Color.white, null, 2f);
            }
        /*}*/

        /*Handles.color = Color.blue;
        Handles.FreeMoveHandle(dst.GetPoint(.5f), Quaternion.identity, .1f, Vector3.zero, Handles.SphereHandleCap);*/
    }
}
