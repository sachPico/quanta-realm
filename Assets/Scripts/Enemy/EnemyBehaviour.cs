using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyMoveBase enemyMoveBehaviour;
    public int health;
    public int itemSpawn;
    public float timer = 0;

    //For movement parameter
    public Vector3 initialSpawnPoint;

    public void Reset()
    {
        timer = 0;
        transform.localPosition = enemyMoveBehaviour.Move(initialSpawnPoint, ref timer, -10f, 180f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            health -= other.GetComponent<BulletProperty>().attack;
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
        transform.localPosition = enemyMoveBehaviour.Move(initialSpawnPoint, ref timer, -10f, 180f);
    }
}
