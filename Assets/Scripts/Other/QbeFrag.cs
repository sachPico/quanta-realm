using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QbeFrag : MonoBehaviour
{
    public int val;

    private float speed;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
        speed = Random.Range(1f, 5f);
    }

    private void FixedUpdate()
    {
        transform.localPosition -= Vector3.right * speed * Time.fixedDeltaTime;
    }
}
