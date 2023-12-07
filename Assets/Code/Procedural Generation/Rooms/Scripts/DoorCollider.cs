using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N = 0,
    E = 1,
    S = 2,
    W = 3
}
public class DoorCollider : MonoBehaviour
{
    public Direction dir;
    [SerializeField]
    RoomScript room;
    private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioManager.instance.PlayDoorSound(clip);
            EventManager.TriggerEvent(Event.RoomExit, new RoomExitPacket()
            {
                roomIndex = room.roomIndex,
                nextRoomIndex = LevelGeneration.Instance.GetNeighbourOfRoom(room.roomIndex, dir),
                direction = dir,
            }); 
        }
    }
}
