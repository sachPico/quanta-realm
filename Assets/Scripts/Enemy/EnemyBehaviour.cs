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
        gameObject.SetActive(false);
    }

    public void OnAttack()
    {
        //Meh
    }

    void FixedUpdate()
    {
        //Debug.Log(enemyMoveBehaviour);
        transform.localPosition = enemyMoveBehaviour.Move(initialSpawnPoint, ref timer, -10f, 180f, parameterA, parameterB, parameterC);
    }
}
