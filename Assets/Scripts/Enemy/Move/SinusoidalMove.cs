using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMove : EnemyMoveBase
{
    public override Vector3 Move(Vector3 startPoint, ref float timer, params float[] sinusoidalParameters)
    {
        timer+=Time.fixedDeltaTime;
        float degreeCounter=timer * sinusoidalParameters[1];
        degreeCounter%=360f;
        degreeCounter*=Mathf.Deg2Rad;
        float rad2 = sinusoidalParameters[2] * Mathf.Deg2Rad;

        Vector3 localRight = Vector3.right * Mathf.Cos(rad2) + Vector3.up * Mathf.Sin(rad2);
        Vector3 localUp = Vector3.right * Mathf.Sin(rad2) - Vector3.up * Mathf.Cos(rad2);
        Vector3 output = (localRight * sinusoidalParameters[0] * timer) + (localUp * Mathf.Sin(degreeCounter)) + startPoint;
        
        return output;
    }
}
