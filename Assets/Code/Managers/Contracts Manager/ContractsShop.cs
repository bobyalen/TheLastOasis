using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contract", menuName = "Shop Menu/Contracts")]
public class ContractsShop : ScriptableObject
{
    public enum GAIN_STAT
    {
        Health,
        Damage,
        Defence,
        Dexterity,
        Speed
    }
    public List<GAIN_STAT> GainList;
    public enum LOSE_STAT
    {
        Health,
        Damage,
        Defence,
        Dexterity,
        Speed
    }
    public List<LOSE_STAT> LostList;

    public int ContractCost;
    public string ContractName;
    [TextArea] public string ContractDescription;
    public List<float> AmountToGainList;
    public List<float> AmountToLoseList;
    public Sprite NpcImage;
    public string NpcName;

    public bool ContractBought;

    public void PurchaseContract()
    {
        for(int i = 0; i < GainList.Count; i++)
        {
            //switch(GainList[i])
            //{
            //    case GAIN_STAT.Health:
            //        PlayerStats.Instance.maxHealth += 20;
            //        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
            //        break;
            //    case GAIN_STAT.Damage:
            //        PlayerStats.Instance.maxDamage += 6;
            //        PlayerStats.Instance.currentDamage = PlayerStats.Instance.maxDamage;
            //        break;
            //    case GAIN_STAT.Defence:
            //        break;
            //    case GAIN_STAT.Dexterity:
            //        break;
            //    case GAIN_STAT.Speed:
            //        PlayerStats.Instance.maxSpeed += 0.5f;
            //        PlayerStats.Instance.currentSpeed = PlayerStats.Instance.maxSpeed;
            //        break;
            //}
        }

        for (int i = 0; i < LostList.Count; i++)
        {
            //switch (LostList[i])
            //{
            //    case LOSE_STAT.Health:
            //        break;
            //    case LOSE_STAT.Damage:
            //        PlayerStats.Instance.maxDamage -= 3;
            //        PlayerStats.Instance.currentDamage = PlayerStats.Instance.maxDamage;
            //        break;
            //    case LOSE_STAT.Defence:
            //        break;
            //    case LOSE_STAT.Dexterity:
            //        break;
            //    case LOSE_STAT.Speed:
            //        PlayerStats.Instance.maxSpeed -= 0.7f;
            //        PlayerStats.Instance.currentSpeed = PlayerStats.Instance.maxSpeed;
            //        break;
            //}
        }

        Inventory.Instance.RemoveCoins(ContractCost);
        ContractBought = true;
    }

    public string GainText()
    {
        string gainString = string.Empty;

        for (int i = 0; i < AmountToGainList.Count; i++)
        {
            gainString += "+" + AmountToGainList[i].ToString("F1") + " " + GainList[i].ToString() + "\n";
        }

        return gainString;
    }
    public string LoseText()
    {
        string loseString = string.Empty;

        for (int i = 0; i < AmountToLoseList.Count; i++)
        {
            loseString += AmountToLoseList[i].ToString("F1") + " " + LostList[i].ToString() + "\n";
        }

        return loseString;
    }

    public void CalculateAmountToGain()
    {
        AmountToGainList.Clear();

        for (int i = 0; i < GainList.Count; i++)
        {
            switch (GainList[i])
            {
                case GAIN_STAT.Health:
                    AmountToGainList.Add(20);
                    break;
                case GAIN_STAT.Damage:
                    AmountToGainList.Add(6);
                    break;
                case GAIN_STAT.Defence:
                    break;
                case GAIN_STAT.Dexterity:
                    break;
                case GAIN_STAT.Speed:
                    AmountToGainList.Add(0.5f);
                    break;
            }
        }
    }

    public void CalculateAmountToLose()
    {
        AmountToLoseList.Clear();

        for (int i = 0; i < LostList.Count; i++)
        {
            switch (LostList[i])
            {
                case LOSE_STAT.Health:
                    break;
                case LOSE_STAT.Damage:
                    AmountToLoseList.Add(3);
                    break;
                case LOSE_STAT.Defence:
                    break;
                case LOSE_STAT.Dexterity:
                    break;
                case LOSE_STAT.Speed:
                    AmountToLoseList.Add(0.7f);
                    break;
            }
        }
    }
}
