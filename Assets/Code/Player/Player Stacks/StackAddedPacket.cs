using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackAddedPacket : IEventPacket
{
    public int stackNumber = 1;
    public Stack stackToAdd;
    public int currentRoom = -1;
}
