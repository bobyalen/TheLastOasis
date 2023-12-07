using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopUp : MonoBehaviour
{
    private ShopPopUp shopScript;

    private void Start()
    {
        shopScript = GetComponent<ShopPopUp>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chef"))
        {
            MessageManager.instance.DisplayChefText();
        }
        if (collision.gameObject.CompareTag("Carpenter"))
        {
            MessageManager.instance.DisplayCarpenterText();
        }
        if (collision.gameObject.CompareTag("Gunner"))
        {
            MessageManager.instance.DisplayGunnerText();
        }
        if (collision.gameObject.CompareTag("Surgeon"))
        {
            MessageManager.instance.DisplaySurgeonText();
        }
        if (collision.gameObject.CompareTag("QuarterMaster"))
        {
            MessageManager.instance.DisplayQMText();
        }
        if (collision.gameObject.CompareTag("Mast"))
        {
            MessageManager.instance.DisplayMastText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chef"))
        {
            MessageManager.instance.DisableChefText();
        }
        if (collision.gameObject.CompareTag("Carpenter"))
        {
            MessageManager.instance.DisableCarpenterText();
        }
        if (collision.gameObject.CompareTag("Gunner"))
        {
            MessageManager.instance.DisableGunnerText();
        }
        if (collision.gameObject.CompareTag("Surgeon"))
        {
            MessageManager.instance.DisableSurgeonText();
        }
        if (collision.gameObject.CompareTag("QuarterMaster"))
        {
            MessageManager.instance.DisableQMText();
        }
        if (collision.gameObject.CompareTag("Mast"))
        {
            MessageManager.instance.DisableMastText(); 
        }
    }
}
