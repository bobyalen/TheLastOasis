using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusPoisonStack : TimeDepletableStack
{
    [SerializeField]
    private int damagePerStack;
    public FungusPoisonStack(int damagePerStack, float timeToDeplete)
    {
        this.damagePerStack = damagePerStack;
        this.timeToDeplete = timeToDeplete;
    }
    public override void OnDeplete()
    {
        PlayerStats.Instance.WoundPlayer(damagePerStack, false);
        EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
        {
            textColor = Color.green,
            damage = damagePerStack,
            position = PlayerController.Instance.transform.position
        });
    }

    public override void OnAdd()
    {

    }

    public override void OnFinalDeplete()
    {

    }
}
