using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { PK001, PK002 };

[System.Serializable]
public class Enemy
{
    public string enemyName;
    public EnemyType enemyType;
    public GameObject enemyPrefab;
    public Texture2D enemyGUIIcon;
    public string enemyGUIIconPath;
}

[CreateAssetMenu(fileName = "EnemyLibrary", menuName = "Library/Enemy")]
public class EnemyLibrary : ScriptableObject
{
    public List<Enemy> enemyLibrary = new List<Enemy>();
}
