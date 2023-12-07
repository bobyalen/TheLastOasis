using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinLossModifierStack", menuName = "Stacks/Coin Loss Modifier Stack")]
public class CoinLossStack : PermanentStack
{
    private StatModifier modifier = null;
    public StatModifierType modifierType;
    public float value = 0.0f;

    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Coin_Loss, modifierType, value);
    }

    public override void OnAdd()
    {
        PlayerStats.Instance.AddModifier(modifier);
    }
}
