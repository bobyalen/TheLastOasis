using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnEventPersistentStack : PermanentStack
{
    [SerializeField]
    protected Event RegainOnEvent;
    [SerializeField]
    List<StackToAdd> stacksToAddOnRegain;
    public override void OnAdd()
    {
        EventManager.StartListening(RegainOnEvent, OnRegain);
    }

    void OnRegain(IEventPacket packet)
    {
        if(packet != null)
        {
            if(RegainCondition(packet))
            {
                foreach(var stack in stacksToAddOnRegain)
                {
                    EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                    {
                        stackNumber = stack.value,
                        stackToAdd = stack.stack
                    });
                }
            }
        }
    }

    protected abstract bool RegainCondition(IEventPacket packet);

}
