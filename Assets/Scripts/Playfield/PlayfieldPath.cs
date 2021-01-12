using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfieldPath : MonoBehaviour
{
    public enum EnemyMoveEnum{Sinusoidal, Straight};
    [System.Serializable]
    public struct EnemySpawnProperty
    {
        public string enemyKeyword;
        public int spawnNumber;
        public float spawnTime;
        public float spawnInterval;
        //Replace with a struct that can handle multiple enemy spawns in one call
        public GameObject enemy;
        //Should be removed
        public Vector3 spawnPosition;
        public EnemyMoveEnum enemyMoveBehaviour;
    }

    public string playerGameObjectName;

    public Playfield _playfield;
    public DeltaHoriController _dhc;

    public AnimationClip stageAnimation;
    public List<EnemySpawnProperty> enemySpawnProperties;

    int spawnCount=0;

    void Start()
    {
        _playfield = this.gameObject.GetComponent<Playfield>();
        _dhc = GameObject.Find(playerGameObjectName).GetComponent<DeltaHoriController>();

        if (_playfield == null)
        {
            Debug.Log("PF NOT FOUND");
            Debug.Break();
        }
        if (_dhc == null)
        {
            Debug.Log("DHC NOT FOUND");
            Debug.Break();
        }
        GetComponent<Animation>().Play("stage1_a");
    }

    public void Spawn()
    {
        //Spawns EnemySpawner
        EnemySpawner es = PoolHandler.instance.RequestObject("EnemySpawner").GetComponent<EnemySpawner>();
        es.transform.localPosition = enemySpawnProperties[spawnCount].spawnPosition;
        switch(enemySpawnProperties[spawnCount].enemyMoveBehaviour)
        {
            case EnemyMoveEnum.Sinusoidal:
                es.spawnedEnemyMoveBehaviour = new SinusoidalMove();
                break;
        }
        es.gameObject.SetActive(true);
        StartCoroutine(es.Spawn(enemySpawnProperties[spawnCount].enemyKeyword, enemySpawnProperties[spawnCount].spawnNumber, enemySpawnProperties[spawnCount].spawnInterval));
        spawnCount++;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach(var ep in enemySpawnProperties)
        {
            Gizmos.DrawSphere(new Vector3
            (
                UnityEditor.AnimationUtility.GetEditorCurve(stageAnimation, UnityEditor.AnimationUtility.GetCurveBindings(stageAnimation)[0]).Evaluate(ep.spawnTime),
                UnityEditor.AnimationUtility.GetEditorCurve(stageAnimation, UnityEditor.AnimationUtility.GetCurveBindings(stageAnimation)[1]).Evaluate(ep.spawnTime),
                UnityEditor.AnimationUtility.GetEditorCurve(stageAnimation, UnityEditor.AnimationUtility.GetCurveBindings(stageAnimation)[2]).Evaluate(ep.spawnTime)
            ), 5f);
        }
    }
}
