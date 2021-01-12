using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUtility : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponType
    {
        public string weaponName;
        public GameObject bullet;
        public int initialSize;
        public float fireRate;
    }
    public WeaponType[] weaponTypes;
    public int weaponID;
    public static float activeFireRate;

    public static void Shoot(Vector3 spawnPosition, float spawnDirection)
    {
        //SHMUPObjectPool.instance.RequestPool(spawnPosition, spawnDirection);
        BulletProperty bp = PoolHandler.instance.RequestObject("BulletPool").GetComponent<BulletProperty>();
        bp.transform.position = spawnPosition;
        bp.transform.localEulerAngles = Vector3.forward * spawnDirection;
        bp.gameObject.SetActive(true);
    }
    void Start()
    {
        if(weaponID >= weaponTypes.Length)
        {
            Debug.LogException(new System.Exception("A weaponID is larger than the weapon type array size"));
        }
        else
        {
            SHMUPObjectPool.playfieldTransform = GameObject.FindGameObjectWithTag("Playfield").transform;
            //SHMUPObjectPool.instance.SetupPool(weaponTypes[weaponID].initialSize, weaponTypes[weaponID].bullet);
            activeFireRate = weaponTypes[weaponID].fireRate;
        }
    }
}