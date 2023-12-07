using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ContractInfoScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contractName;
    [SerializeField] TextMeshProUGUI contractGains;
    [SerializeField] TextMeshProUGUI contractLosses;
    [SerializeField] Image contractIcon;
    [SerializeField] int ContractUIposition;
    public Contract selectedContract;

    ContractPopupManager contractPopupManager;

    void Start()
    {
        contractPopupManager = FindObjectOfType<ContractPopupManager>(true);
    }

    void Update()
    {
        if (ContractUIposition == 1)
        {
            selectedContract = contractPopupManager.contract1;
        }

        if (ContractUIposition == 2)
        {
            selectedContract = contractPopupManager.contract2;
        }

        if (ContractUIposition == 3)
        {
            selectedContract = contractPopupManager.contract3;
        }

        FillContractDetails();
    }

    void FillContractDetails()
    {
        contractName.text = selectedContract.contractName;
        contractGains.text = selectedContract.contractGains;
        contractLosses.text = selectedContract.contractLosses;
        //contractIcon.sprite = selectedContract.contractIcon; //NOT IMPLEMENTED
    }
}
