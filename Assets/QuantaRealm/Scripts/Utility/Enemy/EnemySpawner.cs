using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public IEnumerator Spawn(string enemyKeyword, int spawnNumber, float interval)
    {
        for(int i=0; i<spawnNumber; i++)
        {
            EnemyBehaviour eb = PoolHandler.instance.RequestObject(enemyKeyword, false, transform.localPosition).GetComponent<EnemyBehaviour>();
            eb.Reset();
            eb.gameObject.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
        gameObject.SetActive(false);
        yield break;
     }
}