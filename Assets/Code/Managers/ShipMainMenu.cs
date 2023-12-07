using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMainMenu : MonoBehaviour
{
    [SerializeField] Button teleportIsland;
    [SerializeField] Button teleportDungeon;

    void Start()
    {
        teleportIsland.onClick.AddListener(IslandStart);
        teleportDungeon.onClick.AddListener(DungeonStart);
    }

    private void IslandStart()
    {
        ScenesManager.instance.LoadNewGame();
    }

    private void DungeonStart()
    {
        ScenesManager.instance.LoadScene(ScenesManager.Scene.SampleScene);
    }
}
