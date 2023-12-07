using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{

    [SerializeField]
    private bool playerIsInRange = false;
    [SerializeField]
    PopupManager popupManager;

    private void Start()
    {
        popupManager = FindObjectOfType<PopupManager>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerIsInRange)
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
        yield return StartCoroutine(TransitionManager.Instance.FadeIn());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ship");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
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
}
