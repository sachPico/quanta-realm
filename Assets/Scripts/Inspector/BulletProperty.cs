using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperty : MonoBehaviour
{
    public float bulletSpeed;
    public float direction;

    void FixedUpdate()
    {
        transform.localPosition += (Vector3.right * Mathf.Cos(Mathf.Deg2Rad * direction)) + (Vector3.up * Mathf.Sin(Mathf.Deg2Rad * direction));

        if(Camera.main.WorldToViewportPoint(transform.position).x >= 1f || Camera.main.WorldToViewportPoint(transform.position).y >= 1f || Camera.main.WorldToViewportPoint(transform.position).x <=0f || Camera.main.WorldToViewportPoint(transform.position).y <=0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    
}
