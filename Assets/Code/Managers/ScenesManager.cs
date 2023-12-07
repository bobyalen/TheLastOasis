using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;

    private void Awake()
    {
        instance = this;
    }

    //List of constants, index number for scene builds order
    public enum Scene
    {
        MainMenu,
        Ship,
        StartTestScene,
        SampleScene,
        StatFlow,
        EndScreen
    } 

    public void LoadScene(Scene scene)
    {
        //Reads enum as string
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadNewGame()
    {
        //Loads the scene for entrance to the island dungeon
        SceneManager.LoadScene(Scene.StartTestScene.ToString()); 
    }

    public void LoadNextScene()
    {
        //Gets current active scene in build index and loads next one in list
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadShipHub()
    {
        SceneManager.LoadScene(Scene.Ship.ToString());

    }

    public void LoadStatTest()
    {
        SceneManager.LoadScene(Scene.StatFlow.ToString());

    }

    public void LoadMainScreen()
    {
        SceneManager.LoadScene(Scene.MainMenu.ToString());

    }public void LoadEndScreen()
    {
        SceneManager.LoadScene(Scene.EndScreen.ToString());

    }

    //Open canvas UI and disable player
}
