using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackFunctions
{
    //Swing - 
    public static IEnumerator Swing(IAttackBehaviour atk, IMovementBehaviour move, float windUpTime, float strikeTime)
    {
        move.StopMovement();
        for (float i = 0; i < windUpTime; i += Time.deltaTime)
            yield return null;
        atk.BeginAttack();
        for (float i = 0; i < strikeTime; i += Time.deltaTime)
            yield return null;
        atk.StopAttack();
        move.ResumeMovement();
    }
    public static IEnumerator Smash(IAttackBehaviour atk, float windUpTime, float strikeTime)
    {
        for (float i = 0; i < windUpTime; i += Time.deltaTime)
            yield return null;
        atk.BeginAttack();
        for (float i = 0; i < strikeTime; i += Time.deltaTime)
            yield return null;
        atk.StopAttack();
    }
}
