using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sachet.Utility
{
    public static class Utility
    {
        public static Vector3 GetPositionInPlayfieldSpace(Vector3 worldPos)
        {
            Vector3 result = Vector3.zero;

            result = StageManager.Instance.playfield.InverseTransformPoint(worldPos);

            return result;
        }

        public static Vector3 GetProjectedPositionInPlayfieldPlane(Vector3 worldPos)
        {
            Vector3 result = Vector3.zero;

            result = GetPositionInPlayfieldSpace(worldPos);
            result.z = 0;

            return result;
        }

        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        /// <summary>
        /// Convert degree value from longitude 0-360 to -180-180
        /// </summary>
        /// <returns></returns>
        public static float ConvertDegreeLongitude180(float value)
        {
            return value > 180f ? -360f + value : value;
        }
    }

    public class Timer
    {
        bool repeatable;
        float duration;
        float counter;

        Action onCounterFinished;
        bool finished;

        public Timer(float d, Action a, bool r)
        {
            duration = d;
            onCounterFinished = a;
            repeatable = r;
        }

        public void Update()
        {
            //if (finished) return;

            if(counter>=duration)
            {
                onCounterFinished?.Invoke();
                counter = 0f;

                //finished = !repeatable;
            }
            counter += Time.fixedDeltaTime;
        }

        public void Reset()
        {
            Debug.Log("HUBLA");
        }
    }

    public class StaticScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load(typeof(T).Name) as T;
                }
                return instance;
            }
        }
    }
}
