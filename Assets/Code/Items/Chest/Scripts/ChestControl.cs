using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ChestControl : MonoBehaviour
{
    public bool isOpen;
    public int roomIndex;
    //Add probability
    [SerializeField] public int maxNumberCoins;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openChest, closeChest;
    //Access enemy prefabs & number of them
    private float dropChance;
    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest Opened");
            spriteRenderer.sprite = openChest;
            //On open chest, spawn coins, display canvas ui interaction
        }
        else
        {
            //Display chest close sprite
            spriteRenderer.sprite = closeChest;
        }
    }
}
