using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    [System.Serializable]
    public struct PoolProperties
    {
        public string name;
        public GameObject defaultSpawnedObject;
        public Transform poolTransform;
    }

    public static PoolHandler instance;
    public List<PoolProperties> poolProperties;
    public Dictionary<string, GameObject> defaultSpawnedObjects;
    public Dictionary<string, Transform> pools;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        defaultSpawnedObjects = new Dictionary<string, GameObject>();
        pools = new Dictionary<string, Transform>();
        foreach(var property in poolProperties)
        {
            pools.Add(property.name, property.poolTransform);
            defaultSpawnedObjects.Add(property.name, property.defaultSpawnedObject);
        }

        foreach (var o in poolProperties)
        {
            if (o.defaultSpawnedObject != null && o.poolTransform != null)
            {
                Instantiate(o.defaultSpawnedObject, o.poolTransform).SetActive(false);
            }
        }
    }

    GameObject SpawnNewObject(string keyword)
    {
        GameObject output = Instantiate(defaultSpawnedObjects[keyword], Vector3.zero, Quaternion.identity, pools[keyword]);
        output.SetActive(false);
        return output;
    }

    public GameObject RequestObject(string poolName, bool activateInHierarchy)
    {
        GameObject output;
        // Debug.Log(pools[poolName].hierarchyCount);
        //Check in the pool
        if(pools[poolName].childCount > 0)
        {
            for(int i=0; i<pools[poolName].childCount; i++)
            {
                if(!pools[poolName].GetChild(i).gameObject.activeInHierarchy)
                {
                    output = pools[poolName].GetChild(i).gameObject;
                    output.SetActive(activateInHierarchy);
                    return output;
                }
            }
            return SpawnNewObject(poolName);
        }
        else
        {
            return SpawnNewObject(poolName);
        }
    }

    public GameObject RequestObject(string poolName, bool activateInHierarchy, Vector3 playfieldPosition)
    {
        PlayfieldObject a = RequestObject(poolName, activateInHierarchy).GetComponent<PlayfieldObject>();
        a.RelativePos = playfieldPosition;
        return a.gameObject;
    }
}
