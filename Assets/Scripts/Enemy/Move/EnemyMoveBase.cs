using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMoveBase
{
    public EnemyMoveBase()
    {
        // Debug.Log("YEAH!");
    }
    public virtual Vector3 Move(Vector3 sp, ref float t, params float[] pl)
    {
        return Vector3.zero;
    }
}
