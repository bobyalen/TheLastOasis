using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingGainChange", menuName = "Stacks/Permanent Healing Gain Change")]
public class HealingGainChangeStack : PermanentStack
{
    private StatModifier modifier = null;
    public StatModifierType modifierType;
    public float value = 0.0f;

    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Healing, modifierType, value);
    }

    public override void OnAdd()
    {
        PlayerStats.Instance.AddModifier(modifier);
    }
}
