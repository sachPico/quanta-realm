using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperty : PlayfieldObject
{
    public Vector3 Velocity
    {
        get
        {
            return RelativeVelocity;
        }
        set
        {
            RelativeVelocity = value;
        }
    }
    public Turret Turret
    {
        get
        {
            return turret;
        }
        set
        {
            turret = value;
            startWorldPos = turret.transform.position;
        }
    }


    Turret turret;
    Vector3 distance;
    Vector3 startWorldPos;

    void FixedUpdate()
    {
        if (turret)
        {
            if (turret.stayInLocalSpace)
            {
                RelativePos = Playfield.instance.playerPivot.InverseTransformPoint(startWorldPos) + distance;
                distance += RelativeVelocity * Time.fixedDeltaTime;
            }
            else
            {
                RelativePos += RelativeVelocity * Time.fixedDeltaTime;
            }
        }
        else
        {
            RelativePos += RelativeVelocity * Time.fixedDeltaTime;
        }

        if(RelativePos.x >= StageManager.Instance.stageEnemySettings.generalStageSettings.maxStageBorder.x || RelativePos.x <= StageManager.Instance.stageEnemySettings.generalStageSettings.minStageBorder.x)
        //if(Camera.main.WorldToViewportPoint(transform.position).x >= 1f || Camera.main.WorldToViewportPoint(transform.position).y >= 1f || Camera.main.WorldToViewportPoint(transform.position).x <=0f || Camera.main.WorldToViewportPoint(transform.position).y <=0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        //startWorldPosnya hrs bisa ngikuti gerak envi-nya

        transform.localRotation = Quaternion.identity;
        startWorldPos = Vector3.zero;
        distance = Vector3.zero;
    }
}
