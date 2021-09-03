using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperty : MonoBehaviour
{
    public int attack;
    public float bulletSpeed;

    void FixedUpdate()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;

        if(Camera.main.WorldToViewportPoint(transform.position).x >= 1f || Camera.main.WorldToViewportPoint(transform.position).y >= 1f || Camera.main.WorldToViewportPoint(transform.position).x <=0f || Camera.main.WorldToViewportPoint(transform.position).y <=0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    
}
