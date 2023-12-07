using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected EnemyBase enemyBase;
    public bool IsAttacking = false;
    public bool hasAttacked = false;
    public SpriteRenderer hitboxRenderer;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!IsAttacking)
            {
                //send message to attack
                EventManager.TriggerEvent(Event.EnemyHitboxEntered, new EnemyHitboxEnteredPacket()
                {
                    Hitbox = this.gameObject
                });
            }
            else if (IsAttacking && !hasAttacked)
            {
                EventManager.TriggerEvent(Event.EnemyHitPlayer, new EnemyHitPacket()
                {
                    healthDeplete = enemyBase.attackDamage,
                    playerInvulnerability = PlayerController.Instance.invulnerability,
                    playerDivineShield = PlayerStats.Instance.cachedCalculatedValues[Stat.Divine_Shield] > 0
                });
                hasAttacked = true;
            }
        }
    }
}
