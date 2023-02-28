using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class Extensions
{
    #region String
    public static string GetOrdinal(this int num)
    {
        if (num <= 0) return num.ToString();

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }
    #endregion

    #region Float
    public static bool Within(this float _value, float min, float max)
    {
        return _value > min && _value <= max;
    }

    public static bool Between(this float _value, float min, float max)
    {
        return _value > min && _value < max;
    }

    public static float Remap(this float _value, float from1, float to1, float from2, float to2)
    {
        return ((_value - from1) / (to1 - from1) * (to2 - from2)) + from2;
    }
    #endregion

    #region Int
    public static bool Within(this int _value, int min, int max)
    {
        return _value > min && _value <= max;
    }
    #endregion
}
