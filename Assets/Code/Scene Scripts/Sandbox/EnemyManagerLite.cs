using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class EnemyManagerLite : MonoBehaviour
{
    //TODO: add to singleton;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject room;
    [SerializeField]
    private List<Enemy> enemies;
    [SerializeField]
    RoomScript roomScript;
    Enemy enemyData;

    public Dictionary<int, List<EnemyRuntimeData>> spawnedEnemies;
    private List<int> hasSpawned;

    [Header("Debug")]
    public bool SpawnOnStart = false;
    void Start()
    {
        EventManager.StartListening(Event.EnemyDestroyed, OnEnemyDestroyed);
        EventManager.StartListening(Event.TestRoom, Onspawn);
        spawnedEnemies = new Dictionary<int, List<EnemyRuntimeData>>();
        hasSpawned = new List<int>();
        enemyData = enemies[0];
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyDestroyed, OnEnemyDestroyed);
        EventManager.StopListening(Event.TestRoom, Onspawn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("1 pressed");
            enemyData = enemies[0];
            roomScript.testRoom();
        }if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("2 pressed");
            enemyData = enemies[1];
            roomScript.testRoom();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("3 pressed");
            enemyData = enemies[2];
            roomScript.testRoom();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("4 pressed");
            enemyData = enemies[3];
            roomScript.testRoom();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("5 pressed");
            enemyData = enemies[4];
            roomScript.testRoom();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Debug.Log("6 pressed");
            enemyData = enemies[5];
            roomScript.testRoom();
        }
    }

    public void SpawnEnemies(RoomEnterPacket e)
    {
        if (e.isBoss)
            return;
        if (!SpawnOnStart)
            if (e.isStart)
                return;
        //for the sake of the task, let's spawn enemies inside every room, at least 1
        float currentDifficulty = 0;
        List<Vector2> filledPositions = new List<Vector2>();
        for (int i = 0; i < e.enemyPositions.enemySpawns.Count; i++)
        {
            EnemySpawn es = e.enemyPositions.enemySpawns[i];
            var go = Instantiate(enemyData.prefabToSpawn, e.roomCentre + es.spawnPosition, Quaternion.identity);
            go.transform.parent = room.transform;

            EnemyRuntimeData erd = new EnemyRuntimeData()
            {
                go = go,
                SpawnPos = e.roomCentre + es.spawnPosition,
                maxHealth = enemyData.MaxHealth
            };

            //go.GetComponent<SpriteRenderer>().color = enemyData.color;

            //setting the index;
            EnemyBase eb = go.GetComponent<EnemyBase>();
            eb.roomIndex = e.roomIndex; 
            eb.currentHealth = enemyData.MaxHealth;
            eb.attackDamage = enemyData.Damage;
            eb.multiplier = enemyData.multiplier;
            eb.enemyData = enemyData;
            //eb.onCollisionDamage = enemyData.DamageOnCollision;
            eb.rs = room.GetComponent<RoomScript>();
            eb.rs.enemies.Add(eb);
            eb.GetComponent<SpriteRenderer>().sprite = enemyData.Sprite;
            foreach (ItemDrop id in enemyData.itemDrops)
            {
                float random = Random.Range(0.0f, 1.0f);
                if (random <= id.dropProbability)
                {
                    eb.lootToDrop.Add(id.itemType, Random.Range(id.minItemQuantity, id.maxItemQuantity + 1));
                }
            }
            AddEnemyToDictionary(erd, e.roomIndex);
        }
    }

    Vector2 DecideEnemySpawnPosition(EnemySpawnPosition esp, List<Vector2> takenPositions)
    {
        List<EnemySpawn> buffer = new List<EnemySpawn>(esp.enemySpawns);
        buffer.RemoveAll(x => takenPositions.Contains(x.spawnPosition));
        while (buffer.Count == 0)
        {
            buffer = new List<EnemySpawn>(esp.enemySpawns);
            int mult = (takenPositions.Count / esp.enemySpawns.Count) % 2;
            int inc = (takenPositions.Count / esp.enemySpawns.Count);
            if (mult == 0)
                mult--;
            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i].spawnPosition += Vector2.left * mult * inc * 0.5f;
            }

            buffer.RemoveAll(x => takenPositions.Contains(x.spawnPosition));

        }
        int index = Random.Range(0, buffer.Count);
        takenPositions.Add(buffer[index].spawnPosition);
        return buffer[index].spawnPosition;
    }

    private void OnEnemyDestroyed(IEventPacket packet)
    {
        EnemyDestroyedPacket edp = packet as EnemyDestroyedPacket;

        //removing the reference to the enemy in the list of spawned enemies
        int index = edp.go.GetComponent<EnemyBase>().roomIndex;
        spawnedEnemies[index].RemoveAll(x => x.go == edp.go);
        if (spawnedEnemies[index].Count == 0)
        {
            spawnedEnemies.Remove(index);
            EventManager.TriggerEvent(Event.SpawnObelisk, null);
        }

        //awarding the player the necessary items
        RoomScript rs = edp.go.transform.root.GetComponent<RoomScript>();
        foreach (var kvp in edp.lootToDrop)
        {
            if (kvp.Key is CollectableData)
            {
                CollectableData cd = kvp.Key as CollectableData;
            }
        }

        Destroy(edp.go);
    }

    private void DisableEnemies(int index)
    {
        if (spawnedEnemies.ContainsKey(index) == false)
            return;
        foreach (var enemy in spawnedEnemies[index])
        {
            IEnemyBehaviour behaviourComponent = enemy.go.GetComponent<IEnemyBehaviour>();
            enemy.go.transform.position = enemy.SpawnPos;
            enemy.go.GetComponent<EnemyBase>().currentHealth = enemy.maxHealth;
            if (behaviourComponent != null)
            {
                behaviourComponent.ResetEnemy();
            }
            enemy.go.SetActive(false);
        }
    }
    void AddEnemyToDictionary(EnemyRuntimeData erd, int index)
    {
        if (spawnedEnemies.ContainsKey(index))
        {
            spawnedEnemies[index].Add(erd);
        }
        else
        {
            spawnedEnemies.Add(index, new List<EnemyRuntimeData>());
            spawnedEnemies[index].Add(erd);
        }
    }

    private void Onspawn(IEventPacket packet)
    {
        Debug.Log("onspawn");
        RoomEnterPacket e = packet as RoomEnterPacket;
        SpawnEnemies(e);
    }
}
