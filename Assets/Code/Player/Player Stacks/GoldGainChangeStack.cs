using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingGainChange", menuName = "Stacks/Permanent Gold Gain Change")]
public class GoldGainChangeStack : PermanentStack
{
    private StatModifier modifier = null;
    public StatModifierType modifierType;
    public float value = 0.0f;

    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Coin_Gain, modifierType, value);
    }

    public override void OnAdd()
    {
        PlayerStats.Instance.AddModifier(modifier);
    }
}
