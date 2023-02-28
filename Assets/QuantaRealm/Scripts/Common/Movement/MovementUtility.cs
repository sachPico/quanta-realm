using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFollower
{
    [System.Serializable]
    public class Path
    {
        public Vector3 startPoint;
        public Vector3 endPoint;
        public Vector3 vector;

        public float angle;
        public float radius;
        public float magnitude;

        public Path(Vector3 s, Vector3 e, float r)
        {
            Initialize(s, e, r);
        }

        public void Initialize(Vector3 s, Vector3 e, float r)
        {
            startPoint = s;
            endPoint = e;
            radius = r;

            vector = endPoint - startPoint;
            angle = Vector3.SignedAngle(Vector3.right, vector, Vector3.forward);
            magnitude = vector.magnitude;
        }

        public void Initialize()
        {
            Initialize(startPoint, endPoint, radius);
        }
    }

    public static class PathFollowerUtility
    {
        /// <summary>
        /// Returns new steering force. Desired velocity is calculated based on target and object position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="maxSteerForce"></param>
        /// <returns></returns>
        public static Vector3 GetSteerForce(Vector3 position, Vector3 target, Vector3 currentVelocity, float maxDesiredSpeed, float maxSteerForce)
        {
            Vector3 desired = target - position;
            return GetSteerForce(desired, currentVelocity, maxDesiredSpeed, maxSteerForce);
        }

        /// <summary>
        /// Returns new steering force.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="maxSteerForce"></param>
        /// <returns></returns>
        public static Vector3 GetSteerForce(Vector3 desiredVelocity, Vector3 currentVelocity, float maxDesiredSpeed, float maxSteerForce)
        {
            Vector3 steer = Vector3.zero;

            steer = desiredVelocity.normalized * maxDesiredSpeed - currentVelocity;

            if (steer.magnitude > maxSteerForce)
            {
                steer = steer.normalized * maxSteerForce;
            }

            return steer;
        }
    }
}