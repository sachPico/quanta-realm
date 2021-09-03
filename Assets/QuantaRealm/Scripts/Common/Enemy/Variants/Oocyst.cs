using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OocystProperties : Property
{
    public float timeToAlterDirection;
    public Vector3 moveDirection;
}

public class Oocyst : EnemyBase
{
    Vector3 dir;
    Vector3 velocity;
    bool previouslySet = false;

    private void OnEnable()
    {
        if (previouslySet)
        {
            StartCoroutine(Turn());
        }
    }

    private void Start()
    {
        dir = Vector3.left;
        velocity = Vector3.zero;
        StartCoroutine(Turn());
        previouslySet = true;
    }

    IEnumerator Turn()
    {
        yield return new WaitForSeconds(((OocystProperties)enemyProperty).timeToAlterDirection);
        Vector3 target = ((OocystProperties)enemyProperty).moveDirection;
        Debug.Log(target);

        while (dir!=target)
        {
            dir = Vector3.SmoothDamp(dir, target, ref velocity, 1f);
            yield return null;
        }
        
        velocity = Vector3.zero;
        yield break;
    }

    void UpdateRelativePos()
    {
        RelativePos += dir.normalized * Time.fixedDeltaTime * speed;
    }

    private void FixedUpdate()
    {
        UpdateRelativePos();
    }

    private void OnDisable()
    {
        dir = Vector3.left;
        velocity = Vector3.zero;
        StopAllCoroutines();
    }
}
