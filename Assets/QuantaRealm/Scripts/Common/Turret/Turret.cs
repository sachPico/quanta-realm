using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForwardAxis {XLocal, YLocal, ZLocal, NegXLocal, NegYLocal, NegZLocal }

public class Turret : PlayfieldObject
{
    public bool stayInLocalSpace;
    public PatternProfile patternProfile;
    public float shootInterval;

    float timer = 0;

    [HideInInspector] public bool isShooting;
    public void CalculateShooting()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= shootInterval)
        {
            foreach (var p in patternProfile.subPatterns)
            {
                p.profile.CalculateSubPattern(this);
            }
            timer = 0;
        }
    }

    public void Reset()
    {
        timer = 0;
    }

    //IEnumerator Shooting()
    //{

    //}
}
