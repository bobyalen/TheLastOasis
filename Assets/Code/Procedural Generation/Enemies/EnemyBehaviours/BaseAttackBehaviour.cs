using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackBehaviour : MonoBehaviour, IEnemyBehaviour, IAttackBehaviour
{

    // Start is called before the first frame update
    protected void Start()
    {
        AddAdditionalEventListeners();
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
    }
    // Update is called once per frame
    void Update()
    {
        Act();
    }

    void OnDestroy()
    {
        RemoveAdditionalEventListeners();
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
    }
    public void Act()
    {
        DoAct();
    }

    public void Attack()
    {
        DoAttack();
    }

    public void BeginAttack()
    {
        DoBeginAttack();
    }
    public void StopAttack()
    {
        DoStopAttack();
    }

    public void OnHit(IEventPacket packet)
    {
        PlayerHitPacket php = packet as PlayerHitPacket;
        if(php.enemy == this.gameObject)
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

    public void ResetEnemy()
    {
        throw new System.NotImplementedException();
    }

    public void SetAnimatorVariables()
    {
        DoSetAnimatorVariables();
    }

    protected abstract void DoAct();
    protected abstract void DoAttack();
    protected abstract void DoBeginAttack();
    protected abstract void DoStopAttack();
    protected abstract void OnHitAction();
    protected abstract void DoSetAnimatorVariables();
    protected abstract void AddAdditionalEventListeners();
    protected abstract void RemoveAdditionalEventListeners();
}

