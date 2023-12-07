using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MastPopUp : MonoBehaviour
{
    public GameObject menuPanel;
    public KeyCode interactKey = KeyCode.E;
   //Disable player attack on clicking in menus
    public bool isInMenu;
    public bool menuActive;
    public bool playerInRange;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Button exit;

    private void Start()
    {
        exit.onClick.AddListener(UnfreezePlayer);

    }
    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && playerInRange)
        {
            //If menu panel is already active in heirarchy
            if (menuPanel.activeInHierarchy)
            {
                menuPanel.SetActive(false);
                EventManager.TriggerEvent(Event.DialogueFinish, null);
                isInMenu = false;
            }
            else
            {
                menuPanel.SetActive(true);
                EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
                isInMenu = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Set player state to idle
            playerInRange = true;
            Debug.Log("Player in range" + playerInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player not in range" + playerInRange);
        }
    }

    void UnfreezePlayer()
    {
        EventManager.TriggerEvent(Event.DialogueFinish, null);

        isInMenu = false;
    }
}
