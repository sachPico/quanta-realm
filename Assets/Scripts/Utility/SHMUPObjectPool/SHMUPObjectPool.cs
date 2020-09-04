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

    public Transform playfieldTransform;
    public Dictionary<string, List<GameObject>> objectPool = new Dictionary<string, List<GameObject>>();

    [SerializeField]
    public List<ObjectProperty> objProp;

    public void Start()
    {
        List<GameObject> tmpSpawnList = new List<GameObject>();
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
        }
    }
}
