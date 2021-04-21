﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUtility : MonoBehaviour
{
    [System.Serializable]
    public struct Spawner
    {
        public Vector3 position;
        public float direction;
    }

    [System.Serializable]
    public struct WeaponType
    {
        public string weaponName;
        public List<int> weaponAttack;
        public List<float> weaponFireRate;
        public List<Spawner> spawner;
        public int initialPoolSize;
    }

    public Transform player;
    public WeaponType[] mainWeaponTypes;

    [Header("Main Weapon")]
    public int mainWeaponID;
    public int mainWeaponPower;

    [Header("Sub Weapon")]
    public int subWeaponID;
    public int subWeaponPower;

    public int activeMainWeaponAttack;
    public float activeMainWeaponFireRate;

    public static WeaponUtility instance;

    private WeaponType activeWeaponType;
    private float fireCounter = 0;

    public void Shoot()
    {
        if (fireCounter >= activeMainWeaponFireRate)
        {
            Vector3 spawnWorldPos;
            foreach (var spawners in activeWeaponType.spawner)
            {
                spawnWorldPos = Playfield.instance.playerPivot.TransformVector(spawners.position);
                Shoot(activeWeaponType.weaponName, player.position + spawnWorldPos, spawners.direction);
            }
            fireCounter = 0;
        }
        else
        {
            fireCounter += Time.deltaTime;
        }
    }

    public void ResetFireCounter()
    {
        fireCounter = activeMainWeaponFireRate;
    }

    public void MainPowerUp()
    {
        mainWeaponPower = Mathf.Clamp(mainWeaponPower + 1, 0, 10);
        activeMainWeaponFireRate = activeWeaponType.weaponFireRate[mainWeaponPower];
        activeMainWeaponAttack = activeWeaponType.weaponAttack[mainWeaponPower];
    }

    public void MainPowerDown()
    {
        mainWeaponPower = Mathf.Clamp(mainWeaponPower - 1, 0, 10);
        activeMainWeaponFireRate = activeWeaponType.weaponFireRate[mainWeaponPower];
        activeMainWeaponAttack = activeWeaponType.weaponAttack[mainWeaponPower];
    }

    void Shoot(string poolName, Vector3 spawnPosition, float spawnDirection)
    {
        BulletProperty bp = PoolHandler.instance.RequestObject(poolName).GetComponent<BulletProperty>();
        bp.transform.position = spawnPosition;
        bp.transform.localEulerAngles = Vector3.forward * spawnDirection;
        bp.gameObject.SetActive(true);
    }

    private void SetActiveMainWeapon()
    {
        activeWeaponType = mainWeaponTypes[mainWeaponID];

        activeMainWeaponFireRate = activeWeaponType.weaponFireRate[0];
        activeMainWeaponAttack = activeWeaponType.weaponAttack[0];
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        SetActiveMainWeapon();

        if(mainWeaponID >= mainWeaponTypes.Length)
        {
            Debug.LogException(new System.Exception("A weaponID is larger than the weapon type array size"));
        }
        else
        {
            SHMUPObjectPool.playfieldTransform = GameObject.FindGameObjectWithTag("Playfield").transform;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            MainPowerUp();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            MainPowerDown();
        }
    }
}