using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericalMethodUtility : MonoBehaviour
{
    private static readonly (float, float)[] CubicQuadrature =
    {(-0.7745966F, 0.5555556F), (0, 0.8888889F), (0.7745966F, 0.5555556F)};

    public static float Integrate(Func<float, float> f, in float lowerBound, in float upperBound)
    {
        var sum = 0f;
        foreach (var (arg, weight) in CubicQuadrature)
        {
            var t = Mathf.Lerp(lowerBound, upperBound, Mathf.InverseLerp(-1, 1, arg));
            sum += weight * f(t);
        }

        return sum * (upperBound - lowerBound) / 2;
    }
}
