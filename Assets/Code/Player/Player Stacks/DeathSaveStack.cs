using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Death Save", menuName = "Stacks/Death Save")]
public class DeathSaveStack : EventDepletableStack
{
    private StatModifier modifier = null;
    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Death_Save, StatModifierType.SET, 1);
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

    }
    public override void OnFinalDeplete()
    {
        base.OnFinalDeplete();
        PlayerStats.Instance.RemoveModifier(modifier);
    }
}
