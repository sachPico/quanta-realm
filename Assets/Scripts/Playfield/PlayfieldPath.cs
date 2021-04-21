using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfieldPath : MonoBehaviour
{
    public enum EnemyMoveEnum{Sinusoidal, Straight};
    [System.Serializable]
    public struct SpawnerProperty
    {
        public EnemyMoveEnum enemyMoveBehaviour;
        public Vector3 spawnerPosition;
        public int spawnNumber;
        public float spawnInterval;
    }

    [System.Serializable]
    public struct SpawnerAtTimeProperty
    {
        public string spawnerKeyword;
        public float spawnTime;
        public SpawnerProperty[] enemySpawnerProperty;
    }

    public float maxSpeed;

    public AnimationClip stageAnimation;
    public List<Vector3> nodePos;
    public List<SpawnerAtTimeProperty> spawnerAtTimeProperties;

    int spawnCount=0;

    void Start()
    {
        GetComponent<Animation>().Play("stage1_a");
    }

    public void Spawn()
    {
        //Spawns EnemySpawner
        for(int i=0; i<spawnerAtTimeProperties[spawnCount].enemySpawnerProperty.Length; i++)
        {
            EnemySpawner es = PoolHandler.instance.RequestObject("EnemySpawner").GetComponent<EnemySpawner>();
            es.transform.localPosition = spawnerAtTimeProperties[spawnCount].enemySpawnerProperty[i].spawnerPosition;
            switch(spawnerAtTimeProperties[spawnCount].enemySpawnerProperty[i].enemyMoveBehaviour)
            {
                case EnemyMoveEnum.Sinusoidal:
                    es.spawnedEnemyMoveBehaviour = new SinusoidalMove();
                    break;
            }
            es.gameObject.SetActive(true);
            StartCoroutine(es.Spawn(spawnerAtTimeProperties[spawnCount].spawnerKeyword, spawnerAtTimeProperties[spawnCount].enemySpawnerProperty[i].spawnNumber, spawnerAtTimeProperties[spawnCount].enemySpawnerProperty[i].spawnInterval));
        }
        spawnCount++;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.white;
    //     foreach(var ep in enemySpawnProperties)
    //     {
    //         Gizmos.DrawSphere(new Vector3
    //         (
    //             UnityEditor.AnimationUtility.GetEditorCurve(stageAnimation, UnityEditor.AnimationUtility.GetCurveBindings(stageAnimation)[0]).Evaluate(ep.spawnTime),
    //             UnityEditor.AnimationUtility.GetEditorCurve(stageAnimation, UnityEditor.AnimationUtility.GetCurveBindings(stageAnimation)[1]).Evaluate(ep.spawnTime),
    //             UnityEditor.AnimationUtility.GetEditorCurve(stageAnimation, UnityEditor.AnimationUtility.GetCurveBindings(stageAnimation)[2]).Evaluate(ep.spawnTime)
    //         ), 5f);
    //     }
    // }
}
