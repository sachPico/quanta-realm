using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float health;
    public int itemSpawn;
    
    public abstract void OnDestroyed();
    public abstract void OnAttack();
    public abstract void OnHit(float damage);
}

public class EnemyBehaviour : EnemyBase
{
    public override void OnHit(float damage)
    {
        health -= damage * Time.deltaTime;
        if (health < 0)
        {
            OnDestroyed();
        }
    }

    public override void OnDestroyed()
    {
        //TEMPORARY
        gameObject.SetActive(false);
    }

    public override void OnAttack()
    {
        //Meh
    }
}
