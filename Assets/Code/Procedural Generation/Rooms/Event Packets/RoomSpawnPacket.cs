using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPacket : IEventPacket
{
    public int doors;
    public GameObject go;
    public int distToStart;
    public int distToBoss;
}
