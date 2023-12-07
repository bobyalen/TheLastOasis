using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Shop Menu/Shop Item")]
public class ShopItem : ScriptableObject
{
    public enum ITEM_STAT
    {
        HEALTH,
        DAMAGE,
        DEFENCE,
        DEXTERITY,
        MOVEMENT_SPEED
    };

    public ITEM_STAT ItemStat;

    public float ItemBaseStatValue;
    public int ItemLevel;
    [HideInInspector] public float ItemStatValue;

    public Sprite ItemSprite;
    public string ItemTitle;
    [TextArea] public string ItemDescription;
    public int ItemCost;

    [Header("DO NOT AMEND")]
    [SerializeField] private float Level1Value;

    public float CalculateStatValue()
    {
        switch (ItemStat)
        {
            case ITEM_STAT.HEALTH:
                if (ItemLevel == 1)
                    return ItemBaseStatValue;
                else if (ItemLevel >= 2 && ItemLevel <= 10)
                {
                    ItemBaseStatValue++;
                    return ItemBaseStatValue;
                }
                break;

            case ITEM_STAT.DAMAGE:
                if (ItemLevel == 1)
                    return ItemBaseStatValue;
                else if(ItemLevel >= 2 && ItemLevel <= 10)
                {
                    ItemBaseStatValue++;
                    return ItemBaseStatValue;
                }
                break;

            case ITEM_STAT.DEFENCE:
                break;

            case ITEM_STAT.DEXTERITY:
                if (ItemLevel == 1)
                    return ItemBaseStatValue;
                else if (ItemLevel >= 2 && ItemLevel <= 10)
                {
                    ItemBaseStatValue += 0.005f;
                    return ItemBaseStatValue;
                }
                break;

            case ITEM_STAT.MOVEMENT_SPEED:
                if(ItemLevel == 1)
                {
                    return ItemBaseStatValue;
                }
                else if (ItemLevel >= 2 && ItemLevel <= 10)
                {
                    ItemBaseStatValue += 0.01f;
                    return ItemBaseStatValue;
                }
                break;
        }
        return 0;
    }

    public void PurchaseItem()
    {
        if(Inventory.Instance.GetCoins() >= ItemCost)
        {
            Inventory.Instance.RemoveCoins(ItemCost);
            switch (ItemStat)
            {
                case ITEM_STAT.HEALTH:
                    PlayerStats.Instance.baseStatValues[Stat.Health] += CalculateStatValue();
                    PlayerStats.Instance.baseStatValues[Stat.Current_Health] = PlayerStats.Instance.baseStatValues[Stat.Health];
                    PlayerStats.Instance.cachedCalculatedValues[Stat.Current_Health] = PlayerStats.Instance.baseStatValues[Stat.Current_Health];
                    PlayerStats.Instance.cachedCalculatedValues[Stat.Health] = PlayerStats.Instance.baseStatValues[Stat.Health];

                    break;

                case ITEM_STAT.DAMAGE:
                    PlayerStats.Instance.baseStatValues[Stat.Damage] += CalculateStatValue();
                    PlayerStats.Instance.cachedCalculatedValues[Stat.Damage] = PlayerStats.Instance.baseStatValues[Stat.Damage];
                    break;

                case ITEM_STAT.DEFENCE:
                    // TO BE COMPLETED
                    break;

                case ITEM_STAT.DEXTERITY:
                    PlayerStats.Instance.baseStatValues[Stat.Dexterity] += CalculateStatValue();
                    PlayerStats.Instance.cachedCalculatedValues[Stat.Dexterity] = PlayerStats.Instance.baseStatValues[Stat.Dexterity];
                    break;

                case ITEM_STAT.MOVEMENT_SPEED:
                    PlayerStats.Instance.baseStatValues[Stat.Speed] += CalculateStatValue();
                    PlayerStats.Instance.cachedCalculatedValues[Stat.Speed] = PlayerStats.Instance.baseStatValues[Stat.Speed];
                    break;
            }
            ItemLevel++;
        }
    }

    public int CalculateItemCost()
    {
        ItemCost = ItemLevel * 100;
        return ItemCost;
    }
}
