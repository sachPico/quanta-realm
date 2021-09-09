using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OocystProperties : Property
{
    public float timeToAlterDirection;
    public Vector3 moveDirection;
    public BezierPoints bezier;
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
        /*yield return new WaitForSeconds(((OocystProperties)enemyProperty).timeToAlterDirection);
        Vector3 target = ((OocystProperties)enemyProperty).moveDirection;
        Debug.Log(target);

        while (dir!=target)
        {
            dir = Vector3.SmoothDamp(dir, target, ref velocity, 1f);
            yield return null;
        }
        
        velocity = Vector3.zero;*/

        List<Vector3> curvePoints = ((OocystProperties)enemyProperty).bezier.point;

        float t = 0;
        float maxT = curvePoints.Count / 3;

        Debug.Log(maxT);

        while(t<=maxT)
        {
            RelativePos = Bezier.GetPoint(curvePoints, t);
            t += .05f * Time.fixedDeltaTime;
            yield return null;
        }

        gameObject.SetActive(false);

        yield break;
    }

    void UpdateRelativePos()
    {
        RelativePos += dir.normalized * Time.fixedDeltaTime * speed;
    }

    private void OnDisable()
    {
        dir = Vector3.left;
        velocity = Vector3.zero;
        StopAllCoroutines();
    }
}
