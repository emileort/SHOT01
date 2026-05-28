using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemName> itemList = new List<ItemName>();

    public ItemDetails itemData;

    private void OnStartNewGameEvent(int gameWeek)
    {
        itemList.Clear();
    }
    
    private void OnItemUsedEvent(ItemName itemName)
    {
        var index = GetItemIndex(itemName);
        itemList.RemoveAt(index);

        if (itemList.Count == 0)
            EventHandler.CallUpdateUIEvent(null, -1);
    }

    private void OnAfterSceneLoadedEvent()
    {
        if(itemList.Count==0)
            EventHandler.CallUpdateUIEvent(null, -1);
        else
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemList[i]),i);
            }
        }
    }

    private void OnChangeItemEvent(int index)
    {
        if (index >= 0 && index < itemList.Count)
        {
            ItemDetails item = itemData.GetItemDetails(itemList[index]);
            EventHandler.CallUpdateUIEvent(item, index);
        }
        else
        {
            Debug.Log("¶W¹L");
        }
    }

    public void AddItem(ItemName itemName)
    {
        if (!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
        }
        EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemName), itemList.Count - 1);
    }

    private int GetItemIndex(ItemName itemName)
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            if (itemName.Equals(itemList[i]))
                return i;
        }
        return -1;
    }
}
