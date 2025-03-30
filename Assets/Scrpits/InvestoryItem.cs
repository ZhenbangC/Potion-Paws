using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestoryItem : MonoBehaviour
{
    public itemData itemData;

    public int onGridPositionX;
    public int onGridPositionY;

    internal void Set(itemData item)
    {
        this.itemData = item;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * GridPosition.tileSizewidth;
        size.y = itemData.height * GridPosition.tileSizeheight;
        GetComponent<RectTransform>().sizeDelta = size;
    }    
}
