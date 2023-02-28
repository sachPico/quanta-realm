using PathFollower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public IEnumerator Spawn(EnemySpawn _enemySpawn)
    {
        Vector3 randomizedSpawnPosition;

        for (int i = 0; i < _enemySpawn.spawnNumber; i++)
        {
            randomizedSpawnPosition.x = Random.Range(_enemySpawn.minSpawnPosition.x, _enemySpawn.maxSpawnPosition.x);
            randomizedSpawnPosition.y = Random.Range(_enemySpawn.minSpawnPosition.y, _enemySpawn.maxSpawnPosition.y);
            randomizedSpawnPosition.z = Random.Range(_enemySpawn.minSpawnPosition.z, _enemySpawn.maxSpawnPosition.z);

            EnemyBase eb = PoolHandler.instance.RequestObject(StageManager.Instance.enemyLibrary.EnemyList[_enemySpawn.enemyID], false, randomizedSpawnPosition).GetComponent<EnemyBase>();
            eb.Begin();
            //eb.Reset();
            eb.gameObject.SetActive(true);
            yield return new WaitForSeconds(_enemySpawn.spawnInterval);
        }

        yield break;
    }
}