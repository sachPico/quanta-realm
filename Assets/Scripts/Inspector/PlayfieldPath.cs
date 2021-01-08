using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfieldPath : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnProperty
    {
        public float spawnTime;
        //Replace with a struct that can handle multiple enemy spawns in one call
        public GameObject enemy;
        public Vector3 spawnPosition;
    }
    public string playerGameObjectName;

    public float maxSpeed;

    public Playfield _playfield;
    public DeltaHoriController _dhc;

    public AnimationClip stageAnimation;
    public List<Vector3> nodePos;
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
        //Replace with object pooling
        GameObject s = Instantiate(enemySpawnProperties[spawnCount].enemy, Vector3.zero, Quaternion.identity, _playfield.playerPivot);
        s.transform.localPosition = enemySpawnProperties[spawnCount].spawnPosition;
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
