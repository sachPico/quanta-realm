using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageSettings))]
public class StageSettingsEditor : Editor
{
    StageSettings settingTarget;

    SerializedProperty stageNameField, stageEnemyField;

    private void OnEnable()
    {
        settingTarget = (StageSettings)target;
        Debug.Log(settingTarget.name);

        stageNameField = serializedObject.FindProperty("stageName");
        stageEnemyField = serializedObject.FindProperty("stageEnemyProperties");

    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        EditorGUILayout.PropertyField(stageNameField, new GUIContent("Current Stage Name"));
        EditorGUILayout.PropertyField(stageEnemyField, new GUIContent("Stage Enemies"));

        /*if(GUILayout.Button("A"))
        {
            Debug.Log("OK");
        }*/
    }
}