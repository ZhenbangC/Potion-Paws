using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    public string itemName;
    public Sprite icon;
    public int quantity;
    public string description;

    public InventorySlot(string name, Sprite icon, string desc)
    {
        this.itemName = name;
        this.icon = icon;
        this.quantity = 1;
        this.description = desc;
    }

    public void AddQuantity() => quantity++;
    public void RemoveQuantity() => quantity--;
}