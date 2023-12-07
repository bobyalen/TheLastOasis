using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMovementBehaviour
{
    public Vector2 GetNextMovement();

    public void StopMovement();

    public void ResumeMovement();
}

public abstract class BaseMovementBehaviour : MonoBehaviour, IMovementBehaviour, IEnemyBehaviour
{
    protected bool canMove = true;
    [SerializeField]
    protected Rigidbody2D rb;
    protected bool isSpriteFlipped = false;

    public void Start()
    {
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
    }

    public void Update()
    {
        Act();
    }

    public void OnDestroy()
    {
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
    }
    public Vector2 GetNextMovement()
    {
        return GetMovement();
    }

    protected abstract Vector2 GetMovement();

    public void ResumeMovement()
    {
        canMove = true;
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void Act()
    {
        if (canMove)
            rb.velocity = GetNextMovement();
        else
            rb.velocity = Vector2.zero;
        SetAnimatorVariables();

    }

    public void ResetEnemy()
    {
        canMove = true;
        rb.velocity = Vector2.zero;
    }

    public void SetAnimatorVariables()
    {
        isSpriteFlipped = rb.velocity.x < 0;
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
}
