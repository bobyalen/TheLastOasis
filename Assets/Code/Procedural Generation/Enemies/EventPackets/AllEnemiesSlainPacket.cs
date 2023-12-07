using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockUnlockPacket : IEventPacket
{
    public int roomIndex;
    public bool isUnlock;
}
