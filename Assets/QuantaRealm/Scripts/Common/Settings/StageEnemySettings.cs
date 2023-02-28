using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum SpawnPosition {UpperRight, MiddleRight, LowerRight};
public enum SpawnMode { Singular, RandomPeriodic };

[System.Serializable]
public class EnemySpawn
{
    public bool show;
    public int spawnNumber;
    public float spawnInterval;
    public SpawnMode spawnMode;
    //public EnemyType enemyType;
    public Vector3 minSpawnPosition;
    public Vector3 maxSpawnPosition;

    public int enemyID;

    public EnemySpawn()
    {
        spawnNumber = 0;
        spawnInterval = 0;
        spawnMode = SpawnMode.Singular;
        minSpawnPosition = Vector3.zero;
        maxSpawnPosition = Vector3.zero;
        enemyID = 0;
    }
}

[System.Serializable]
public class EnemyWave
{
    public bool show;
    public float spawnTime;

    [SerializeField]
    public List<EnemySpawn> enemySpawns;

    public EnemyWave()
    {
        spawnTime = 0f;
        enemySpawns = new List<EnemySpawn>();
    }
}

[CreateAssetMenu(fileName = "stage_", menuName = "Amnisium/Stage/StageSetting")]
public class StageEnemySettings : ScriptableObject
{
    public bool showOriginalInspector;
    /// <summary>
    /// Show title sequence at specified second.
    /// </summary>
    public float titleShow;
    public string stageName;
    public string stageDescription;
    public string stageLocation;

    public AnimationClip animationClip;
    public GeneralStageSettings generalStageSettings;

    [SerializeField]
    public List<EnemyWave> enemyWaves;

    public void Apply()
    {
        AnimationEvent[] events = new AnimationEvent[enemyWaves.Count + 2];

        for (int i = 0; i < enemyWaves.Count; i++)
        {
            events[i] = new AnimationEvent();
            events[i].functionName = "Spawn";
            events[i].intParameter = i;
            events[i].time = enemyWaves[i].spawnTime;
        }

        events[enemyWaves.Count] = new AnimationEvent();
        events[enemyWaves.Count].functionName = "FadeOut";
        events[enemyWaves.Count].time = 0f;

        events[enemyWaves.Count+1] = new AnimationEvent();
        events[enemyWaves.Count+1].functionName = "ShowTitle";
        events[enemyWaves.Count+1].time = titleShow;

        AnimationUtility.SetAnimationEvents(animationClip, events);
    }
}

[CustomEditor(typeof(StageEnemySettings)), CanEditMultipleObjects]
public class StageEnemySettingsEditor : Editor
{
    StageEnemySettings dst;
    SerializedProperty enemyToSpawnProp;
    SerializedProperty enemyWavesProp;
    SerializedProperty currentEnemySpawnProp;
    SerializedProperty animationClipProp;

    GUIStyle waveLabelStyle;
    GUIStyle spawnLabelStyle;

    string[] list;

    private Vector3 GetSpawnPosition(SpawnPosition _spawnPosition)
    {
        Vector3 result = Vector3.zero;

        switch(_spawnPosition)
        {
            case SpawnPosition.UpperRight:
                result = new Vector3(dst.generalStageSettings.maxStageBorder.x, dst.generalStageSettings.maxStageBorder.y, 0f);
                break;
            case SpawnPosition.MiddleRight:
                result = new Vector3(dst.generalStageSettings.maxStageBorder.x, 0f, 0f);
                break;
            case SpawnPosition.LowerRight:
                result = new Vector3(dst.generalStageSettings.maxStageBorder.x, dst.generalStageSettings.minStageBorder.y, 0f);
                break;
        }

        return result;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnEnable()
    {
        dst = (StageEnemySettings) target;

        EditorUtility.SetDirty(dst);

        SceneView.duringSceneGui += OnSceneGUI;

        enemyWavesProp = serializedObject.FindProperty("enemyWaves");
        enemyToSpawnProp = serializedObject.FindProperty("enemyToSpawn");
        animationClipProp = serializedObject.FindProperty("animationClip");

        currentEnemySpawnProp = enemyWavesProp.GetArrayElementAtIndex(0);
    }            

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        EditorGUILayout.PropertyField(animationClipProp);
        //DrawDefaultInspector();

        dst.showOriginalInspector = EditorGUILayout.Toggle("Show original Inspector", dst.showOriginalInspector);

        if (dst.showOriginalInspector)
        {
            base.DrawDefaultInspector();
        }
        else
        {
            waveLabelStyle = EditorStyles.foldout;
            waveLabelStyle.fontStyle = FontStyle.Bold;
            spawnLabelStyle = EditorStyles.foldout;
            spawnLabelStyle.fontStyle = FontStyle.Bold;

            for (int i = 0; i < dst.enemyWaves.Count; i++)
            {
                dst.enemyWaves[i].show = EditorGUILayout.Foldout(dst.enemyWaves[i].show, $"{(i + 1).GetOrdinal()} wave", waveLabelStyle);
                if (dst.enemyWaves[i].show)
                {
                    if (GUILayout.Button("Delete Enemy Wave"))
                    {
                        dst.enemyWaves.Remove(dst.enemyWaves[i]);
                    }

                    //dst.enemyWaves[i].spawnPosition = EditorGUILayout.Vector3Field("Spawn position", dst.enemyWaves[i].spawnPosition);//GetSpawnPosition(dst.enemyWaves[i].spawnPositionType));
                    dst.enemyWaves[i].spawnTime = EditorGUILayout.FloatField("Spawn time", dst.enemyWaves[i].spawnTime);

                    if (GUILayout.Button("Add Enemy Spawn"))
                    {
                        dst.enemyWaves[i].enemySpawns.Add(new EnemySpawn());
                    }

                    for (int j = 0; j < dst.enemyWaves[i].enemySpawns.Count; j++)
                    {
                        dst.enemyWaves[i].enemySpawns[j].show = EditorGUILayout.Foldout(dst.enemyWaves[i].enemySpawns[j].show, $"{(j + 1).GetOrdinal()} spawn", spawnLabelStyle);
                        if (dst.enemyWaves[i].enemySpawns[j].show)
                        {
                            if (GUILayout.Button("Delete Enemy Spawn"))
                            {
                                dst.enemyWaves[i].enemySpawns.Remove(dst.enemyWaves[i].enemySpawns[j]);
                            }

                            var o = dst.enemyWaves[i].enemySpawns[j];
                            //var p = o.enemyProperty;

                            EditorGUILayout.BeginVertical();

                            o.spawnInterval = EditorGUILayout.FloatField("Spawn interval", o.spawnInterval);
                            o.spawnNumber = EditorGUILayout.IntField("Spawn number", o.spawnNumber);
                            o.minSpawnPosition = EditorGUILayout.Vector3Field("Min spawn position", o.minSpawnPosition);
                            o.maxSpawnPosition = EditorGUILayout.Vector3Field("Max spawn position", o.maxSpawnPosition);
                            o.enemyID = EditorGUILayout.Popup("Test Enemy ID", o.enemyID, EnemyLibrary.Instance.EnemyList);

                            EditorGUILayout.EndVertical();

                            //if (p == null || EnemyLibrary.GetEnemyProperty(o.enemyType).GetType() != p.GetType())
                            //{
                            //    o.enemyProperty = EnemyLibrary.GetEnemyProperty(o.enemyType);
                            //}

                            //o.enemyProperty.OnDrawCustomInspector();
                            EditorGUILayout.Space(8f);
                        }
                    }
                }

                //EditorGUILayout.Space(16f);
            }
        }

        if (GUILayout.Button("Add Enemy Wave"))
        {
            dst.enemyWaves.Add(new EnemyWave());
        }

        if (GUILayout.Button("Apply Stage Setting to AnimationClip"))
        {
            dst.Apply();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI(SceneView _sceneView)
    {
        
    }
}