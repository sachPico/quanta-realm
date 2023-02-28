using Sachet.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyItem
{
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Amnisium", menuName = "EnemyLibrary")]
public class EnemyLibrary : ScriptableObject
{
    static EnemyLibrary instance;

    public static EnemyLibrary Instance
    {
        get
        {
            if(instance==null)
            {
                instance = Resources.Load<EnemyLibrary>("EnemyLibrary/EnemyLibrary");
            }
            return instance;
        }
    }

    [SerializeField]
    public List<EnemyItem> enemies = new List<EnemyItem>();

    public string[] EnemyList
    {
        get
        {
            string[] result = new string[enemies.Count];
            for(int i=0; i<enemies.Count; i++)
            {
                result[i] = enemies[i].name;
            }
            return result;
        }
    }
}