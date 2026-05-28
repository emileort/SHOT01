using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;
    public bool isDone;

    protected virtual void OnClickedAction()
    {

    }

    public virtual void EmptyClicked()
    {

    }

    public void CheckItem(ItemName itemName)
    {
        if (itemName == requireItem && !isDone)
        {
            isDone = true;

            OnClickedAction();
            EventHandler.CallItemUsedEvent(itemName);
        }
    }
}
