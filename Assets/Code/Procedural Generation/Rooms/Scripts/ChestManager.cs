//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChestManager : MonoBehaviour
{
    [SerializeField] private GameObject chestPrefab;
    //List of game objects to spawn 
    private Dictionary<int, List<GameObject>> spawnedChests;
    private List<int> hasSpawnedChest;
    //Chest spawn chance set in inspector, 0.5 = 50%
    [SerializeField] private float spawnProbability = 0.5f;
    [SerializeField] private int maxNumberChests = 8;
    [SerializeField] private int chestCounter = 0;

    void Start()
    {
        EventManager.StartListening(Event.ChestSpawn, OnRoomEnter);
        
        spawnedChests = new Dictionary<int, List<GameObject>>();
        hasSpawnedChest = new List<int>();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.ChestSpawn, OnRoomEnter);

    }

    
    private void OnRoomEnter(IEventPacket packet)
{
    ChestSpawnPacket csp = packet as ChestSpawnPacket;
    if (hasSpawnedChest.Contains(csp.roomIndex))
    {
        ActivateChests(csp.roomIndex);
    }
    else
    {
        //Add current room index to list of visited rooms, then spawn the chest
        hasSpawnedChest.Add(csp.roomIndex);
        SpawnChestInRoom(csp);
    }

}
    private void SpawnChestInRoom(ChestSpawnPacket csp)
    {
        Vector3 pos = Vector2.zero;
        //For now spawn in room center
        pos.x = csp.roomCentre.x;
        pos.y = csp.roomCentre.y;
        //Instantiate new game object of chest prefab
        //Add chest index counter, Caps spawn rate to max number of chests with counter, checks if spawn probability is 50%
        if (Random.value > spawnProbability && chestCounter < maxNumberChests)
        {
            var go = Instantiate(chestPrefab, pos, Quaternion.identity);
            //Set index by getting component of script attached to chest prefab
            ChestControl cc = go.GetComponent<ChestControl>();
            cc.roomIndex = csp.roomIndex;
            AddChestToDictionary(go, csp.roomIndex);
            chestCounter++;
            /// <summary>
            ///Spawn a chest upon entering a room event, the given probability is 50% for now
            ///a) the chance a room will contain a chest
            ///b) we've already spawned the max number of chest
           /// we don't spawn another
            /// </summary>
        }
    }
    private void ActivateChests(int index)
    {
        if (spawnedChests.ContainsKey(index) == false)
            return;
    }

    private void DisableChests(int index)
    {
        if (spawnedChests.ContainsKey(index) == false)
            return;
    }
    void AddChestToDictionary(GameObject go, int index)
    {
        if (spawnedChests.ContainsKey(index))
        {
            spawnedChests[index].Add(go);
        }
        else
        {
            spawnedChests.Add(index, new List<GameObject>());
            spawnedChests[index].Add(go);
        }
    }
}

