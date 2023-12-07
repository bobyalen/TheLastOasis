using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stack : ScriptableObject
{
    public abstract void OnAdd();
}

public abstract class DepletableStack : Stack
{
    public abstract void OnDeplete();
    public abstract void OnFinalDeplete();
}
public abstract class TimeDepletableStack : DepletableStack
{
    public float timeToDeplete;
}

public abstract class EventDepletableStack : DepletableStack
{
    public Event depleteOnEvent;

    public override void OnAdd()
    {
        if(PlayerStacks.Instance.ContainsStack(this) == false)
        EventManager.StartListening(depleteOnEvent, OnDeplete);
    }

    public void OnDeplete(IEventPacket packet)
    {
        if(DepleteCondition(packet))
        if (PlayerStacks.Instance.RemoveStack(this))
        {
            OnFinalDeplete();
        }
        else
        {
            OnDeplete();
        }
             
    }
    public abstract bool DepleteCondition(IEventPacket packet);
    public override void OnFinalDeplete()
    {
        EventManager.StopListening(depleteOnEvent, OnDeplete);
    }
    public void RemoveEventListeners()
    {
        EventManager.StopListening(depleteOnEvent, OnDeplete);
    }
}

public abstract class PermanentStack : Stack
{

}

public abstract class PersistentStack : Stack
{
}


