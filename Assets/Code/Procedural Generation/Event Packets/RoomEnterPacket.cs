using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterPacket : IEventPacket
{
    public Vector2 roomCentre;
    public int roomIndex;
    public bool wasVisited = false;
    public float difficulty;
    public bool isBoss;
    public bool isStart;
    public EnemySpawnPosition enemyPositions;
}
