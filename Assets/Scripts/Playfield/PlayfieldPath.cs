using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        public float parameterA, parameterB, parameterC;
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
        SpawnerProperty sp;
        SpawnerAtTimeProperty spat = spawnerAtTimeProperties[spawnCount];
        //Spawns EnemySpawner
        for(int i=0; i<spat.enemySpawnerProperty.Length; i++)
        {
            sp = spat.enemySpawnerProperty[i];
            //EnemySpawner es = PoolHandler.instance.RequestObject("EnemySpawner").GetComponent<EnemySpawner>();
            //es.transform.localPosition = sp.spawnerPosition;
            // switch(sp.enemyMoveBehaviour)
            // {
            //     case EnemyMoveEnum.Sinusoidal:
            //         es.spawnedEnemyMoveBehaviour = new SinusoidalMove();
            //         break;
            // }
            //es.gameObject.SetActive(true);
            //StartCoroutine(es.Spawn(spat.spawnerKeyword, sp.spawnNumber, sp.spawnInterval, sp.parameterA, sp.parameterB, sp.parameterC));
        }
        spawnCount++;
    }

    void OnDrawGizmos()
    {
        EditorCurveBinding[] ecb = AnimationUtility.GetCurveBindings(stageAnimation);
        Vector3 squareGizmosCenter = Vector3.zero;
        foreach(var esp in spawnerAtTimeProperties)
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
            foreach(var sp in esp.enemySpawnerProperty)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(squareGizmosCenter + sp.spawnerPosition, 2f);
            }
        }
    }
}
