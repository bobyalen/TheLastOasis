using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="LevelGenerationData", menuName="Level Generation/Level Generation Data", order = 0)]
public class LevelGenerationData : ScriptableObject
{

    public float healthSpawnChanceDestructible;
    public float coinSpawnChanceDestructible;
    public float mediumThreshold;
    public float highThreshold;
    public Vector2 easyDifficultyRange;
    public Vector2 mediumDifficultyRange;
    public Vector2 highDifficultyRange;
}
