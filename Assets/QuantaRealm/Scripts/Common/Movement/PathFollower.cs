using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PathFollower : MonoBehaviour
{
    public Vector3 location;
    public Vector3 velocity;

    [SerializeField]
    public Path[] paths;

    public int currentPathSegment;

    Vector3 predictedLocation
    {
        get
        {
            return location + velocity;
        }
    }

    Vector3 A
    {
        get
        {
            return predictedLocation - paths[currentPathSegment].startPoint;
        }
    }

    Vector3 B
    {
        get
        {
            return paths[currentPathSegment].endPoint - paths[currentPathSegment].startPoint;
        }
    }

    float ABCosinus
    {
        get
        {
            return Vector3.Dot(A, B) / (A.magnitude * B.magnitude);
        }
    }

    Vector3 Projection
    {
        get
        {
            float d = A.magnitude * ABCosinus;
            return paths[currentPathSegment].startPoint + (B.normalized * Vector3.Dot(A, B));
        }
    }

    *//*private void OnDrawGizmos()
    {
        //Draw paths
        Gizmos.color = Color.black;
        foreach (var p in paths)
        {
            Gizmos.DrawLine(p.startPoint, p.endPoint);
        }

        //Draw A
        Gizmos.color = Color.red;
        Gizmos.DrawLine(paths[currentPathSegment].startPoint, paths[currentPathSegment].startPoint + A);

        //Draw B
        Gizmos.color = Color.green;
        Gizmos.DrawLine(paths[currentPathSegment].startPoint, paths[currentPathSegment].startPoint + B);

        //Draw C
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(paths[currentPathSegment].startPoint, Projection);

        //Draw line from current path's start point to player position
        Gizmos.color = Color.white;
        Gizmos.DrawLine(paths[currentPathSegment].startPoint, location);
    }*//*
}*/
