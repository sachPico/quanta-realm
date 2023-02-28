using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public float steeringAngleThreshold;

    private void Start()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
}
