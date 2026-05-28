using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class EventHandler : MonoBehaviour
{
    public static event Action<ItemDetails, int> UpdateUIEvent;

    public static void CallUpdateUIEvent(ItemDetails itemDetails,int index)
    {
        UpdateUIEvent?.Invoke(itemDetails, index);
    }

    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static Action<ItemDetails, bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<ItemName> ItemUsedEvent;

    public static void CallItemUsedEvent(ItemName itemName)
    {
        ItemUsedEvent?.Invoke(itemName);
    }

    public static event Action<int> ChangeItmeEvent;

    public static void CallChangeItmeEvent(int index)
    {
        ChangeItmeEvent?.Invoke(index);
    }

    public static event Action<string> ShowDialogEvent;

    public static void CallShowDialogEvent(string dialog)
    {
        ShowDialogEvent?.Invoke(dialog);
    }

}
