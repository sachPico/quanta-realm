using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "generalStageSetting", menuName = "Amnisium/GeneralSetting")]
public class GeneralStageSettings : ScriptableObject
{
    public Vector2 minStageBorder;
    public Vector2 maxStageBorder;
}
