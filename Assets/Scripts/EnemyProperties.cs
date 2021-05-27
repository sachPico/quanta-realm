using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EnemyProperties<T>
{
    private T _data;

    public T data
    {
        get { return _data; }
        set
        {
            _data = value;
            Debug.Log("Type: " + _data.GetType().ToString());
            switch(_data.GetType().ToString())
            {
                case ("PK001"):
                    PK001 a = _data as PK001;
                    Debug.Log(a.hoopla);
                    break;
                case ("PK002"):
                    PK002 b = _data as PK002;
                    Debug.Log(b.hubla);
                    break;
            }
        }
    }

    public float s;
}

[Serializable]
public class Ene
{

}

[Serializable]
public class PK001 : Ene
{
    public float hoopla;

    public PK001()
    {
        hoopla = 4;
    }
}

[Serializable]
public class PK002 : Ene
{
    public float hubla;

    public PK002()
    {
        hubla = 5;
    }
}