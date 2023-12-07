using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    [SerializeField] private BoxCollider2D roomTrigger;
    [SerializeField] private GameObject bossPrefab;
    
    //Scriptable Object
    public Enemy boss;
    public float currentHealth;
    public GameObject bossHPSlider;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool hasSpawned = false;
    private bool isBossRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Event.EnemyDestroyed, OnEnemyDestroyed);

        if (gameObject.name == "BossRoom")
        {
            currentHealth = boss.MaxHealth;
            isBossRoom = true;
            //bossHPSlider = GameObject.Find("BossHP");
            //bossHPSlider.SetActive(false);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void bossTest()
    {
        if (!hasSpawned)
        {
            hasSpawned = true;
            SpawnBoss();
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyDestroyed, OnEnemyDestroyed);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isBossRoom)
        {
            if (roomTrigger != null)
            {
                if (!hasSpawned)
                {
                    hasSpawned = true;
                    SpawnBoss();
                }
            }
        }
    }
    

    private void OnEnemyDestroyed(IEventPacket packet)
    {
        EnemyDestroyedPacket edp = packet as EnemyDestroyedPacket;
        spawnedEnemies.Remove(edp.go);
        Destroy(edp.go);
    }

    private void SpawnBoss()
    {
        GameObject go = Instantiate(bossPrefab, transform.position, Quaternion.identity);

        bossHPSlider.SetActive(true);
    }

    public void BossHP()
    {
        if(bossHPSlider.activeSelf)
            bossHPSlider.GetComponent<Slider>().value = currentHealth;
    }
}
