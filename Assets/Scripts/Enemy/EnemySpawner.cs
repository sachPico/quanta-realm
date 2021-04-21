// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemySpawner : MonoBehaviour
// {
//     public EnemyMoveBase spawnedEnemyMoveBehaviour;
    
//     public IEnumerator Spawn(string enemyKeyword, int spawnNumber, float interval, params float[] parameters)
//     {
//         for(int i=0; i<spawnNumber; i++)
//         {
//             EnemyBehaviour eb = PoolHandler.instance.RequestObject(enemyKeyword).GetComponent<EnemyBehaviour>();
//             eb.initialSpawnPoint = transform.localPosition;
//             eb.enemyMoveBehaviour = spawnedEnemyMoveBehaviour;
//             eb.parameterA = parameters[0];
//             eb.parameterB = parameters[1];
//             eb.parameterC = parameters[2];
//             eb.Reset();
//             eb.gameObject.SetActive(true);
//             yield return new WaitForSeconds(interval);
//         }
//         gameObject.SetActive(false);
//         yield break;
//     }
// }
