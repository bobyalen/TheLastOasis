using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDamageIncrease", menuName = "Stacks/Enemy Damage Increase")]
public class EnemyDamageIncreaseStack : EventDepletableStack
{
    private StatModifier modifier = null;
    public float percentage = 0.0f;
    private int roomAddedIndex = -1;

    private void OnValidate()
    {
        modifier = new StatModifier(Stat.Enemy_Dmg, StatModifierType.NUMERICAL, percentage);
    }

    public override bool DepleteCondition(IEventPacket packet)
    {
        RoomExitPacket rep = packet as RoomExitPacket;
        if(rep != null)
        {
            return !LevelGeneration.Instance.WasRoomVisited(rep.nextRoomIndex) && rep.roomIndex != roomAddedIndex;
        }
        return false;
    }
    public override void OnAdd()
    {
        base.OnAdd();
        roomAddedIndex = LevelGeneration.Instance.roomIndex;
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
