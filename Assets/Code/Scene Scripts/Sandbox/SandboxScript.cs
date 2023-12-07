using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SandboxScript : MonoBehaviour
{
    //public GameObject enemyPrefab;
    public GameObject[] enemies;
    public RoomScript roomScript;
    public BossManager testBoss;

    [SerializeField]
    int roomIndex = 1;
    [SerializeField]
    float roomDifficulty = 1;
    [SerializeField]
    int roomDistanceToStart = 1;
    [SerializeField]
    float onCollisionDamage = 1;
    [SerializeField]
    float attackDamage = 1;
    [SerializeField]
    float multiplier = 1;
    [SerializeField]
    bool enemyDelayedSpawn = false; //press 'i' to trigger a delayed spawn

    [Header("PlayerStats Display")]
    public GameObject currentHealthtext;
    public GameObject currentSpeedtext;
    public GameObject currentDexteritytext;
    public GameObject currentDamagetext;
    public GameObject currentDefencetext;

    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<EnemyBase>() != null)
            {
                //Debug.Log(enemy);
                enemy.GetComponent<EnemyBase>().roomIndex = roomIndex;
                enemy.GetComponent<EnemyBase>().attackDamage = attackDamage;
                enemy.GetComponent<EnemyBase>().multiplier = multiplier;
                enemy.GetComponent<EnemyBase>().rs = roomScript;
                if (enemyDelayedSpawn)
                {
                    enemy.SetActive(false);
                }
            }            
        }
        roomScript.roomIndex = roomIndex;
        roomScript.roomDifficulty = roomDifficulty;
        roomScript.distToStart = roomDistanceToStart;
    }

    void updateStatDisplay()
    {
        currentHealthtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: " + PlayerStats.Instance.cachedCalculatedValues[Stat.Current_Health].ToString();
        currentSpeedtext.GetComponent<TMPro.TextMeshProUGUI>().text = "Speed: " + PlayerStats.Instance.cachedCalculatedValues[Stat.Speed].ToString();
        currentDexteritytext.GetComponent<TMPro.TextMeshProUGUI>().text = "Dexterity: " + PlayerStats.Instance.cachedCalculatedValues[Stat.Dexterity].ToString();
        currentDamagetext.GetComponent<TMPro.TextMeshProUGUI>().text = "Damage: " + PlayerStats.Instance.cachedCalculatedValues[Stat.Damage].ToString();
        currentDefencetext.GetComponent<TMPro.TextMeshProUGUI>().text = "Defense: " + PlayerStats.Instance.cachedCalculatedValues[Stat.Defence].ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBase>() != null)
                {
                    enemy.SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            roomScript.isBoss= true;
            testBoss.bossTest();
        }
        updateStatDisplay();
    }


}
