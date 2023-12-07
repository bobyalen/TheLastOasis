using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPacket : IEventPacket
{
    public float healthDeplete;
    public bool playerInvulnerability = false;
    public bool playerDivineShield = false;
}
