using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContinuousEffectStack", menuName = "Stacks/Continuous Effect Stack")]
public class ContinuousEffectStack : PermanentStack
{
    [SerializeField]
    List<Effect> effects;
    [SerializeField]
    List<float> timers;
    
    public override void OnAdd()
    {
        for(int i = 0; i < effects.Count; i++)
            PlayerStacks.Instance.StartCoroutine(Timer(timers[i], effects[i]));
    }

    IEnumerator Timer(float seconds, Effect effect)
    {
        for(float i = 0; i < seconds; i += Time.deltaTime)
        {
            yield return new WaitUntil(() =>
            {
                return PlayerController.Instance.currentState != CURRENT_STATE.SCENE_CHANGE;
            });
        }
        DoEffect(effect);
        PlayerStacks.Instance.StartCoroutine(Timer(seconds, effect));
    }

    public void DoEffect(Effect effect)
    {
        int value = effect.value;
        if (effect.operation == Operation.DECREASE)
            value = -value;
        switch(effect.target)
        {
            case Target.Current_Health:
                PlayerStats.Instance.cachedCalculatedValues[Stat.Current_Health] += value;
                break;
            case Target.Coins:
                Inventory.Instance.AddCoins(value, true);
                break;
        }
    }

}

[Serializable]
public class Effect
{
    public Target target;
    public Operation operation;
    public int value;
}

public enum Target
{
    Current_Health,
    Coins
}

public enum Operation
{
    INCREASE,
    DECREASE
}

