using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpriteAssigner : MonoBehaviour
{
    [SerializeField]
    List<Direction> directionsToListen;
    [SerializeField]
    Sprite waterSprite;
    // Start is called before the first frame update
    void Awake()
    {
        EventManager.StartListening(Event.RoomSpawn, SetDoor);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.RoomSpawn, SetDoor);
    }

    void SetDoor(IEventPacket packet)
    {
        RoomSpawnPacket rsp = packet as RoomSpawnPacket;
        if(rsp.go.transform == transform.root)
        {
            for(int i = 0; i < 4; i++)
            {
                bool isDoor = (rsp.doors & (1 << i)) != 0;
                if(!isDoor && directionsToListen.Contains((Direction)i))
                {
                    GetComponent<SpriteRenderer>().sprite = waterSprite;
                }
                else if(isDoor && rsp.distToStart >= rsp.distToBoss / 2)
                {
                    GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
