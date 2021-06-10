using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMoveBase
{
    public virtual Vector3 Move(Vector3 sp, ref float t)
    {
        return Vector3.zero;
    }
}
