using Sachet.Utility;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    #region VARIABLES
    #region STATIC
    public static StageManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = FindObjectOfType<StageManager>(true);
            }
            return instance;
        }
    }
    public static PlayfieldObject ActivePlayer
    {
        get
        {
            return Instance.activePlayer;
        }
    }
    private static StageManager instance;
    #endregion

    public PlayfieldObject activePlayer;

    public bool trackPlayfieldPosition = false;

    public Transform player;
    public Transform playfield;
    public float maxSpeed;
    public EnemyLibrary enemyLibrary;

    public AnimationClip stageAnimation;
    public StageEnemySettings stageEnemySettings;

    public float ambientHeatMultiplier;

    [Header("UI")]
    public Animation stageFade;
    public Animation stageUIAnimation;
    public TMP_Text stageTitle;
    public TMP_Text stageDescription;
    public TMP_Text stageLocation;

    [Header("Development")]
    public string trackedPlayfieldPositionsFilename;

    [Header("Debugging")]
    public Vector3 diffs = Vector3.zero;

    int spawnCount = 0;
    List<Vector3> playfieldLocalPos;
    #endregion


    #region METHODS
    void Start()
    {
        if(trackPlayfieldPosition)
        {
            playfieldLocalPos = new List<Vector3>();
        }

        instance = this;

        GetComponent<Animation>().AddClip(stageEnemySettings.animationClip, "active");
        GetComponent<Animation>().Play("active");
    }

    public void Spawn(int _enemyWaveIndex)
    {
        Vector3 spawnPosition = new Vector3();

        foreach (var s in stageEnemySettings.enemyWaves[_enemyWaveIndex].enemySpawns)
        {
            EnemySpawner es = PoolHandler.instance.RequestObject("EnemySpawner", true).GetComponent<EnemySpawner>();
            es.transform.localPosition = spawnPosition;
            es.gameObject.SetActive(true);
            StartCoroutine(es.Spawn(s));
        }

        spawnCount++;
    }

    public void ShowTitle()
    {
        stageTitle.text = stageEnemySettings.stageName;
        stageDescription.text = stageEnemySettings.stageDescription;
        stageLocation.text = stageEnemySettings.stageLocation;

        stageUIAnimation.Play();
    }

    public void FadeOut()
    {
        stageFade.Play();
    }

    void OnDrawGizmos()
    {
        /*EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(StageAnimation);
        Vector3 squareGizmosCenter = Vector3.zero;
        foreach(var esp in StageEnemyProperties)
        {
            Gizmos.color = Color.green;
            squareGizmosCenter.x = AnimationUtility.GetEditorCurve(StageAnimation, ecb[0]).Evaluate(esp.spawnTime);
            squareGizmosCenter.y = AnimationUtility.GetEditorCurve(StageAnimation, ecb[1]).Evaluate(esp.spawnTime);
            squareGizmosCenter.z = AnimationUtility.GetEditorCurve(StageAnimation, ecb[2]).Evaluate(esp.spawnTime);

            //Bisa dibikin Gizmos.DrawCube aja g sih
            //Draw upper border
            Gizmos.DrawLine(squareGizmosCenter + Vector3.right * -23f + Vector3.up * 13f, squareGizmosCenter + Vector3.right * 21f + Vector3.up * 13f);
            //Draw lower border
            Gizmos.DrawLine(squareGizmosCenter + Vector3.right * -23f + Vector3.up * -13f, squareGizmosCenter + Vector3.right * 21f + Vector3.up * -13f);
            //Draw right border
            Gizmos.DrawLine(squareGizmosCenter + Vector3.right * 21f + Vector3.up * 13f, squareGizmosCenter + Vector3.right * 21f + Vector3.up * -13f);
            //Draw left border
            Gizmos.DrawLine(squareGizmosCenter + Vector3.right * -23f + Vector3.up * 13f, squareGizmosCenter + Vector3.right * -23f + Vector3.up * -13f);

            // foreach(var sp in esp.enemySpawnerProperty)
            // {
            //     Gizmos.color = Color.white;
            //     //Gizmos.DrawSphere(squareGizmosCenter + sp.spawnerPosition, 2f);
            //     Gizmos.DrawIcon(squareGizmosCenter + sp.spawnerPosition, enemyLibrary.enemyLibrary[(int)esp.enemyType].enemyGUIIconPath, false);
            // }
        }*/
    }
    #endregion
}

[CustomEditor(typeof(StageManager))]
public class StageEditor : Editor
{
    StageManager dst;
    string[] data;
    Vector3 currentPosition;
    Vector3 lastPosition;
    Vector3 diff;

    List<Vector3> points = new List<Vector3>();
    List<Vector3> diffs = new List<Vector3>();
    List<float> deg_diffs = new List<float>();
    Keyframe[] x_keys;
    Keyframe[] y_keys;
    Keyframe[] z_keys;
    AnimationCurve curve;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*
        if(GUILayout.Button("Import playfield positions data"))
        {
            data = File.ReadAllLines($"{Application.dataPath}/{dst.trackedPlayfieldPositionsFilename}.txt");

            if (data.Length > 0)
            {
                diffs.Clear();
                points.Clear();
                points.Add(Utility.StringToVector3(data[0]));

                for(int i=0;i<data.Length;i++)
                {
                    currentPosition = Utility.StringToVector3(data[i]);

                    if (i > 0)
                    {
                        if (i > 1)
                        {
                            if ((currentPosition - lastPosition).normalized != diff)
                            {
                                points.Add(currentPosition);
                                diffs.Add((currentPosition-lastPosition).normalized);
                            }
                        }

                        diff = (currentPosition - lastPosition).normalized;
                    }

                    lastPosition = currentPosition;
                }

                Debug.Log($"There are {points.Count} different points!");
                string result = "";
                foreach(var point in points)
                {
                    result += $"{point}\n";
                }
                foreach(var d in diffs)
                {
                    Debug.Log(d);
                }
                File.WriteAllText($"{Application.dataPath}/{dst.trackedPlayfieldPositionsFilename}_result.txt", result);
            }
        }

        if(GUILayout.Button("Apply Baked Playfield Animation"))
        {
            ImportBakedAnimationII();
        }
        */
    }

    private void OnEnable()
    {
        dst = (StageManager)target;
    }

    void ImportBakedAnimationII()
    {
        Vector3 lastDiff = Vector3.zero;
        Vector3 playfieldPos = dst.playfield.localPosition;

        List<float> times = new List<float>();

        data = File.ReadAllLines($"{Application.dataPath}/{dst.trackedPlayfieldPositionsFilename}.txt");

        if (data.Length > 0)
        {
            points.Clear();

            Debug.Log(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                playfieldPos += lastDiff;
                if(lastDiff != Utility.StringToVector3(data[i]))
                {
                    points.Add(playfieldPos);
                    times.Add(i * Time.fixedDeltaTime);
                    lastDiff = Utility.StringToVector3(data[i]);
                }
            }
            points.Add(playfieldPos);
            times.Add(data.Length * Time.fixedDeltaTime);

            x_keys = new Keyframe[points.Count];
            y_keys = new Keyframe[points.Count];
            z_keys = new Keyframe[points.Count];


            for (int i = 0; i < points.Count; i++)
            {
                x_keys[i] = new Keyframe(times[i], points[i].x);
                y_keys[i] = new Keyframe(times[i], points[i].y);
                z_keys[i] = new Keyframe(times[i], points[i].z);
            }

            curve = new AnimationCurve(x_keys);
            dst.stageEnemySettings.animationClip.SetCurve("PlayField", typeof(Transform), "localPosition.x", curve);
            curve = new AnimationCurve(y_keys);
            dst.stageEnemySettings.animationClip.SetCurve("PlayField", typeof(Transform), "localPosition.y", curve);
            curve = new AnimationCurve(z_keys);
            dst.stageEnemySettings.animationClip.SetCurve("PlayField", typeof(Transform), "localPosition.z", curve);
        }
    }
}
