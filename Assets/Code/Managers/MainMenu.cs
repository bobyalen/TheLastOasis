using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button newGame;
    [SerializeField] Button Continue;
    [SerializeField] Button Quit;
    [SerializeField] private AudioClip clip;
    void Start()
    {
        newGame.onClick.AddListener(NewGame);
        Continue.onClick.AddListener(ContinueGame);
        Quit.onClick.AddListener(QuitGame);
        AudioManager.instance.PlayMenuMusic(clip);
    }
    private void Update()
    {
    }
    private void NewGame()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(DoSceneChange());
    }

    private void ContinueGame()
    {
        StartCoroutine(DoSceneChange());
    }
    IEnumerator DoSceneChange()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ship");
        AudioManager.instance.StopMenu();
        Destroy(AudioManager.instance);
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