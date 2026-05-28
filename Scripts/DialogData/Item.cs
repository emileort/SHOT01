using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Item : MonoBehaviour
{
    public ItemName itemName;
    public void ItemClicked()
    {
        this.gameObject.SetActive(false);
        // InventoryManager.Instance.AddItem(itemName);
    }

}
