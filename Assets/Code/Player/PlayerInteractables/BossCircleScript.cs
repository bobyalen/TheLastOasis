using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircleScript : MonoBehaviour
{
    // Start is called before the first frame update

    bool playerInRange = false;
    [SerializeField]
    private GameObject KeyToShow;
    [SerializeField]
    private GameObject[] runes;
    bool isTransitionRunning = false;
    void Start()
    {
        KeyToShow.SetActive(false);
        foreach(GameObject go in runes) 
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            if(playerInRange)
            {
                KeyToShow.SetActive(false);
                EventManager.TriggerEvent(Event.BossRoomTransitionStarted, null);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            KeyToShow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            KeyToShow.SetActive(false);
        }
    }

    public IEnumerator LightUpRunes(float delay)
    {
        foreach(var rune in runes)
        {
            yield return new WaitForSeconds(delay);
            rune.SetActive(true);
        }
    }
}
