using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKEnemy : EnemyBase
{
    protected override void UpdateRelativePos()
    {
        relativePos += Vector3.left * Time.fixedDeltaTime * speed;
        base.UpdateRelativePos();
    }

    void FixedUpdate()
    {
        UpdateRelativePos();
    }
}
