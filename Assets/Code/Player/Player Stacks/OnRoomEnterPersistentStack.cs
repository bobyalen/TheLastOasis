using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "On Room Enter Persistent Stack", menuName = "Stacks/On Room Enter Persistent Stack")]
public class OnRoomEnterPersistentStack : OnEventPersistentStack
{
    private void OnValidate()
    {
        RegainOnEvent = Event.RoomEnter;
    }
    protected override bool RegainCondition(IEventPacket packet)
    {
        RoomEnterPacket rep = packet as RoomEnterPacket;
        if(rep != null)
        {
            return !rep.wasVisited;
        }
        return false;
    }
}
