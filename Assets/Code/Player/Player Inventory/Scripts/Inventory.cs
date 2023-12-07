using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Dictionary handles item stacking from List
    private Dictionary<CollectableData, int> itemDictionary = new Dictionary<CollectableData, int>();
    [SerializeField]
    private CollectableData coinEntry;

    public static Inventory Instance;

    private void Awake()
    {
        if(coinEntry != null)
            itemDictionary.Add(coinEntry, 0);
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnDestroy()
    {

    }


    public void Add(CollectableData collectableData, int amount)
    {
        //we handle the coin addition separately
        if (collectableData.isCoin)
        {
            itemDictionary[coinEntry] += amount;
            return;
        }
        
        if(itemDictionary.ContainsKey(collectableData))
        {
            itemDictionary[collectableData] += amount;
        }
        else
        {
            itemDictionary.Add(collectableData, amount);
        }
    }

    public void AddCoins(int amount, bool ignoreCoinGain = false)
    {
        if(!ignoreCoinGain)
            amount = (int)((PlayerStats.Instance.cachedCalculatedValues[Stat.Coin_Gain] / 100.0f) * amount);
        itemDictionary[coinEntry] += amount;
        if (itemDictionary[coinEntry] < 0)
            itemDictionary[coinEntry] = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            AddCoins(10000);
        }
    }
    public void RemoveCoins(int amount)
    {
        //if (itemDictionary[coinEntry] > amount)
        //{
        //    itemDictionary[coinEntry] = 0;
        //    return;
        //}
        itemDictionary[coinEntry] -= amount;
    }

    //Check for Collisions with enemies and remove items
    public void Remove(CollectableData collectableData, int amount)
    {
        if (itemDictionary.ContainsKey(collectableData))
        {
            if (itemDictionary[collectableData] <= amount)
            {
                itemDictionary[collectableData] = 0;
            }
            else
                itemDictionary[collectableData] -= amount;
        }
        else
        {
            Debug.LogError($"Could not find item of type {collectableData.name} in the inventory!");
            return;
        }
        ///}
    }
    public void ClearInventory()
    {   //Clear inv when player dies in dungeon, keep items at save points
        //NEEDS MAKING MORE ROBUST, CLEAR MORE THAN ONE TYPE OF COLLECTABLE


        itemDictionary.Clear();
        itemDictionary.Add(coinEntry, 0);


    }

    public int QueryInv(CollectableData collectableData)
    {
        if(itemDictionary.ContainsKey(collectableData))
        {
            return itemDictionary[collectableData];
        }
        else
        {
            return 0;
        }
    }

    public int GetCoins()
    {
        return coinEntry == null ? 0 : itemDictionary[coinEntry];
    }


    public void DisplayInv(CollectableData collectableData)
    {
        //Display in UI as Stretch goal

    }

    public bool HasCoin(CollectableData collectableData)
    {
        if (itemDictionary.ContainsKey(collectableData))
        {
            return true;
        }
        return false;
    }


}
