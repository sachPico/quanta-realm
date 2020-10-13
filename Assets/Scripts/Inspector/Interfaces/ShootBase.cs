﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootBase : MonoBehaviour
{
    public float damage;
    public abstract void Shoot();

    public virtual void SetDamage(EnemyBehaviour eb)
    {
        eb.OnHit(damage);
    }
}