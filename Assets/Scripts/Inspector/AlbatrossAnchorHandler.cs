using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbatrossAnchorHandler : MonoBehaviour
{
    public LineRenderer lr;
    public bool isCollide = false;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    
    /*void OnTriggerStay(Collider other)
    {
        isCollide = true;
        lr.enabled = false; 
        //Debug.Log(gameObject.name+" is hit!");
    }

    void OnTriggerExit(Collider other)
    {
        isCollide = false;
        lr.enabled = true;
    }*/
}
