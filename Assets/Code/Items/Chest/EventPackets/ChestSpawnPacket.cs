using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPacket : IEventPacket
{
    public Vector3 roomCentre;
    //public Vector2 chestPos;
    public int roomIndex;
    public float difficulty;
    public bool canSpawnChest;

}
