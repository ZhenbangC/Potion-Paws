using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighLight : MonoBehaviour
{
    [SerializeField] RectTransform hightLighter;

    public void Show(bool b)
    {
        hightLighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * GridPosition.tileSizewidth;
        size.y = targetItem.itemData.height * GridPosition.tileSizeheight;
        hightLighter.sizeDelta = size;
    }

    public void SetPosition(GridPosition targetGrid, InventoryItem targetItem)
    {
        hightLighter.SetAsLastSibling();
        Vector2 pos = targetGrid.CalculatePosOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        hightLighter.localPosition = pos;
    }

    public void Setparent(GridPosition targetGrid)
    {
        if (targetGrid == null) return;
        hightLighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(GridPosition targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        hightLighter.SetAsFirstSibling();

        Vector2 pos = targetGrid.CalculatePosOnGrid(targetItem, posX, posY);

        hightLighter.localPosition = pos;
    }
}