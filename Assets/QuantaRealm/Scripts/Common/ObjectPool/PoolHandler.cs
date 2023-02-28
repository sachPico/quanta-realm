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

        foreach(var o in poolProperties)
        {
            if(o.poolTransform != null && o.defaultSpawnedObject != null)
            {
                Instantiate(o.defaultSpawnedObject, o.poolTransform).SetActive(false);
            }
        }
    }

    public GameObject SpawnNewObject(string keyword, bool activateInHierarchy)
    {
        GameObject output = Instantiate(defaultSpawnedObjects[keyword], Vector3.zero, defaultSpawnedObjects[keyword].transform.rotation, pools[keyword]);
        output.SetActive(activateInHierarchy);
        return output;
    }

    public GameObject RequestObject(string poolName, bool activateInHierarchy)
    {
        GameObject output;
        
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
            return SpawnNewObject(poolName, activateInHierarchy);
        }
        else
        {
            return SpawnNewObject(poolName, activateInHierarchy);
        }
    }

    //Returns associated component from the requested object in pool
    public T RequestObject<T>(string poolName, bool activateInHierarchy)
    {
        T obj = RequestObject(poolName, activateInHierarchy).GetComponent<T>();
        return obj;
    }

    public GameObject RequestObject(string poolName, bool activateInHierarchy, Vector3 spawnPos)
    {
        PlayfieldObject o = RequestObject(poolName, activateInHierarchy).GetComponent<PlayfieldObject>();
        o.RelativePos = spawnPos;
        return o.gameObject;
    }
}
