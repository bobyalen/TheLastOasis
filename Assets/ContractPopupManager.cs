using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractPopupManager : MonoBehaviour
{
    public static ContractPopupManager Instance { get; private set; }
    [SerializeField] private Button contract1Btn;
    [SerializeField] private Button contract2Btn;
    [SerializeField] private Button contract3Btn;
    [SerializeField] private Button contractExitBtn;
    public List<Contract> contracts = new List<Contract>();
    public Contract contract1;
    public Contract contract2;
    public Contract contract3;

    //for keyboard UI movement
    RectTransform location;
    

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
    }


    public void SpawnContractPopup(Action select1, Action select2, Action select3, Action exit)
    {
        gameObject.SetActive(true);
        contract1Btn.Select();
        contract1Btn.onClick.AddListener(() =>
        {
            Hide();
            select1();
        });
        contract2Btn.onClick.AddListener(() =>
        {
            Hide();
            select2();
        });
        contract3Btn.onClick.AddListener(() =>
        {
            Hide();
            select3();
        });
        contractExitBtn.onClick.AddListener(() =>
        {
            Hide();
            exit();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
