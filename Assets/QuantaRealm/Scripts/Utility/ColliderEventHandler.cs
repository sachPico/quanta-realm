using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerEvent : UnityEvent<Collider>
{

}

public class ColliderEventHandler : MonoBehaviour
{
    [SerializeField] public TriggerEvent onTriggerEnter = new TriggerEvent();
    [SerializeField] public TriggerEvent onTriggerExit = new TriggerEvent();
    [SerializeField] public TriggerEvent onTriggerStay = new TriggerEvent();

    public void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }

    public void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }
}
