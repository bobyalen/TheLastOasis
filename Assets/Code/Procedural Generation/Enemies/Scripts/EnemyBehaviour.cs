using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyBehaviour
{
    public void Act();
    public void ResetEnemy();
    public void SetAnimatorVariables();
    public void OnHit(IEventPacket packet);
}


public interface IAttackBehaviour
{
    public void BeginAttack();

    public void StopAttack();

    public void Attack();
}
