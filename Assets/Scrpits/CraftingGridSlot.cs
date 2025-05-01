using UnityEngine;
using UnityEngine.UI;

public class CraftingGridSlot : MonoBehaviour
{
    public UnityEngine.UI.Image icon;  
    public Item storedItem;

    public void SetItem(Item item)
    {
        storedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        storedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public bool HasItem(string itemName, int count = 1)
    {
        return storedItem != null && storedItem.itemName == itemName;
    }
}