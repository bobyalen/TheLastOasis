using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealtPacket : IEventPacket
{
    public Color textColor;
    public int damage;
    public Vector2 position;
}
