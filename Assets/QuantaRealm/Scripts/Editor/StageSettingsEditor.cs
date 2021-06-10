// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(StageSettings))]
// public class StageSettingsEditor : Editor
// {
//     StageSettings settingTarget;

//     SerializedProperty stageNameField, stageEnemyField;

//     FieldInfo[] targetFieldInfo;

//     private void OnEnable()
//     {
//         settingTarget = (StageSettings)target;
//         Debug.Log(settingTarget.name);

//         stageNameField = serializedObject.FindProperty("stageName");
//         stageEnemyField = serializedObject.FindProperty("stageEnemyProperties");

//         targetFieldInfo = target.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//     }

//     public override void OnInspectorGUI()
//     {
//         EditorGUILayout.PropertyField(stageNameField, new GUIContent("Current Stage Name"));
//         EditorGUILayout.PropertyField(stageEnemyField, new GUIContent("Stage Enemies"));

//         // foreach(FieldInfo field in targetFieldInfo)
//         // {
//         //     //if(field.IsNotSerialized || field.IsStatic)
//         //     //{
//         //     //    continue;
//         //     //}
    
//         //     if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
//         //     {
//         //         EditorGUILayout.PropertyField(serializedObject.FindProperty(field.Name));
//         //     }
//         // }
//     }
// }