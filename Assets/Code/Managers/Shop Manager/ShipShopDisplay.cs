using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShipShopDisplay : MonoBehaviour
{
    public GameObject shop;

    public ShipShop shipShop;

    [Header("Connect Gameobjects")]
    public Image npcSprite;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;

    public TextMeshProUGUI playerCoins;

    public Image itemSprite;
    public TextMeshProUGUI itemTitle;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemCost;
    public TextMeshProUGUI itemLevel;

    public Button purchaseButton;

    private bool canOpenShop;

    public static ShipShopDisplay Instance;

    public void Awake()
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canOpenShop)
            OpenShop();

        if(shop.activeSelf)
        {
            EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
            MessageManager.instance.chefText.alpha = 0;
            MessageManager.instance.carpenterText.alpha = 0;
            MessageManager.instance.surgeonText.alpha = 0;
            MessageManager.instance.qmText.alpha = 0;
            MessageManager.instance.gunnerText.alpha = 0;
        }
    }

    private void AmendButton()
    {
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => OnButtonClick(shipShop.ShopItem));
    }

    private void OnButtonClick(ShopItem _shopItem)
    {
        if (_shopItem.ItemLevel < 10)
        {
            if (_shopItem.CalculateItemCost() > Inventory.Instance.GetCoins())
                npcDialogue.text = "Not enough money! You are missing: " + (_shopItem.CalculateItemCost() - Inventory.Instance.GetCoins());
            else
            {
                _shopItem.PurchaseItem();
                AmendShop();
            }
        }
        else
        {
            _shopItem.PurchaseItem();
            AmendShop();
            itemLevel.text = "Max Level!";
            itemCost.text = "Max Level!";
            purchaseButton.interactable = false;
            purchaseButton.onClick.RemoveAllListeners();
        }
    }

    public void UnfreezePlayer()
    {
        shop.SetActive(false);
        EventManager.TriggerEvent(Event.DialogueFinish, null);
        MessageManager.instance.chefText.alpha = 1;
        MessageManager.instance.carpenterText.alpha = 1;
        MessageManager.instance.surgeonText.alpha = 1;
        MessageManager.instance.qmText.alpha = 1;
        MessageManager.instance.gunnerText.alpha = 1;
    }

    public void OpenShop()
    {
        if(shipShop.ShopItem.ItemLevel > 10)
        {
            shop.SetActive(true);
            AmendShop();
            purchaseButton.interactable = false;
            itemLevel.text = "Max Level!";
            itemCost.text = "Max Level!";
        }
        else
        {
            purchaseButton.interactable = true;

            shop.SetActive(true);

            AmendButton();
            AmendShop();
        }
    }

    private void AmendShop()
    {
        npcSprite.sprite = shipShop.NpcSprite;
        npcName.text = shipShop.NpcName;
        npcDialogue.text = shipShop.NpcDialogue;

        playerCoins.text = "" + Inventory.Instance.GetCoins();

        itemSprite.sprite = shipShop.ShopItem.ItemSprite;
        itemTitle.text = shipShop.ShopItem.ItemTitle;
        itemDescription.text = shipShop.ShopItem.ItemDescription;
        itemCost.text = "Cost: " + shipShop.ShopItem.CalculateItemCost();
        itemLevel.text = "Level " + shipShop.ShopItem.ItemLevel;
    }

    public void GetShop(GameObject go)
    {
        shipShop = go.GetComponent<NpcShop>().Shop;
        canOpenShop = true;
    }

    public void RemoveShop()
    {
        canOpenShop = false;
    }
}
