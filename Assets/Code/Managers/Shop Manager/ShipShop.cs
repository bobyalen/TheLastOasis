using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Shop Menu/Shop Npc")]
public class ShipShop : ScriptableObject
{
    public Sprite NpcSprite;
    public string NpcName;
    [TextArea] public string NpcDialogue;
    public ShopItem ShopItem;
}
