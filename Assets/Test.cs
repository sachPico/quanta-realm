using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    PlayfieldPath _playfieldPath;

    void Start()
    {
        _playfieldPath = FindObjectOfType<PlayfieldPath>();
    }
    void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(_playfieldPath.transform.GetChild(0).forward, _playfieldPath.transform.GetChild(0).up);
    }
}
