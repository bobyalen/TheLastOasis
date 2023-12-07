using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{

    [SerializeField] Button Mainbtn;
    [SerializeField] Button Quit;

    void Start()
    {
        Mainbtn.onClick.AddListener(MainMenu);
        Quit.onClick.AddListener(QuitGame);
    }
    public void MainMenu()
    {
        StartCoroutine(DoSceneChange());
    }

    IEnumerator DoSceneChange()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void QuitGame()
    {
        Debug.Log("Game Quit(Doesn't work on editor)");
        Application.Quit();
    }
}
