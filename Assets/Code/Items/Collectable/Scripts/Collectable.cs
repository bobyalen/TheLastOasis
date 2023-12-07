using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private CollectableData data;
    public int stackSize;                                      //How many of current item stored in inventory


    private void OnValidate()
    {
        //GetComponent<SpriteRenderer>().sprite = data.Sprite;
    }

    void Update()
    {

    }
    
    public void SetCollectableData(CollectableData data)
    {
        this.data = data;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory.Instance.Add(data, stackSize);
            Destroy(gameObject);                   
        }
    }

    public void AddToStack()
    {

        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void Consumeable()
    {
        Destroy(gameObject);
    }
}

