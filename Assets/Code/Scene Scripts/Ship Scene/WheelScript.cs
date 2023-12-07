using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WheelScript : MonoBehaviour
{
    [SerializeField]
    private bool playerIsInRange = false;
    ShipPlayerController controller = null;
    [SerializeField]
    PopupManager popupManager;
    [SerializeField]
    TransitionManager transitionManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(playerIsInRange)
            {
                EventManager.TriggerEvent(Event.DialogueStart, null);
                popupManager.SpawnPopup(() =>
                {
                    StartCoroutine(DoSceneChange());
                }, () =>
                {
                    EventManager.TriggerEvent(Event.DialogueFinish, null);
                });
            }
        }
    }
    IEnumerator DoSceneChange()
    {
        Inventory.Instance.ClearInventory();
        yield return StartCoroutine(TransitionManager.Instance.FadeIn());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartTestScene");
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            playerIsInRange = true;
            controller = collision.gameObject.GetComponent<ShipPlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerIsInRange = false;
            controller = null;
        }
    }
}
