using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Poison Stack", menuName = "Stacks/Enemy Poison Stack")]
public class EnemyPoisonStack : TimeDepletableStack
{

    [SerializeField]
    private int damagePerStack;
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
