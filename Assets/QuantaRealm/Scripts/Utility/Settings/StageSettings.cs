using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType { Single, Wave};
public enum SpawnPosition {UpperCenter, CenterLeft, CenterRight, BottomCenter};

[Serializable]
public class StageEnemyProperties
{
    [HideInInspector]
    public string name;
    [SerializeField]
    public Spawn[] spawns;
    public float spawnTime;
}

[Serializable]
public struct Spawn
{
    [HideInInspector]
    public string name;
    public EnemyType enemy;
    public SpawnType spawnType;
    public SpawnPosition spawnPosition;
    /// <summary>
    /// By default, when you set SpawnPosition to UpperCenter or Bottom Center, the X-axis value will always be 0.
    /// Use this parameter to set the X-axis value. Same goes when you set as CenterRight or CenterLeft, this time Y-axis value is 0.
    /// </summary>
    [Range(-1f, 1f)]
    public float additionalAxisValue;
    /*public SpawnPath spawnPath;*/
    public float spawnerTime;
    public int spawnNumber;
}

[Serializable]
public class SpawnPath
{
    public Vector3[] paths;
}

[CreateAssetMenu(fileName = "stage_", menuName = "Stage")]
public class StageSettings : ScriptableObject
{
    public string stageName;
    public AnimationClip stageAnimation;
    public List<Vector3> nodePos;

    public StageEnemyProperties[] stageEnemyProperties;

    private void OnValidate()
    {
        for(int i=0; i<stageEnemyProperties.Length; i++)
        {
            for(int k=0; k < stageEnemyProperties[i].spawns.Length; k++)
            {
                stageEnemyProperties[i].name = i.ToString();
                stageEnemyProperties[i].spawns[k].name = stageEnemyProperties[i].spawns[k].enemy.ToString();
            }
        }
    }
}
