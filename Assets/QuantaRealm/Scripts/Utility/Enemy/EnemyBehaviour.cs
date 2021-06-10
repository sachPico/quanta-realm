using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : PlayfieldObject
{
    public int health;
    public int itemSpawn;
    public float timer = 0;

    //For movement parameter
    public Vector3 initialSpawnPoint;

    [Header("Qbe Spawn")]
    public int smallQbeFragSpawnNumber;
    public int mediumQbeFragSpawnNumber;
    public int largeQbeFragSpawnNumber;
    public int qbeSpawnNumber;

    public void Reset()
    {
        timer = 0;
        transform.localPosition = Vector3.zero;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            health -= WeaponUtility.instance.activeMainWeaponAttack;
            if (health < 0)
            {
                OnDestroyed();
            }
            other.gameObject.SetActive(false);
        }
    }

    public void OnDestroyed()
    {
        //TEMPORARY
        if (smallQbeFragSpawnNumber != 0) SpawnQbe(smallQbeFragSpawnNumber, "SmallQbe");
        if (mediumQbeFragSpawnNumber != 0) SpawnQbe(mediumQbeFragSpawnNumber, "MediumQbe");
        if (largeQbeFragSpawnNumber != 0) SpawnQbe(largeQbeFragSpawnNumber, "LargeQbe");
        if (qbeSpawnNumber != 0) SpawnQbe(qbeSpawnNumber, "Qbe");

        relativePos = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void OnAttack()
    {
        //Meh
    }

    private void SpawnQbe(int spawnNumber, string keyword)
    {
        PlayfieldObject qbe;
        for (int i = 0; i < spawnNumber; i++)
        {
            qbe = PoolHandler.instance.RequestObject(keyword, true).GetComponent<PlayfieldObject>();
            qbe.relativePos = relativePos + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            qbe.gameObject.SetActive(true);
        }
    }

    protected override void UpdateRelativePos()
    {
        relativePos += Vector3.left * Time.fixedDeltaTime * 30f;
        base.UpdateRelativePos();
    }

    void FixedUpdate()
    {
        UpdateRelativePos();
    }
}
