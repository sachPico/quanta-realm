using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    public float timeScale; 
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        Debug.Log(Time.time+". Unscaled time is "+Time.unscaledTime);
        yield return new WaitForSeconds(3f);
        Debug.Log(Time.time+" with time scale is "+timeScale+". Unscaled time is "+Time.unscaledTime);
        Debug.Break();
    }
}
