using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DivineShieldStack", menuName = "Stacks/Divine Shield Stack")]
public class DivineShieldStack : EventDepletableStack
{
    private StatModifier modifier = null;
    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Divine_Shield, StatModifierType.SET, 1);
    }
    public override bool DepleteCondition(IEventPacket packet)
    {
        return true;
    }
    public override void OnAdd()
    {
        base.OnAdd();
        PlayerStats.Instance.AddModifier(modifier);
    }

    public override void OnDeplete()
    {
        PlayerStats.Instance.RemoveModifier(modifier);
    }
    public override void OnFinalDeplete()
    {
        base.OnFinalDeplete();
        PlayerStats.Instance.RemoveModifier(modifier);
    }
}
