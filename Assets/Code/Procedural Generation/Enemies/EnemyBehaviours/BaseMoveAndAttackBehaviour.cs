using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseMoveAndAttackBehaviour : MonoBehaviour, IMovementBehaviour, IEnemyBehaviour, IAttackBehaviour
{

    //MonoBehaviour functions

    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    protected float speed;
    // Start is called before the first frame update

    protected bool canMove = true;
    protected bool isAttacking = false;
    protected bool isSpriteFlipped = false;

    protected void Start()
    {
        AddAdditionalEventListeners();
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
    }

    private void Update()
    {
        Act();
    }

    private void OnDestroy()
    {
        RemoveAdditionalEventListeners();
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
    }

    //Enemy Behaviour interface
    public void Act()
    {
        DoAct();
    }
    public void OnHit(IEventPacket packet)
    {
        PlayerHitPacket php = packet as PlayerHitPacket;
        if (php.enemy == this.gameObject)
        {
            OnHitAction();
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                damage = (int)php.damage,
                position = this.gameObject.transform.position,
                textColor = Color.yellow

            });
        }
    }

    protected abstract void OnHitAction();

    public void ResetEnemy()
    {

        rb.velocity = Vector2.zero;
        canMove = true;
        isAttacking = false;
        StopAttack();
    }

    public void SetAnimatorVariables()
    {
        DoSetAnimatorVariables();
    }

    //Enemy Movement interface

    public Vector2 GetNextMovement()
    {
        return GetMovement();
    }

    public void ResumeMovement()
    {
        canMove = true;
    }

    public void StopMovement()
    {
        canMove = false;
    }

    //Enemy Attack Interface
    public void BeginAttack()
    {
        DoBeginAttack();
    }

    public void StopAttack()
    {
        DoStopAttack();
    }

    public void Attack()
    {
        DoAttack();
    }

    protected abstract void DoAct();
    protected abstract void DoAttack();
    protected abstract void DoBeginAttack();
    protected abstract void DoStopAttack();
    protected abstract void DoSetAnimatorVariables();
    protected abstract Vector2 GetMovement();
    protected abstract void AddAdditionalEventListeners();
    protected abstract void RemoveAdditionalEventListeners();
}
