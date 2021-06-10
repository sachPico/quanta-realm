using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public IEnumerator Spawn(string enemyKeyword, int spawnNumber, float interval)
    {
        for(int i=0; i<spawnNumber; i++)
        {
            EnemyBehaviour eb = PoolHandler.instance.RequestObject(enemyKeyword, false).GetComponent<EnemyBehaviour>();
            Debug.Break();
            eb.Reset();
            eb.relativePos = transform.localPosition;
            eb.gameObject.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
        gameObject.SetActive(false);
        yield break;
     }
}