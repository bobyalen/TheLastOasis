using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Transactions;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContractMenu : MonoBehaviour
{
    [SerializeField] private bool playerIsInRange = false;
    [SerializeField] ContractPopupManager contractPopupManager;
    [SerializeField] SpriteRenderer shrineSprite;
    GameObject contractPopup;   
    private bool contractAvailable = true;
    Contract contract1;
    Contract contract2;
    Contract contract3;
    int contractIndex1;
    int contractIndex2;
    int contractIndex3;

    private void Start()
    {
        contractPopupManager = FindObjectOfType<ContractPopupManager>(true);
        refreshMenu(); 
    }

    void Update()
    {
        if (playerIsInRange && contractAvailable && Input.GetKeyDown(KeyCode.E))
        {
            EventManager.TriggerEvent(Event.DialogueStart, null);
            contractPopupManager.SpawnContractPopup(() =>
            {
                if (contractAvailable) //bugfix for subsequent shrines giving you the contract you clicked but also the one in the position of the last ones you took
                {
                    //contract 1
                    foreach (var stacks in contract1.stacksToAdd)
                    {
                        EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket() //trigger stack added event for each stack in contract
                        {
                            stackNumber = stacks.stacksToAdd,
                            stackToAdd = stacks.stack,
                            currentRoom = LevelGeneration.Instance.roomIndex
                        });
                    }
                    contractPopupManager.contracts.RemoveAt(contractIndex1);
                    shrineDeactivate(); //deactivate shrine
                }

            }, () =>
            {
                if (contractAvailable)
                {
                    //contract 2
                    foreach (var stacks in contract2.stacksToAdd)
                    {
                        EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                        {
                            stackNumber = stacks.stacksToAdd,
                            stackToAdd = stacks.stack,
                            currentRoom = LevelGeneration.Instance.roomIndex
                        });
                    }
                    contractPopupManager.contracts.RemoveAt(contractIndex2);
                    shrineDeactivate();
                }

            }, () =>
            {
                if (contractAvailable)
                {
                    //contract3
                    foreach (var stacks in contract3.stacksToAdd)
                    {
                        EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                        {
                            stackNumber = stacks.stacksToAdd,
                            stackToAdd = stacks.stack,
                            currentRoom = LevelGeneration.Instance.roomIndex
                        });
                    }
                    contractPopupManager.contracts.RemoveAt(contractIndex3);
                    shrineDeactivate();
                }

            }, () =>
            {
                //exit
                shrineDeactivate();
            });
        }
    }

    private void shrineDeactivate() //make player unable to activate shrine again, set colour to grey, trigger dialogue finish event
    {
        shrineSprite.color = Color.grey; 
        EventManager.TriggerEvent(Event.DialogueFinish, null);
        contractAvailable = false;
        //refreshMenu();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsInRange = false;
        }
    }

    private void refreshMenu()
    {
        contractIndex1 = Random.Range(0, contractPopupManager.contracts.Count); //get random index in contract list
        contract1 = contractPopupManager.contracts[contractIndex1]; //set it as contract1
        contractPopupManager.contract1 = contract1; //public variable for contract info script to access

        do {
            contractIndex2 = Random.Range(0, contractPopupManager.contracts.Count);
        } while (contractIndex2 == contractIndex1); //check for duplicates
        contract2 = contractPopupManager.contracts[contractIndex2];
        contractPopupManager.contract2 = contract2;

        do {
            contractIndex3 = Random.Range(0, contractPopupManager.contracts.Count);
        } while (contractIndex3 == contractIndex1 || contractIndex3 == contractIndex2);
        contract3 = contractPopupManager.contracts[contractIndex3];
        contractPopupManager.contract3 = contract3;
    }
}
