using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DoTransition());
        }
    }

    IEnumerator DoTransition()
    {
        EventManager.TriggerEvent(Event.DialogueStart, null);
        yield return StartCoroutine(TransitionManager.Instance.FadeIn());
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("SampleScene");
        while(!sceneLoad.isDone)
        {
            yield return null;
        }

    }
}
