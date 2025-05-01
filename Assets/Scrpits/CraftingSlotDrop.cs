using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlotDrop : MonoBehaviour, IDropHandler
{
    public GridPosition targetGrid;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem dragItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (dragItem != null)
        {
            InventoryItem item = dragItem.inventoryItem;
            Vector2 localPos = transform.InverseTransformPoint(eventData.position);
            Vector2Int gridPos = targetGrid.GetGridPosition(eventData.position);

            InventoryItem overlap = null;
            if (targetGrid.PlaceItem(item, gridPos.x, gridPos.y, ref overlap))
            {
                // ≥…π¶∑≈÷√
            }
        }
    }
}