using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumeable : MonoBehaviour
{
    [SerializeField] ConsumeableData data;
    public float health = 3.0f;
    //private MessageManager messages;

    void Start()
    {
       // messages = GetComponent<MessageManager>();

    }

    public void SetConsumeableData(ConsumeableData data)
    {
        this.data = data;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            //ADD CALCULATION BY MODIFIER
            PlayerStats.Instance.HealPlayer(health);
            //PlayerStats.Instance.cachedCalculatedValues[Stat.Current_Health] += health;
            //PlayerStats.Instance.currentHealth += (int)health;
        }
    }

}
