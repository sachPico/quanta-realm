using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialBehaviour))]
public class MaterialBehaviourEditor : Editor
{
    // Start is called before the first frame update
    public MaterialBehaviour _scriptTarget;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Print Size of RayCasts"))
        {
            Debug.Log(_scriptTarget.hitInfos.Length);
        }
        if(GUILayout.Button("Print all raycasts property"))
        {
        }
    }
    public void OnEnable()
    {
        _scriptTarget = (MaterialBehaviour)target;
    }
}
