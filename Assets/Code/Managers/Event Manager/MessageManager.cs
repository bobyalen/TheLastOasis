using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class MessageManager : MonoBehaviour
{
    public static MessageManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public TextMeshProUGUI openChestText;
    public TextMeshProUGUI interactChestText;
    public TextMeshProUGUI captainText;
    public TextMeshProUGUI chefText;
    public TextMeshProUGUI carpenterText;
    public TextMeshProUGUI shopkeeperText;
    public TextMeshProUGUI cabinBoyText;
    public TextMeshProUGUI gunnerText;
    public TextMeshProUGUI surgeonText;
    public TextMeshProUGUI qmText;
    public TextMeshProUGUI saText;
    public TextMeshProUGUI wheelText;
    public TextMeshProUGUI mastText;
    [SerializeField]
    private TextMeshProUGUI pickupHealthActivateText;
    public bool displayMessages;
    private ShopPopUp shopPop;
    private void Start()
    {
        shopPop = GetComponent<ShopPopUp>();
    }
    void Update()
    {
        if (!displayMessages)
        {
            DisableChestText();
            DisableChestInteractText();
            DisableHealthConsumableText();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log($"Coins: {Inventory.Instance.GetCoins()}");
        }
    }
    //***************** CHEST MESSAGES *****************//
    public void DisplayChestText()
    {
        openChestText.gameObject.SetActive(true); 
    }

    public void DisableChestText()
    {
        openChestText.gameObject.SetActive(false);
    }

    public void DisplayChestInteractText()
    {
        interactChestText.gameObject.SetActive(true);
    }

    public void DisableChestInteractText()
    {
        interactChestText.gameObject.SetActive(false);
    }

    //***************** HEALTH MESSAGES *****************//

    public void DisplayHealthConsumableText()
    {
        pickupHealthActivateText.gameObject.SetActive(true);
    }

    public void DisableHealthConsumableText()
    {
        pickupHealthActivateText.gameObject.SetActive(false);
    }

    //************** NPC MESSAGES ********************//
    public void DisplayCaptainText()
    {
        captainText.gameObject.SetActive(true);
    }

    public void DisableCaptainText()
    {
        captainText.gameObject.SetActive(false);
    }

    public void DisplayChefText()
    {
        
        chefText.gameObject.SetActive(true);
    }

    public void DisableChefText()
    {
        //CHECK IF IN SHOP THEN SET TRANSPARENCY OF TEXT TO 0 
        chefText.gameObject.SetActive(false);
    }
    
    public void DisplayCarpenterText()
    {
        carpenterText.gameObject.SetActive(true);
    }

    public void DisableCarpenterText()
    {
        carpenterText.gameObject.SetActive(false);
    }

    public void DisplayShopkeeperText()
    {
        shopkeeperText.gameObject.SetActive(true);
    }

    public void DisableShopkeeperText()
    {
        shopkeeperText.gameObject.SetActive(false);
    }

    public void DisplayCabinBoyText()
    {
        cabinBoyText.gameObject.SetActive(true);
    }

    public void DisableCabinBoyText()
    {
        cabinBoyText.gameObject.SetActive(false);
    }

    public void DisplayGunnerText()
    {
        gunnerText.gameObject.SetActive(true);
    }

    public void DisableGunnerText()
    {
        gunnerText.gameObject.SetActive(false);
    }

    public void DisplaySurgeonText()
    {
        surgeonText.gameObject.SetActive(true); 
    }

    public void DisableSurgeonText()
    {
        surgeonText.gameObject.SetActive(false);
    }

    public void DisplayQMText()
    {
        qmText.gameObject.SetActive(true);
    }

    public void DisableQMText()
    {
        qmText.gameObject.SetActive(false);
    }

    public void DisplaySAText()
    {
        saText.gameObject.SetActive(true);
    }

    public void DisableSAText()
    {
        saText.gameObject.SetActive(false);
    }

    public void DisplaywheelText()
    {
        wheelText.gameObject.SetActive(true);
    }

    public void DisablewheelText()
    {
        wheelText.gameObject.SetActive(false);
    }

    public void DisplayMastText()
    {
        mastText.gameObject.SetActive(true); 
    }

    public void DisableMastText()
    {
        mastText.gameObject.SetActive(false);
    }
}
