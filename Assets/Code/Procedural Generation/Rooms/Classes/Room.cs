using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Vector2 gridPos;
    public int distToStart = -1;
    public int type;
    public int doors = 0;
    public int x, y;
    public GameObject go;

    public Room(Vector2 _gridPos, int _type)
    {
        gridPos = _gridPos;
        type = _type;
    }
}
