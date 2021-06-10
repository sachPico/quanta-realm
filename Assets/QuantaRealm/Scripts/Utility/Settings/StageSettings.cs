using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType { Single, Wave};
public enum EnemyMoveEnum{Sinusoidal, Straight};

[Serializable]
public class StageEnemyProperties
{
    [HideInInspector]
    public string name;
    public EnemyMoveEnum enemyMoveType;
    public EnemyType enemy;
    public SpawnType spawnType;
    public SpawnPath spawnPath;
    public float spawnTime;
    public int spawnNumber;
    public Vector3 spawnPlayfieldPosition;
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
            stageEnemyProperties[i].name = stageEnemyProperties[i].enemy.ToString();
        }
    }
}
