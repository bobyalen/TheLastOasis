using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyType
{
    GOON,
    BOSS
}

[CreateAssetMenu(fileName = "collectabledataScriptableobject", menuName = "ScriptableObjects/Enemies/Enemy")]

public class Enemy : ScriptableObject
{
    public string Name;
    public float MaxHealth;
    public Sprite Sprite;
    public int Damage;
    public float difficulty;
    public float multiplier = 1.0f;
    public GameObject prefabToSpawn;
    public List<ItemDrop> itemDrops;
    //debug only, differentiates enemies, remove on future iterations, or when we have sprites
}
