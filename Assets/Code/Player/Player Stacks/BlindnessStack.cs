using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlindnessModifierStack", menuName = "Stacks/Blindness Modifier Stack")]
public class BlindnessStack : PermanentStack
{
    private StatModifier modifier = null;
    public StatModifierType modifierType;
    public float value = 0.0f;

    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Blindness, modifierType, value);
    }

    public override void OnAdd()
    {
        PlayerStats.Instance.AddModifier(modifier);
    }
}
