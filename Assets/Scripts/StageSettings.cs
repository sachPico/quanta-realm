using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy { PK001, PK002 };

[Serializable]
public class StageEnemyProperties
{
    public Enemy enemy;
    public EnemyProperties<Ene> someObject;
}

[CreateAssetMenu(fileName = "stage_", menuName = "Stage")]
public class StageSettings : ScriptableObject
{
    public string stageName;

    public StageEnemyProperties[] stageEnemyProperties;

    private void OnValidate()
    {
        for (int i = 0; i < stageEnemyProperties.Length; i++)
        {
            switch (stageEnemyProperties[i].enemy)
            {
                case (Enemy.PK001): stageEnemyProperties[i].someObject.data = new PK001(); break;

                case (Enemy.PK002): stageEnemyProperties[i].someObject.data = new PK002(); break;
            }
        }
    }
}
