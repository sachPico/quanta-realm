using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubPatternSpawner
{
    public SubPatternBase subPatternProfile;

    public SubPatternSpawner(SubPatternBase _subPatternProfile)
    {
        subPatternProfile = _subPatternProfile;
    }
}
