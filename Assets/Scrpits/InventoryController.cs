using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private GridPosition gridPosition;
    public GridPosition GridPosition
    {
        get => gridPosition;
        set
        {
            gridPosition = value;
            InventoryHighLight.Setparent(value);
        }
    }

    InventoryItem SelectedItem;
    InventoryItem overLapItem;
    RectTransform rectTransform;

    [SerializeField] List<itemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighLight InventoryHighLight;

    private void Awake()
    {
        InventoryHighLight = GetComponent<InventoryHighLight>();
    }

    void Update()
    {
        Debug.Log(SelectedItem);
        mousePositionGet();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            creatRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();
        }

        if (gridPosition == null)
        {
            InventoryHighLight.Show(false);
            return;
        }

        HandleHighLight();

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPress();
        }
    }

    private void InsertRandomItem()
    {
        if (gridPosition == null) return;
        creatRandomItem();
        InventoryItem itemToInsert = SelectedItem;
        SelectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = gridPosition.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null)
        {
            Debug.Log("NO Space");
            if (itemToInsert != null && itemToInsert.gameObject != null)
            {
                Destroy(itemToInsert.gameObject);
            }
            return;
        }

        gridPosition.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    InventoryItem itemToHighLighter;
    Vector2Int oldPosition;

    private void HandleHighLight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) return;

        oldPosition = positionOnGrid;
        if (SelectedItem == null)
        {
            itemToHighLighter = gridPosition.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighLighter != null)
            {
                InventoryHighLight.SetSize(itemToHighLighter);
                InventoryHighLight.SetPosition(gridPosition, itemToHighLighter);
                InventoryHighLight.Show(true);
            }
            else
            {
                InventoryHighLight.Show(false);
            }
        }
        else
        {
            InventoryHighLight.Show(gridPosition.BoundaryCheck(positionOnGrid.x, positionOnGrid.y, SelectedItem.itemData.width, SelectedItem.itemData.height));
            InventoryHighLight.SetSize(SelectedItem);
            InventoryHighLight.SetPosition(gridPosition, SelectedItem, positionOnGrid.x, positionOnGrid.y);
            InventoryHighLight.Show(true);
        }
    }

    private void creatRandomItem()
    {
        InventoryItem item = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        SelectedItem = item;

        rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        item.Set(items[selectedItemID]);
    }

    private void mousePositionGet()
    {
        if (SelectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    private void mouseDownPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (SelectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            placeItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (SelectedItem != null)
        {
            position.x -= (SelectedItem.itemData.width - 1) * GridPosition.tileSizewidth / 2;
            position.y += (SelectedItem.itemData.height - 1) * GridPosition.tileSizeheight / 2;
        }

        Vector2Int tileGridPosition = gridPosition.GetGridPosition(position);
        return tileGridPosition;
    }

    private void placeItem(Vector2Int tileGridPosition)
    {
        bool complete = gridPosition.PlaceItem(SelectedItem, tileGridPosition.x, tileGridPosition.y, ref overLapItem);
        if (complete)
        {
            SelectedItem = null;
            if (overLapItem != null)
            {
                Debug.Log("dsa");
                SelectedItem = overLapItem;
                overLapItem = null;
                rectTransform = SelectedItem.GetComponent<RectTransform>();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        SelectedItem = gridPosition.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (SelectedItem != null)
        {
            rectTransform = SelectedItem.GetComponent<RectTransform>();
        }
    }
}
