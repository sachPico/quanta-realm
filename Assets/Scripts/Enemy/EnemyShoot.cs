using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float minShootRate;
    public float maxShootRate;

    private float shootRate;
    private float timer=0f;

    private void SetShootRate()
    {
        shootRate = Random.Range(minShootRate, maxShootRate);
    }

    private void OnEnable()
    {
        SetShootRate();
    }

    private void Update()
    {
        if(timer >= shootRate)
        {
            SetShootRate();
            timer = 0;

            BulletProperty bp = PoolHandler.instance.RequestObject("DummyEnemyBullet").GetComponent<BulletProperty>();
            Vector3 directionToPlayer = WeaponUtility.instance.player.position - transform.position;
            float direction = Mathf.Clamp(Mathf.Rad2Deg * Mathf.Atan2(directionToPlayer.y, directionToPlayer.x), -180f, 180f);
            bp.transform.position = transform.position;
            bp.transform.localEulerAngles = Vector3.forward * direction;
            bp.gameObject.SetActive(true);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
