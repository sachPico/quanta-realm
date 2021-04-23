using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyMoveBase enemyMoveBehaviour;
    public int health;
    public int itemSpawn;
    public float timer = 0;

    public float parameterA;
    public float parameterB;
    public float parameterC;

    //For movement parameter
    public Vector3 initialSpawnPoint;

    [Header("Qbe Spawn")]
    public int smallQbeFragSpawnNumber;
    public int mediumQbeFragSpawnNumber;
    public int largeQbeFragSpawnNumber;
    public int qbeSpawnNumber;

    public void Reset()
    {
        timer = 0;
        transform.localPosition = enemyMoveBehaviour.Move(initialSpawnPoint, ref timer, -10f, 180f, parameterA, parameterB, parameterC);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            health -= WeaponUtility.instance.activeMainWeaponAttack;
            if (health < 0)
            {
                OnDestroyed();
            }
            other.gameObject.SetActive(false);
        }
    }

    public void OnDestroyed()
    {
        //TEMPORARY
        if (smallQbeFragSpawnNumber != 0) SpawnQbe(smallQbeFragSpawnNumber, "SmallQbe");
        if (mediumQbeFragSpawnNumber != 0) SpawnQbe(mediumQbeFragSpawnNumber, "MediumQbe");
        if (largeQbeFragSpawnNumber != 0) SpawnQbe(largeQbeFragSpawnNumber, "LargeQbe");
        if (qbeSpawnNumber != 0) SpawnQbe(qbeSpawnNumber, "Qbe");
        gameObject.SetActive(false);
    }

    public void OnAttack()
    {
        //Meh
    }

    private void SpawnQbe(int spawnNumber, string keyword)
    {
        Transform qbeTransform;
        for (int i = 0; i < spawnNumber; i++)
        {
            qbeTransform = PoolHandler.instance.RequestObject(keyword).transform;
            qbeTransform.localPosition = transform.localPosition + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            qbeTransform.gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(enemyMoveBehaviour);
        transform.localPosition = enemyMoveBehaviour.Move(initialSpawnPoint, ref timer, -10f, 180f, parameterA, parameterB, parameterC);
    }
}
