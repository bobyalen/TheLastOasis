using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyedPacket : IEventPacket
{
    public GameObject go;
    public Dictionary<Item, int> lootToDrop;
}
