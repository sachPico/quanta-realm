using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayfieldPath : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnerProperty
    {
        public EnemyMoveEnum enemyMoveBehaviour;
        public Vector3 spawnerPosition;
        public int spawnNumber;
        public float spawnInterval;
        public float parameterA, parameterB, parameterC;
    }

    [System.Serializable]
    public struct SpawnerAtTimeProperty
    {
        public EnemyType enemyType;
        public string spawnerKeyword;
        public float spawnTime;
        public SpawnerProperty[] enemySpawnerProperty;
    }

    public float maxSpeed;
    public EnemyLibrary enemyLibrary;
    public StageSettings stageSettings;

    public List<Vector3> thisNodePos;
    //public List<SpawnerAtTimeProperty> spawnerAtTimeProperties;

    public List<Vector3> nodePos
    {
        get
        {
            return stageSettings.nodePos;
        }
    }

    public AnimationClip stageAnimation
    {
        get
        {
            return stageSettings.stageAnimation;
        }
    }

    public StageEnemyProperties[] stageEnemyProperties
    {
        get
        {
            return stageSettings.stageEnemyProperties;
        }
    }

    int spawnCount=0;

    void Start()
    {
        GetComponent<Animation>().clip = stageAnimation;
        GetComponent<Animation>().Play("stage1_a");
    }

    public void Spawn()
    {
        //Spawns EnemySpawner
        foreach(var s in stageEnemyProperties)
        {
            EnemySpawner es = PoolHandler.instance.RequestObject("EnemySpawner").GetComponent<EnemySpawner>();
            es.transform.localPosition = s.spawnPlayfieldPosition;
            switch(s.enemyMoveType)
            {
                case EnemyMoveEnum.Sinusoidal:
                    es.spawnedEnemyMoveBehaviour = new SinusoidalMove();
                    break;
            }
            es.gameObject.SetActive(true);
            //Spawn time should be non-zero
            StartCoroutine(es.Spawn(s.name, s.spawnNumber, s.spawnTime, 0, 0, 0));
        }
        spawnCount++;
    }

    void OnDrawGizmos()
    {
        EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(stageAnimation);
        Vector3 squareGizmosCenter = Vector3.zero;
        foreach(var esp in stageEnemyProperties)
        {
            Gizmos.color = Color.green;
            squareGizmosCenter.x = AnimationUtility.GetEditorCurve(stageAnimation, ecb[0]).Evaluate(esp.spawnTime);
            squareGizmosCenter.y = AnimationUtility.GetEditorCurve(stageAnimation, ecb[1]).Evaluate(esp.spawnTime);
            squareGizmosCenter.z = AnimationUtility.GetEditorCurve(stageAnimation, ecb[2]).Evaluate(esp.spawnTime);

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
        }
    }

    void OnValidate()
    {
        thisNodePos = nodePos;
    }
}
