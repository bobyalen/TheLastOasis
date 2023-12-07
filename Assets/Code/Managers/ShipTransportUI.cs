using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipTransportUI : MonoBehaviour
{
    [SerializeField] Button teleportShip;
    [SerializeField] Button teleportStats;
    void Start()
    {
        teleportShip.onClick.AddListener(GoBackToShip);
        teleportStats.onClick.AddListener(GoBackToStats);
    }
    private void GoBackToShip()
    {
        ScenesManager.instance.LoadShipHub();
    }

    private void GoBackToStats()
    {
        ScenesManager.instance.LoadStatTest();

    }
}
