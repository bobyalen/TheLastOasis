using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackModifier", menuName = "Stacks/Enemy Attack Modifier")]
public class EnemyAttackModifier : PermanentStack
{
    [SerializeField]
    List<StackToAdd> stacksToAdd;

    public override void OnAdd()
    {
        EventManager.StartListening(Event.EnemyHitPlayer, OnHit);
    }

    private void OnHit(IEventPacket packet)
    {
        EnemyHitPacket ehp = packet as EnemyHitPacket;
        if(ehp != null)
        {
            if(ehp.playerInvulnerability == false && !ehp.playerDivineShield)
            {
                foreach(var stack in stacksToAdd)
                {
                    EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                    {
                        stackNumber = stack.value,
                        stackToAdd = stack.stack
                    });
                }
            }
        }
    }
}
[Serializable]
public class StackToAdd
{
    public Stack stack;
    public int value;
}
