using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public KeyCode chestOpenKey;
    public GameObject coinPrefab;
    public GameObject coinPilePrefab;
    public GameObject coinBagPrefab;
    //Prefab list
    //TODO Dylan: modify this so that it holds a list of all items to spawn, alongside their amounts
    //You'll need to do something like the way enemies select which items to drop.
    [SerializeField]
    private CollectableData itemToSpawn;
    [SerializeField]
    //3 types: Coin, CoinPile, CoinBag
    private List<CollectableData> coinsToSpawn;
    [SerializeField]
    //Store max content size of Chest
    private int[] maxStorage = new int[100];
    
    void Update()
    {
        SpawnCoinsFromChest();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var open = gameObject.GetComponent<ChestControl>().isOpen;
        //Prompts player with open message if in range and chest hasnt been opened
        if (collision.gameObject.CompareTag("Player") && !open)
        {
            playerInRange = true;
            MessageManager.instance.DisplayChestText();
        }
        //Notifies user theyve already interacted with this chest 
        if (collision.gameObject.CompareTag("Player") && open)
        {
            playerInRange = true;
            MessageManager.instance.DisableChestText();
            MessageManager.instance.DisplayChestInteractText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //When player is out of range, disable canvas text elements
            playerInRange = false;
            MessageManager.instance.DisableChestText();
            MessageManager.instance.DisableChestInteractText();

        }
    }

    public void SpawnCoinsFromChest()
    {
        var open = gameObject.GetComponent<ChestControl>().isOpen;
        var Chest = gameObject.GetComponent<ChestControl>();
        if (playerInRange && !open && InteractWithNoEnemies())
        {
            if (Input.GetKeyDown(chestOpenKey))
            {   
                Chest.OpenChest();

                //Set random range value of coins to corresponding variable
                int easyCoins = Random.Range(1, Chest.maxNumberCoins/4);
                int mediumCoins = Random.Range(5, Chest.maxNumberCoins/2);
                int hardCoins = Random.Range(10, Chest.maxNumberCoins);
                //Loop through number of coins and spawn the random value
                for (int i = 0; i < easyCoins; i++)
                {
                    if (GetDifficulty() <= 1.5)
                    {
                        Transform t = transform;
                        Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                        t.position += r;
                        ItemSpawnManager.Instance.Spawn(itemToSpawn, t, easyCoins);
                        t.position -= r;
                    }
                }
                for (int i =0; i < mediumCoins; i++)
                {
                    if (GetDifficulty() <= 3 && GetDifficulty() > 1.5)
                    {
                        Transform t = transform;
                        Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                        t.position += r;
                        ItemSpawnManager.Instance.Spawn(itemToSpawn, t, mediumCoins);
                        t.position -= r;
                    }
                }
                //Hardest difficulty loops through 10-20 coins and spawns the range as long as the float difficulty meets the condition
                for (int i =0; i < hardCoins;i++)
                {
                    if (GetDifficulty() <= 6 && GetDifficulty() > 3)
                    {
                        Transform t = transform;
                        Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                        t.position += r;
                        ItemSpawnManager.Instance.Spawn(itemToSpawn, t, hardCoins);
                        t.position -= r;
                    }
                }
               //INCREASE COINPILE & COINBAG DROP CHANCE IN THE HIGHER DIFFICULTIES
                /// <summary>
                /// Checks if player is in range and hasnt been interacted with
                /// Key input E to invoke an event
                /// Gets reference to the chest control function
                /// Instantiates coin prefab at a fixed position in scene from the chest
                /// Spawn random number of coins depending on the Inspector value assigned
                /// </summary>

            }
        }
    }

    private float GetDifficulty()
    {
        //Get current room index of the chest, store it in variable
        int curRoom = gameObject.GetComponent<ChestControl>().roomIndex;
        //Find all objects with roomscript at runtime and store them in an array
        RoomScript[] rooms = FindObjectsOfType<RoomScript>();
        RoomScript room = null;
        //Iterate through number of rooms
        for (int i = 0; i < rooms.Length; i++)
        {
            //Set the current index of the arrays room index equal to the chest index
            if (rooms[i].roomIndex == curRoom)
            {
                room = rooms[i];
                break;
            }


        }
        //Store the rooms difficulty the chest is in
        float totalDif = room.roomDifficulty;
        Debug.Log("Total dif" + totalDif);
        Debug.Log("Index" + curRoom);
        //Return the float so it can be called
        return totalDif;

        //Total their difficulty = sum of enemies spawned * enemy difficulty => Room Difficulty

    }

    private bool InteractWithNoEnemies()
    {
       
        //Get list of spawned enemies in the current room room
        bool canInteract = true;
        //Get room index of chest 
        int curRoom = gameObject.GetComponent<ChestControl>().roomIndex;
        //Get room index off enemybase
        EnemyBase[] enemyIndex = GameObject.FindObjectsOfType<EnemyBase>();
        EnemyBase enemy = null;

        Dictionary<int,List<EnemyRuntimeData>> enemiesInCurRoom = EnemyManager.Instance.spawnedEnemies;

        int enemiesInRoom = 0;
        if (enemyIndex != null && enemy == null)
        {
            for (int i = 0; i < enemyIndex.Length; i++)
            {
                if (curRoom == enemyIndex[i].roomIndex)
                {
                    enemy = enemyIndex[i];
                    enemiesInRoom++;
                }
            }
            //Set a new index from the current room index of the chest
            int newIndex = curRoom;
            //Debug.Log("Number of enemies in room" + " { " + newIndex + " } " + " is " + " { " + enemiesInRoom + " } ");
            //Return canInteract based on condition, prevent interaction while enemy count is over 0
            while (enemiesInRoom > 0)
            {
                return !canInteract;
            }

            return canInteract;
        }
        else
        {
            return canInteract;
        }
        //??Bug => Interacting with chest when no enemies are present in room
    }

    private void ChestStorage()
    {
        //Store all items & drop chances in here
        //Drop coin collectabledata type as loot from chest based in difficulty, given drop chance is higher on harder difficulties
        //https://answers.unity.com/questions/1630997/chest-items-get-added-to-the-wrong-stackchest-item.html
    }
}
