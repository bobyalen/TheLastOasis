using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Timeline;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    private LevelGeneration levelGeneration;
    private PlayerStats playerStats;
    private Inventory inventory;
    private ItemSpawnManager itemSpawnManager;
    private EnemyManager enemyManager;
    private PlayerController playerController;
    private PlayerStacks playerStacks;
    private TransitionManager transitionManager;
    private ShipShopDisplay shipShopDisplay;
    private ContractsDisplay contractsDisplay;

    public GameObject p_playerStats;
    public GameObject p_inventory;

    private void Awake()
    {   
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }    
        else
        {
            Instance = this;
        }
        levelGeneration = GameObject.FindObjectOfType<LevelGeneration>();
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        if(playerStats == null)
            playerStats = Instantiate(p_playerStats).GetComponent<PlayerStats>();
        inventory = GameObject.FindObjectOfType<Inventory>();
        if (inventory == null)
            inventory = Instantiate(p_inventory).GetComponent<Inventory>();
        itemSpawnManager = GameObject.FindObjectOfType<ItemSpawnManager>();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerStacks = GameObject.FindObjectOfType<PlayerStacks>();
        transitionManager = GameObject.FindObjectOfType<TransitionManager>();
        shipShopDisplay = GameObject.FindObjectOfType<ShipShopDisplay>();
        contractsDisplay = GameObject.FindObjectOfType<ContractsDisplay>();

    }

    public void Update()
    {

    }

    public static void Reset()
    {
        Instance = null;
    }

    public LevelGeneration LevelGeneration { get => levelGeneration; }
    public PlayerStats PlayerStats { get => playerStats;}
    public Inventory Inventory { get => inventory; }
    public ItemSpawnManager ItemSpawnManager { get => itemSpawnManager;}
    public EnemyManager EnemyManager { get => enemyManager; }
    public PlayerController PlayerController { get => playerController;}
    public PlayerStacks PlayerStacks { get => playerStacks;}
    public TransitionManager TransitionManager { get => transitionManager;}
    public ShipShopDisplay ShipShopDisplay { get => shipShopDisplay;}
    public ContractsDisplay ContractsDisplay { get => contractsDisplay;}
   
}
