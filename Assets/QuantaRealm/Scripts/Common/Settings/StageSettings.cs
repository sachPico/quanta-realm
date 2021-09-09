using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SpawnType { Single, Wave};
public enum Test { One, Two};
public enum SpawnPosition {CenterLeft, CenterRight, BottomCenter, UpperCenter};

[Serializable]
public class StageEnemyProperties
{
    [HideInInspector]
    public string name;
    [SerializeField]
    public List<Spawn> spawns;
    public float spawnTime;

    public StageEnemyProperties(string n)
    {
        name = n;
    }
}

[Serializable]
public class AdditionalEnemySetting
{
    public Vector3 additionalVector3Value;
}

[Serializable]
public class Spawn
{
    [HideInInspector]
    public string name;
    public EnemyType enemy;
    public SpawnType spawnType;
    public SpawnPosition spawnPosition;
    public Vector3 spawnLocation;
    [SerializeReference]
    public Property enemyProperty;
    public bool showInInspector;
    public float spawnInterval;
    public int spawnNumber;
}

[Serializable]
public class SpawnPath
{
    public Vector3[] paths;
}

[CreateAssetMenu(fileName = "stage_", menuName = "Amnisium/Stage")]
public class StageSettings : ScriptableObject
{
    public GeneralStageSettings generalStageSetting;
    public string stageName;
    public AnimationClip stageAnimation;
    public List<Vector3> nodePos;

    public List<StageEnemyProperties> stageEnemyProperties;

    private void OnValidate()
    {
        for(int i=0; i<stageEnemyProperties.Count; i++)
        {
            for(int k=0; k < stageEnemyProperties[i].spawns.Count; k++)
            {
                stageEnemyProperties[i].name = i.ToString();
                stageEnemyProperties[i].spawns[k].name = stageEnemyProperties[i].spawns[k].enemy.ToString();
            }
        }
    }
}










[CustomEditor(typeof(StageSettings)), CanEditMultipleObjects]
public class StageSettingsEditor : Editor
{
    StageSettings dst;
    int index = 1;
    int bezierToBeDisplayedCount = 0;
    bool[] showStageEnemyProperties;

    List<BezierPoints> bezierToBeDisplayed = new List<BezierPoints>();

    Vector3[] playfieldBound = new Vector3[4];

    private void OnSceneGUI(SceneView sceneView)
    {
        if (bezierToBeDisplayed != null)
        {
            if (bezierToBeDisplayed.Count > 0)
            {
                for (int i = 0; i < bezierToBeDisplayed.Count; i++)
                {
                    SceneViewUtility.DrawBezier(bezierToBeDisplayed[i].point, false);
                }
            }
        }

        Handles.DrawSolidRectangleWithOutline(playfieldBound, new Color(1, 1, 1, .1f), Color.red);
    }

    public override void OnInspectorGUI()
    {
        /*DrawDefaultInspector();*/

        dst.generalStageSetting = (GeneralStageSettings)EditorGUILayout.ObjectField("General stage setting", dst.generalStageSetting, typeof(UnityEngine.Object), true);
        dst.stageAnimation = (AnimationClip)EditorGUILayout.ObjectField("Stage Animation", dst.stageAnimation, typeof(UnityEngine.Object), true);
        SerializedObject so = new SerializedObject(dst);

        EditorGUILayout.Space(8f);

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Prev"))
        {
            if (index > 1)
            {
                index--;
            }
        }

        index = EditorGUILayout.IntField(index);
        EditorGUILayout.IntField(dst.stageEnemyProperties.Count);

        if (GUILayout.Button("Next"))
        {
            if (index < dst.stageEnemyProperties.Count)
            {
                index++;
            }
        }
        EditorGUILayout.EndHorizontal();

        StageEnemyProperties sep = dst.stageEnemyProperties[index-1];

        sep.spawnTime = EditorGUILayout.FloatField("Spawn time", sep.spawnTime);

        List<BezierPoints> tmpBezier = new List<BezierPoints>();

        for (int j = 0; j < sep.spawns.Count; j++)
        {
            sep.spawns[j].enemy = (EnemyType)EditorGUILayout.EnumPopup("Enemy Type", sep.spawns[j].enemy);

            switch ((SpawnType)EditorGUILayout.EnumPopup("Spawn Type", sep.spawns[j].spawnType))
            {
                case SpawnType.Single:
                    sep.spawns[j].spawnType = SpawnType.Single;
                    sep.spawns[j].spawnNumber = 1;
                    break;
                case SpawnType.Wave:
                    sep.spawns[j].spawnType = SpawnType.Wave;
                    sep.spawns[j].spawnNumber = EditorGUILayout.IntField("Spawn Number", Mathf.Max(2, sep.spawns[j].spawnNumber));
                    break;

            }

            sep.spawns[j].spawnInterval = EditorGUILayout.FloatField("Spawn Interval", sep.spawns[j].spawnInterval);

            switch ((SpawnPosition)EditorGUILayout.EnumPopup("Spawn Position", sep.spawns[j].spawnPosition))
            {
                case SpawnPosition.BottomCenter:
                    sep.spawns[j].spawnPosition = SpawnPosition.BottomCenter;
                    /*sep.spawns[j].spawnLocation.x = dst.generalStageSetting.minStageBorder.x + (dst.generalStageSetting.maxStageBorder.x - dst.generalStageSetting.minStageBorder.x)/2f;*/
                    sep.spawns[j].spawnLocation.y = dst.generalStageSetting.minStageBorder.y;
                    break;
                case SpawnPosition.UpperCenter:
                    sep.spawns[j].spawnPosition = SpawnPosition.UpperCenter;
                    /*sep.spawns[j].spawnLocation.x = dst.generalStageSetting.minStageBorder.x + (dst.generalStageSetting.maxStageBorder.x - dst.generalStageSetting.minStageBorder.x) / 2f;*/
                    sep.spawns[j].spawnLocation.y = dst.generalStageSetting.maxStageBorder.y;
                    break;
                case SpawnPosition.CenterLeft:
                    sep.spawns[j].spawnPosition = SpawnPosition.CenterLeft;
                    sep.spawns[j].spawnLocation.x = dst.generalStageSetting.minStageBorder.x;
                    /*sep.spawns[j].spawnLocation.y = dst.generalStageSetting.minStageBorder.y + (dst.generalStageSetting.maxStageBorder.y - dst.generalStageSetting.minStageBorder.y) / 2f;*/
                    break;
                case SpawnPosition.CenterRight:
                    sep.spawns[j].spawnPosition = SpawnPosition.CenterRight;
                    sep.spawns[j].spawnLocation.x = dst.generalStageSetting.maxStageBorder.x;
                    /*sep.spawns[j].spawnLocation.y = dst.generalStageSetting.minStageBorder.y + (dst.generalStageSetting.maxStageBorder.y - dst.generalStageSetting.minStageBorder.y) / 2f;*/
                    break;
            }

            sep.spawns[j].spawnLocation = EditorGUILayout.Vector3Field("Spawn Location", sep.spawns[j].spawnLocation);

            switch (sep.spawns[j].enemy)
            {
                case EnemyType.PK001:
                    sep.spawns[j].enemy = EnemyType.PK001;
                    if (sep.spawns[j].enemyProperty == null)
                    {
                        sep.spawns[j].enemyProperty = new Property();
                    }
                    else
                    {
                        if (sep.spawns[j].enemyProperty.GetType() != typeof(Property))
                        {
                            sep.spawns[j].enemyProperty = new Property();
                        }
                    }

                    break;
                case EnemyType.PK002:
                    sep.spawns[j].enemy = EnemyType.PK002;
                    if (sep.spawns[j].enemyProperty == null)
                    {
                        sep.spawns[j].enemyProperty = new Property();
                    }
                    else
                    {
                        if (sep.spawns[j].enemyProperty.GetType() != typeof(Property))
                        {
                            sep.spawns[j].enemyProperty = new Property();
                        }
                    }

                    break;
                case EnemyType.PK003:
                    sep.spawns[j].enemy = EnemyType.PK003;
                    if (sep.spawns[j].enemyProperty == null)
                    {
                        sep.spawns[j].enemyProperty = new OocystProperties();
                    }
                    else
                    {
                        if(sep.spawns[j].enemyProperty.GetType() != typeof(OocystProperties))
                        {
                            sep.spawns[j].enemyProperty = new OocystProperties();
                        }
                    }

                    OocystProperties op = ((OocystProperties)sep.spawns[j].enemyProperty);

                    op.moveDirection = EditorGUILayout.Vector3Field("Next direction", op.moveDirection);
                    op.timeToAlterDirection = EditorGUILayout.FloatField("Time to alter direction", op.timeToAlterDirection);

                    EditorGUILayout.Space(8f);

                    //Start handling enemy's Bezier curve

                    if (op.bezier.point != null)
                    {
                        if (op.bezier.point.Count <= 3)
                        {
                            op.bezier.point.Add(Vector3.zero);
                            op.bezier.point.Add(Vector3.one * 1);
                            op.bezier.point.Add(Vector3.one * 2);
                            op.bezier.point.Add(Vector3.one * 3);
                        }
                        else
                        {
                            for (int i = 0; i < op.bezier.point.Count; i++)
                            {
                                if(i==0)
                                {
                                    op.bezier.point[i] = sep.spawns[j].spawnLocation;
                                }
                                else
                                {
                                    op.bezier.point[i] = EditorGUILayout.Vector3Field("Point " + i, op.bezier.point[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        op.bezier.point = new List<Vector3>();
                        InitializeBezier(op.bezier.point);
                    }

                    if(GUILayout.Button("Add new anchor point"))
                    {
                        op.bezier.point.Add(op.bezier.point[op.bezier.point.Count - 4]);
                        op.bezier.point.Add(op.bezier.point[op.bezier.point.Count - 3]);
                        op.bezier.point.Add(op.bezier.point[op.bezier.point.Count - 2]);
                    }

                    if(GUILayout.Button("Remove last Bezier segment"))
                    {
                        for(int i=0; i<3; i++)
                        {
                            op.bezier.point.RemoveAt(op.bezier.point.Count - 1);
                        }
                    }

                    if(GUILayout.Button("Reset Bezier"))
                    {
                        List<Vector3> points = new List<Vector3>();
                        InitializeBezier(points);
                        op.bezier.point = points;
                    }

                    tmpBezier.Add(op.bezier);

                    break;
            }

            EditorGUILayout.Space(16f);
        }

        bezierToBeDisplayed = tmpBezier;

        if (GUILayout.Button("Add Enemy"))
        {
            sep.spawns.Add(new Spawn());
        }

        if (GUILayout.Button("Remove Last Enemy"))
        {
            sep.spawns.RemoveAt(sep.spawns.Count - 1);
        }

        EditorGUILayout.Space(4f);

        if (GUILayout.Button("Add Spawn"))
        {
            int newLastIndex = dst.stageEnemyProperties.Count;
            string newName = newLastIndex.ToString();
            dst.stageEnemyProperties.Add(new StageEnemyProperties(newName));

            dst.stageEnemyProperties[newLastIndex].spawns = new List<Spawn>();
            dst.stageEnemyProperties[newLastIndex].spawns.Add(new Spawn());

            UpdateFoldoutToggle();
        }

        if (GUILayout.Button("Remove Spawn"))
        {
            dst.stageEnemyProperties.RemoveAt(dst.stageEnemyProperties.Count - 1);

            UpdateFoldoutToggle();
        }

        EditorGUILayout.Space(32f);
        if (GUILayout.Button("Apply stage settings to animation clip"))
        {
            AnimationEvent[] events = new AnimationEvent[dst.stageEnemyProperties.Count];

            for(int i=0; i<events.Length; i++)
            {
                events[i] = new AnimationEvent();
                events[i].functionName = "Spawn";
                events[i].time = dst.stageEnemyProperties[i].spawnTime;
            }

            AnimationUtility.SetAnimationEvents(dst.stageAnimation, events);

            so.ApplyModifiedProperties();
        }
    }

    void InitializeBezier(List<Vector3> newBezier)
    {
        newBezier.Add(Vector3.zero);
        newBezier.Add(Vector3.one * 1);
        newBezier.Add(Vector3.one * 2);
        newBezier.Add(Vector3.one * 3);
    }

    private void OnEnable()
    {
        dst = (StageSettings)target;

        UpdateFoldoutToggle();

        EditorUtility.SetDirty(dst);

        playfieldBound[0] = new Vector3(dst.generalStageSetting.minStageBorder.x, dst.generalStageSetting.minStageBorder.y);
        playfieldBound[1] = new Vector3(dst.generalStageSetting.minStageBorder.x, dst.generalStageSetting.maxStageBorder.y);
        playfieldBound[2] = new Vector3(dst.generalStageSetting.maxStageBorder.x, dst.generalStageSetting.maxStageBorder.y);
        playfieldBound[3] = new Vector3(dst.generalStageSetting.maxStageBorder.x, dst.generalStageSetting.minStageBorder.y);

        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void UpdateFoldoutToggle()
    {
        showStageEnemyProperties = new bool[dst.stageEnemyProperties.Count];
        if (showStageEnemyProperties.Length > 0)
        {
            for (int i = 0; i < showStageEnemyProperties.Length; i++)
            {
                showStageEnemyProperties[i] = true;
            }
        }
    }
}