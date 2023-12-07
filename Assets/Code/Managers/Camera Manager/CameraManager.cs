using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineConfiner camConfiner;
    private RoomScript room;

    public bool changeRoom;
    private void Start()
    {
        room = FindObjectOfType<RoomScript>();
        camConfiner.GetComponent<CinemachineConfiner>();
    }
    private void Update()
    {
        if(room == null)
            room = FindObjectOfType<RoomScript>();
        if(room.enabled && !changeRoom)
        {
            changeRoom = true;
            camConfiner.m_BoundingShape2D = room.GetComponentInChildren<PolygonCollider2D>();
        }
        else
        {
            room = FindObjectOfType<RoomScript>();
            changeRoom = false;
        }
    }
}
