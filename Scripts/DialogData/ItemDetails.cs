using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "ItemDataList",menuName ="Inventory/ItemDataList")]

[System.Serializable]
public class ItemDetails : ScriptableObject
{
    public ItemName itemName;
    public Sprite itemSprite;
    public List<ItemDetails> itemDetailsList = new List<ItemDetails>();
    public ItemDetails GetItemDetails(ItemName itemName)
    {
        return itemDetailsList.Find(i => i.itemName.Equals(itemName));
    }
}
