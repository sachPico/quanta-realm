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
        /*public EnemyMoveEnum enemyMoveBehaviour;*/
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

    public Transform player;
    public float maxSpeed;
    public EnemyLibrary enemyLibrary;
    public StageSettings stageSettings;

    /*public List<Vector3> NodePos
    {
        get
        {
            return stageSettings.nodePos;
        }
    }*/

    public AnimationClip StageAnimation
    {
        get
        {
            return stageSettings.stageAnimation;
        }
    }

    public List<StageEnemyProperties> StageEnemyProperties
    {
        get
        {
            return stageSettings.stageEnemyProperties;
        }
    }

    int spawnCount = 0;

    void Start()
    {
        GetComponent<Animation>().clip = StageAnimation;
        GetComponent<Animation>().Play("stage1_b");
    }

    public void Spawn()
    {
        var s = StageEnemyProperties[spawnCount];
        Vector3 spawnPosition = new Vector3();

        foreach(var t in s.spawns)
        {
            spawnPosition = t.spawnLocation;

            switch(t.spawnType)
            {
                case SpawnType.Wave:
                    EnemySpawner es = PoolHandler.instance.RequestObject("EnemySpawner", true).GetComponent<EnemySpawner>();
                    es.transform.localPosition = spawnPosition;
                    es.gameObject.SetActive(true);
                    StartCoroutine(es.Spawn(t.name, t.spawnNumber, t.spawnInterval, spawnPosition, t.enemyProperty));
                    break;
                case SpawnType.Single:
                    break;
            }
        }
        
        spawnCount++;
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

    private void FixedUpdate()
    {
        transform.position += transform.right * maxSpeed * Time.fixedDeltaTime;
    }
}
