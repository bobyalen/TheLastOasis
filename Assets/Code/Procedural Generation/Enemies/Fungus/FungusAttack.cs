using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusAttack : AttackBase
{

    float timer = 0;
    float timeToAddStacks = 0.5f;
    [SerializeField]
    float timeToDepleteStacks;
    [SerializeField]
    int stackDamage;
    bool isInMist = false;
    private FungusPoisonStack stack;

    private void Start()
    {
        stack = new FungusPoisonStack(stackDamage, timeToDepleteStacks);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsAttacking)
            {
                //send message to attack
                EventManager.TriggerEvent(Event.EnemyHitPlayer, new EnemyHitPacket()
                {
                    healthDeplete = enemyBase.attackDamage,
                    playerInvulnerability = PlayerController.Instance.invulnerability,
                    playerDivineShield = PlayerStats.Instance.cachedCalculatedValues[Stat.Divine_Shield] > 0
                });
                isInMist = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInMist = false;
        }
    }

    public void StopAttack()
    {
        isInMist = false;
    }

    private void Update()
    {
        if(isInMist)
        {
            timer += Time.deltaTime;
            if(timer > timeToAddStacks)
            {
                EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                {
                    stackToAdd = stack
                });
                timer = 0;
            }
        }
    }
}
