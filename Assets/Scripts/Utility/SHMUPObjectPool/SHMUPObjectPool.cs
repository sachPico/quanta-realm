using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    HOW TO USE
 */

public class SHMUPObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectProperty
    {
        public string objTag;
        public GameObject objPrefab;
        public int defaultSpawnNumber;
    }

    public bool isDynamic;
    public Transform poolParent;
    private static bool m_isDynamic;

    public static SHMUPObjectPool instance;
    public static Transform playfieldTransform;
    public static Dictionary<string, List<GameObject>> objectPool = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
    }

    public void SetupPool(int size, GameObject pooledObject)
    {
        List<GameObject> tmpPool = new List<GameObject>();
        GameObject tmp;
        for(int i=0; i<size; i++)
        {
            tmp = Instantiate(pooledObject, Vector3.zero, Quaternion.identity, poolParent);
            tmp.SetActive(false);
            tmpPool.Add(tmp);
        }
        objectPool.Add("MainWeapon", tmpPool);
    }

    public void RequestPool(Vector3 spawnPoint, float bulletDirection)
    {
        foreach(var bullet in objectPool["MainWeapon"])
        {
            if(bullet.activeInHierarchy)
            {
                continue;
            }
            else
            {
                bullet.transform.position = spawnPoint;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.transform.localEulerAngles = new Vector3(0,0,bulletDirection);
                // Debug.Log(bulletDirection);
                bullet.SetActive(true);
                //Debug.Log("No new object is added to \"MainWeapon\"");
                return;
            }
        }
        if(m_isDynamic)
        {
            GameObject tmp;
            tmp = Instantiate(objectPool["MainWeapon"][0], spawnPoint, Quaternion.identity, poolParent);
            tmp.transform.localEulerAngles = new Vector3(0,0,bulletDirection);
            // tmp.transform.rotation = Quaternion.AngleAxis(bulletDirection, tmp.transform.forward);
            //Debug.Log(bulletDirection);
            tmp.SetActive(false);
            objectPool["MainWeapon"].Add(tmp);
            tmp.SetActive(true);
            //Debug.Log("New object is added to \"MainWeapon\"");
        }
    }

    public void Start()
    {
        m_isDynamic = isDynamic;
        //IMPORTANT!!! Set playfieldTransform HERE!!!
        /*List<GameObject> tmpSpawnList = new List<GameObject>();
        GameObject tmpSpawnObj;
        for (int i = 0; i < objProp.Count; i++)
        {
            for (int j = 0; j < objProp[i].defaultSpawnNumber; j++)
            {
                tmpSpawnObj = Instantiate(objProp[i].objPrefab, Vector3.zero, Quaternion.identity);
                tmpSpawnObj.SetActive(false);
                tmpSpawnList.Add(tmpSpawnObj);
                //Debug.Log("tmpSpawnList's size is " + tmpSpawnList.Count);
            }
            objectPool.Add(objProp[i].objTag, tmpSpawnList);
        }

        for (int i = 0; i < objectPool.Count; i++)
        {
            Debug.Log("Pool tagged "+objProp[i].objTag+" has "+objectPool[objProp[i].objTag].Count+" objects.");
        }*/
    }
}
