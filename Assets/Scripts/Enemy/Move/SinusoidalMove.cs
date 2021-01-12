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
        Vector3 output = (Vector3.right * sinusoidalParameters[0] * timer) + (Vector3.up * Mathf.Sin(Mathf.Deg2Rad * degreeCounter)) + startPoint;
        
        return output;
    }
}
