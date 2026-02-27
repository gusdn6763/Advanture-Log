using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryModule
{
    public Action<Item> OnItemCountChangedAction;

    private List<Item> items;

    public void OnItemCountChanged(Item item)
    {
        //base.OnItemCountChanged(item);
        //currentWeight += Player.instance.GetWeight();
        //weightText.text = currentWeight + " / " + fullWeight;
    }

    public void ItemCommandChange(Item item)
    {
        //item.GetItem();
    }

    public int GetItemCountFromId(string id)
    {
        return 0;
    }

    public Item GeItemFromId(string id)
    {
        return items[0];
    }

    public void ThrowItem()
    {

    }
}
