using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class ItemToolTip : MonoBehaviour
{
    public Text itemNameText;

    public void UpdateItmeName(ItemName itemName)
    {
        itemNameText.text = itemName switch
        {
            ItemName.key => "¤M¤l",
            ItemName.Ticket=>"Å@¥Ò",
        };
    }
}
