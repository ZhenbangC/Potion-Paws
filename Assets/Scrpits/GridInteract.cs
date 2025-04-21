using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridPosition))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;
    GridPosition gridPosition;

    private void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        inventoryController.GridPosition = gridPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        inventoryController.GridPosition = null;
    }
}