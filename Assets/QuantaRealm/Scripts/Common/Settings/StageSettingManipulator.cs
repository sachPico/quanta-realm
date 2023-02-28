// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// public class StageSettingManipulator : MonoBehaviour
// {
//     public StageSettings targetStage;

//     public List<Vector3> points;
// }

// [CustomEditor(typeof(StageSettingManipulator)), CanEditMultipleObjects]
// public class StageSettingManipulatorEditor : Editor
// {
//     StageSettingManipulator dst;

//     private void OnSceneGUI()
//     {
//         if (dst != null)
//         {
//             SceneViewUtility.DrawBezier(((OocystProperties)dst.targetStage.stageEnemyProperties[0].spawns[0].enemyProperty).bezier.point, false);
//         }
//         Handles.FreeMoveHandle(Vector3.zero, Quaternion.identity, .5f, Vector3.zero, Handles.SphereHandleCap);
//     }

//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();

//         if(GUILayout.Button("Extract points"))
//         {
//             dst.points = ((OocystProperties)dst.targetStage.stageEnemyProperties[0].spawns[0].enemyProperty).bezier.point;
//         }
//     }

//     private void OnEnable()
//     {
//         dst = (StageSettingManipulator)target;
//     }
// }
