using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractsDisplay : MonoBehaviour
{
    public ContractsShop contractsShop;

    public GameObject shopWindow;

    [Header("Connect Gameobjects")]
    public TextMeshProUGUI contractName;
    public TextMeshProUGUI contractDescription;
    public TextMeshProUGUI contractGoodStat;
    public TextMeshProUGUI contractBadStat;
    public TextMeshProUGUI contractPrice;

    public TextMeshProUGUI playerCoins;

    public Image npcImage;
    public TextMeshProUGUI npcName;

    public Button purchaseButton;

    private bool canOpen;

    public static ContractsDisplay Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void AmendContractShop()
    {
        purchaseButton.interactable = !contractsShop.ContractBought;
        contractsShop.CalculateAmountToGain();
        contractsShop.CalculateAmountToLose();

        contractName.text = contractsShop.ContractName;
        contractDescription.text = contractsShop.ContractDescription;
        contractGoodStat.text = "You will gain:\n";
        contractBadStat.text = "You will lose:\n";

        contractGoodStat.text += contractsShop.GainText();
        contractBadStat.text += contractsShop.LoseText();
        contractPrice.text = "Cost: " + contractsShop.ContractCost;
        playerCoins.text = Inventory.Instance.GetCoins().ToString();
        npcImage.sprite = contractsShop.NpcImage;
        npcName.text = contractsShop.NpcName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canOpen)
            OpenContractShop();

        if (shopWindow.activeSelf)
        {
            EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
            MessageManager.instance.chefText.alpha = 0;
            MessageManager.instance.carpenterText.alpha = 0;
            MessageManager.instance.surgeonText.alpha = 0;
            MessageManager.instance.qmText.alpha = 0;
            MessageManager.instance.gunnerText.alpha = 0;
        }
    }
    public void OpenContractShop()
    {
        AmendContractShop();
        AmendButton();
        shopWindow.SetActive(true);
    }

    public void UnfreezePlayer()
    {
        shopWindow.SetActive(false);
        EventManager.TriggerEvent(Event.DialogueFinish, null);
        MessageManager.instance.chefText.alpha = 1;
        MessageManager.instance.carpenterText.alpha = 1;
        MessageManager.instance.surgeonText.alpha = 1;
        MessageManager.instance.qmText.alpha = 1;
        MessageManager.instance.gunnerText.alpha = 1;
    }

    public void GetContractShop(GameObject go)
    {
        contractsShop = go.GetComponent<NpcShop>().Contracts;
        canOpen = true;
    }

    public void RemoveShop()
    {
        canOpen = false;
    }

    private void AmendButton()
    {
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => OnButtonClick(contractsShop));
    }

    private void OnButtonClick(ContractsShop _contractsShop)
    {
        if(Inventory.Instance.GetCoins() >= _contractsShop.ContractCost)
        {
            _contractsShop.PurchaseContract();
            purchaseButton.interactable = false;
            purchaseButton.onClick.RemoveAllListeners();
        }
    }
}
