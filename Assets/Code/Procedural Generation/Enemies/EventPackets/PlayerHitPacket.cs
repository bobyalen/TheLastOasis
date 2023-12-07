using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitPacket : IEventPacket
{
    public GameObject enemy;
    public float damage;
}
