using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHealth : MonoBehaviour
{
    public Button buyHealth;
    public Button buyHealth2;
    void Start()
    {
        buyHealth.onClick.AddListener(BuyLvl1);
        buyHealth2.onClick.AddListener(BuyLvl2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void BuyLvl1()
    {
    }

    private void BuyLvl2()
    {
    }
}
