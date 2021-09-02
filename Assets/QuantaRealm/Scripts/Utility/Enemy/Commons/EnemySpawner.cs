using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*public EnemyMoveBase spawnedEnemyMoveBehaviour;*/
    public IEnumerator Spawn(string enemyKeyword, int spawnNumber, float interval, Vector3 spawnPlayfieldPos, Property enemyProperty)
    {
        for(int i=0; i<spawnNumber; i++)
        { 
            EnemyBase eb = PoolHandler.instance.RequestObject(enemyKeyword, false, spawnPlayfieldPos).GetComponent<EnemyBase>();
            eb.enemyProperty = enemyProperty;
            eb.Reset();
            eb.gameObject.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
        gameObject.SetActive(false);
        yield break;
     }
}