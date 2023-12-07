using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    const int doorLayer = 6;
    const int wallLayer = 8;

    List<bool> doorOpen;
    public int doorsBits;
    public int x, y;
    [SerializeField]
    RoomScript roomScript;
    //ALL door objects in order N->E->S->W
    //
    [SerializeField]
    List<GameObject> doors = new List<GameObject>();
    [SerializeField]
    List<GameObject> doorActiveGrids = new List<GameObject>();
    [SerializeField]
    List<GameObject> doorInactiveGrids = new List<GameObject>();
    [SerializeField]
    List<GameObject> doorLights = new List<GameObject>();

    void Awake()
    {
    }
    private void Start()
    {
        doorOpen = new List<bool>();
        for (int i = 0; i < doors.Count; i++)
            doorOpen.Add(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReinitialiseDoors(int doorsBits)
    {
        for(int i =0; i < 4; i++)
        {
            bool isDoor = (doorsBits & (1 << i)) != 0;
            var doorLockedSprite = doors[i].GetComponent<SpriteRenderer>();
            if (isDoor)
            {
                doors[i].layer = doorLayer;
                doorInactiveGrids[i].SetActive(false);
                doorActiveGrids[i].SetActive(true);
                doorLockedSprite.enabled = true;
                doorLights[i].SetActive(false);
            }
            else
            {
                //there is no door;
                //toggle collision on:
                doors[i].layer = wallLayer;
                //toggle inactive tileMap on;
                doorInactiveGrids[i].SetActive(true);
                //toggle active tileMap off;
                doorActiveGrids[i].SetActive(false);
                doorLockedSprite.enabled = false;
                doorLights[i].SetActive(false);
            }
        }
    }

    public void SetAllDoors(bool isOpen)
    {
        for(int i = 0; i < 4; i++)
        {
            bool isDoor = (doorsBits & (1 << i)) != 0;
            if(isDoor)
            {
                var doorLockedSprite = doors[i].GetComponent<SpriteRenderer>();
                if(!isOpen)
                {
                    doorLockedSprite.enabled = true;
                    doorLights[i].SetActive(false);
                    doors[i].layer = 8;
                }
                else
                {
                    doorLockedSprite.enabled = false;
                    doorLights[i].SetActive(true);
                    doors[i].layer = 6;
                }
            }
        }
    }

    public GameObject GetDoor(int direction)
    {
        return doors[direction];
    }

}
