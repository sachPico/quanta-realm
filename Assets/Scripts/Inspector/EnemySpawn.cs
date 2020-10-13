/*
TODO
- Add feature so that enemy spawner may read level editor's texture to determine where and what enemy should be spawned
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Serializable]
    public class SpawnProperty
    {
        public Vector3 spawnPosition;

        public int enemyID;
    }

    [Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public string enemyName;
        //public int health;
        //public short itemSpawn;
    }

    //Store all kinds of enemy types and for easy search
    //private Dictionary<string, EnemyType> enemyTypesDictionary;
    //Store all kinds of enemy types and available for edit in Inspector
    public List<EnemyType> enemyTypesList;
    //Store where and what enemy should be spawned
    public List<SpawnProperty> spawnPropertiesList;
    public Transform playfieldTransform;

    //private string tmpSpawnName;
    private GameObject tmpSpawnedEnemy;

    void Awake()
    {
        //Dunno why, but I feel that I should've decided whether I'm gonna use List or Dictionary only
        //Something tells me this is not an efficient method
        //enemyTypesDictionary = new Dictionary<string, EnemyType>();
        /*foreach (EnemyType et in enemyTypesList)
        {
            enemyTypesDictionary.Add(et._enemyName, et);
        }*/
    }

    void Start()
    {
        SpawnEnemies();
    }
    
    void SpawnEnemies()
    {
        for(int i=0; i<spawnPropertiesList.Count; i++)
        {
            tmpSpawnedEnemy = Instantiate(enemyTypesList[spawnPropertiesList[i].enemyID].enemyPrefab, playfieldTransform.TransformPoint(spawnPropertiesList[i].spawnPosition), Quaternion.identity, playfieldTransform);
            tmpSpawnedEnemy.name = enemyTypesList[spawnPropertiesList[i].enemyID].enemyName + "_" + i;
        }
    }

    void OnValidate()
    {
        foreach (var sp in spawnPropertiesList)
        {
            sp.enemyID = Mathf.Clamp(sp.enemyID, 0, enemyTypesList.Count-1);
        }
    }
}
