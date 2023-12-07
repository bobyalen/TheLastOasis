using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCell
{
    //cell index
    public int x, y;
    //can be passed
    public bool isObstacle;
    public float f, g, h;
    public GridCell parent = null;
    //position of cell
    public Vector2 pos;
}

public class RoomScript : MonoBehaviour
{
    public bool isBoss;
    public bool isStart;
    public int roomIndex;
    public float roomDifficulty;
    public int distToStart;
    public EnemySpawnPosition easySpawnPosition;
    public EnemySpawnPosition mediumSpawnPosition;
    public EnemySpawnPosition hardSpawnPosition;
    [SerializeField]
    private List<Destructible> destructibles;
    [SerializeField]
    private GameObject Rocks;
    private List<GameObject> spawnedItems;
    public GridCell[,] pathFindingGrid;
    public List<EnemyBase> enemies;

    private void Awake()
    {
        spawnedItems = new List<GameObject>();
        GenerateGrid();
        EventManager.StartListening(Event.DoorsLockUnlock, LockUnlockDoors);
    }

    void GenerateGrid()
    {
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        pathFindingGrid = new GridCell[(int)collider.size.y, (int)collider.size.x];
        destructibles = transform.GetComponentsInChildren<Destructible>(true).ToList();
        List<Transform> rocks = new List<Transform>();
        if(Rocks != null)
        foreach(Transform t in Rocks.transform)
        {
            rocks.Add(t);
        }
        int columns = pathFindingGrid.GetLength(1);
        int rows = pathFindingGrid.GetLength(0);
        Vector2 pos = (Vector2)transform.position - new Vector2(columns / 2, -rows / 2);
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                //GridCell cell = pathFindingGrid[i, j];
                pathFindingGrid[i, j] = new GridCell();
                pathFindingGrid[i, j].pos = pos + new Vector2(j, -i);
                pathFindingGrid[i, j].x = j;
                pathFindingGrid[i, j].y = i;
                if(destructibles.Any(x => (Vector2)x.transform.position == pathFindingGrid[i, j].pos) ||
                    rocks.Any(x => (Vector2)x.transform.position == pathFindingGrid[i, j].pos))
                {
                    pathFindingGrid[i, j].isObstacle = true;
                }
                else
                {
                    pathFindingGrid[i, j].isObstacle = false;
                }
            }
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Event.RoomExit, OnRoomExit);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.DoorsLockUnlock, LockUnlockDoors);
        EventManager.StopListening(Event.RoomExit, OnRoomExit);
    }

    private void OnRoomExit(IEventPacket packet)
    {
        RoomExitPacket rep = packet as RoomExitPacket;
        if(rep.roomIndex == roomIndex)
        {
            foreach(GameObject go in spawnedItems)
            {
                if(go != null)
                    Destroy(go);
            }
            spawnedItems.Clear();
        }
    }

    public void AddtoSpawnedList(GameObject go)
    {
        spawnedItems.Add(go);
    }
    
    public void RemoveDestructibleFromList(Destructible d)
    {
        destructibles.Remove(d); 
        int columns = pathFindingGrid.GetLength(1);
        int rows = pathFindingGrid.GetLength(0);
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                if (pathFindingGrid[i,j].pos == (Vector2)d.transform.position)
                {
                    pathFindingGrid[i, j].isObstacle = true;
                    break;
                }
            }
        }
    }

    void LockUnlockDoors(IEventPacket packet)
    {
        DoorLockUnlockPacket dlup = packet as DoorLockUnlockPacket;
        if(dlup.roomIndex == roomIndex)
        {
            DoorManager dm = GetComponent<DoorManager>();
            if (!isStart)
            {
                dm.SetAllDoors(dlup.isUnlock);
            }
            else
                dm.SetAllDoors(true);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestructible(float healthSpawnChance, float coinSpawnChance)
    {
        foreach(Destructible d in destructibles)
        {
            float r;
            r = Random.Range(0.0f, 1.0f);
            if(r < healthSpawnChance)
            {
                d.AddHealth();
            }
            r = Random.Range(0.0f, 1.0f);
            if(r < coinSpawnChance)
            {
                d.AddCoin((int)(5 * roomDifficulty));
            }
        }
    }

    public void testRoom()
    {
        EventManager.TriggerEvent(Event.TestRoom, new RoomEnterPacket
        {
            roomCentre = transform.position,
            isBoss = isBoss,
            isStart = isStart,
            roomIndex = roomIndex,
            difficulty = roomDifficulty,
            enemyPositions = easySpawnPosition,
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.TriggerEvent(Event.RoomEnter, new RoomEnterPacket
            {
                roomCentre = transform.position,
                wasVisited = LevelGeneration.Instance.WasRoomVisited(roomIndex),
                isBoss = isBoss,
                isStart = isStart,
                roomIndex = roomIndex,
                difficulty = roomDifficulty,
                enemyPositions = SelectSpawnPosition()
            }) ;
            EventManager.TriggerEvent(Event.ChestSpawn, new ChestSpawnPacket
            {
                roomCentre = transform.position,
                roomIndex = roomIndex,
                difficulty = roomDifficulty,
                canSpawnChest = true
            });
            LevelGeneration.Instance.VisitRoom(roomIndex);
        }
    }

    private EnemySpawnPosition SelectSpawnPosition()
    {
        LevelGeneration levelGen = LevelGeneration.Instance;
        int currentVisitedRooms = levelGen.visitedRooms.Count();
        if (currentVisitedRooms < levelGen.mediumDistThreshold)
        {
            Debug.Log("Easy spawn selected");
            return easySpawnPosition;
        }
        else if (currentVisitedRooms < levelGen.hardDistThreshold)
        {
            Debug.Log("Medium spawn selected");
            return mediumSpawnPosition;
        }
        else
        {
            Debug.Log("Hard spawn selected");
            return hardSpawnPosition;
        }
    }
}
