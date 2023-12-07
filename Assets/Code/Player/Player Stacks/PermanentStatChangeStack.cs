using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanentStatChangeStack", menuName = "Stacks/PermanentChangeStack")]
public class PermanentStatChangeStack : PermanentStack
{
    [SerializeField]
    List<StatModifier> modifiers;
    public override void OnAdd()
    {
        foreach(StatModifier modifier in modifiers)
            PlayerStats.Instance.AddModifier(modifier);
    }
}
