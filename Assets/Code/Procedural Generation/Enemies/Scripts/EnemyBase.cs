using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds all the enemy's data from which all the other behaviours are derived
public class EnemyBase : MonoBehaviour
{
    public int roomIndex;
    public float currentHealth = 10;
    public float attackDamage;
    public Dictionary<Item, int> lootToDrop;
    public float multiplier;
    public RoomScript rs;
    public Enemy enemyData;
    [SerializeField]
    private GameObject _coinSpawner;
    private void Awake()
    {
        lootToDrop = new Dictionary<Item, int>();
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
        EventManager.StartListening(Event.PlayerDeath, OnPlayerDeath);
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            EventManager.TriggerEvent(Event.EnemyDestroyed, new EnemyDestroyedPacket()
            {
                go = gameObject,
                lootToDrop = lootToDrop
            });
        }
    }

    private void OnHit(IEventPacket packet)
    {
        PlayerHitPacket php = packet as PlayerHitPacket;
        if(php.enemy == this.gameObject)
        {
            currentHealth -= php.damage;
        }
        //Play enemy hit audio here
    }
    private void OnPlayerDeath(IEventPacket packet)
    {

    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            rs.enemies.Remove(this);
            Instantiate(_coinSpawner, this.transform.position, Quaternion.identity);
        }
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
        EventManager.StopListening(Event.PlayerDeath, OnPlayerDeath);
    }
}
