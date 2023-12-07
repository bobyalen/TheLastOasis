using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyPositionsObject", menuName = "ScriptableObjects/Rooms/Enemy Spawn Position")]
public class EnemySpawnPosition : ScriptableObject
{
    public List<EnemySpawn> enemySpawns;
    public bool isExpandable;
}

[Serializable]
public class EnemySpawn
{
    public Vector2 spawnPosition;
    //public Vector2 difficultyRange;
    public Enemy enemyToSpawn;
}

