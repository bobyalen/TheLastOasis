using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatValues", menuName = "Stats/Base Stat Values")]
public class StatValues : ScriptableObject
{
    public List<StatValue> statValues;
}

[Serializable]
public class StatValue
{
    public Stat stat;
    public float value;
}
