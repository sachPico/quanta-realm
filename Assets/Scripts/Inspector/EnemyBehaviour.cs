using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health;
    public int itemSpawn;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("H");
        OnHit(other.GetComponent<BulletProperty>().attack);
    }
    public void OnHit(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            OnDestroyed();
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
    
}
