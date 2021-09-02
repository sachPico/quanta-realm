using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKEnemy : EnemyBase
{
    void UpdateRelativePos()
    {
        RelativePos += Vector3.left * Time.fixedDeltaTime * speed;
    }

    void FixedUpdate()
    {
        UpdateRelativePos();
    }
}
