using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemDrop 
{
    public Item itemType;
    public int minItemQuantity;
    public int maxItemQuantity;
    public float dropProbability;
}
