using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO: Diganti class biar bisa dapet salinan
public interface IEnemyProfile
{
    public object GetProfile();

    public string GetOfficialName();

    public Vector3 GetRelativePosition(PlayfieldObject _playfieldObject);

    public void GetVariantProperty();

    public void Initialize(object enemyProfile);

    public void OnDrawCustomInspector();
}

public class EnemyBase : PlayfieldObject
{
    public int health;
    public float speed;
    public float timer = 0;

    [Header("Qbe Spawn")]
    public int smallQbeFragSpawnNumber;
    public int mediumQbeFragSpawnNumber;
    public int largeQbeFragSpawnNumber;
    public int qbeSpawnNumber;

    public AnimationClip customMovement;

    public List<Turret> turrets;

    public virtual void Begin()
    {
        
    }

    public void Reset()
    {
        timer = 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            health -= WeaponUtility.instance.activeMainWeaponAttack;
            if (health <= 0)
            {
                AudioSourcesHandler.PlaySFX((int)AudioType.ENEMY_SFX, 1);
                OnDestroyed();
            }
            else
            {
                AudioSourcesHandler.PlaySFX((int)AudioType.ENEMY_SFX, 0);
            }
            other.gameObject.SetActive(false);
        }
    }

    public void OnDestroyed()
    {
        if (smallQbeFragSpawnNumber != 0) SpawnQbe(smallQbeFragSpawnNumber, "SmallQbe");
        if (mediumQbeFragSpawnNumber != 0) SpawnQbe(mediumQbeFragSpawnNumber, "MediumQbe");
        if (largeQbeFragSpawnNumber != 0) SpawnQbe(largeQbeFragSpawnNumber, "LargeQbe");
        if (qbeSpawnNumber != 0) SpawnQbe(qbeSpawnNumber, "Qbe");

        RelativePos = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void SpawnQbe(int spawnNumber, string keyword)
    {
        PlayfieldObject qbe;
        for (int i = 0; i < spawnNumber; i++)
        {
            qbe = PoolHandler.instance.RequestObject(keyword, true).GetComponent<PlayfieldObject>();
            qbe.RelativePos = RelativePos + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            qbe.gameObject.SetActive(true);
        }
    }
}

// [CustomEditor(typeof(EnemyBase)), CanEditMultipleObjects]
// public class EnemyBaseEditor : Editor
// {
//     EnemyBase dst;

//     private void OnEnable()
//     {
//         dst = (EnemyBase)target;
//     }

//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();

//         if (dst.enemyProperty != null)
//         {
//             EditorGUILayout.Space(16f);
//             EditorGUILayout.LabelField($"[{dst.enemyProperty.GetType()}]", new GUIStyle() { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, stretchWidth = true });

//             dst.enemyProperty.OnDrawCustomInspector();
//         }
//         else
//         {
//             EditorGUILayout.LabelField("No EnemyProperty has been assigned", new GUIStyle() { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, stretchWidth = true});
//         }
//     }
// }