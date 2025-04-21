using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public const float tileSizewidth = 30;
    public const float tileSizeheight = 30;

    InventoryItem[,] inventoryItemSlot;

    [SerializeField] int gridSizeWidth = 5;
    [SerializeField] int gridSizeHeight = 5;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeheight, height * tileSizeheight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizewidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeheight);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overLapItem)
    {
        if (!BoundaryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height))
            return false;

        if (!OverlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overLapItem))
        {
            overLapItem = null;
            return false;
        }

        if (overLapItem != null)
        {
            CleanGrid(overLapItem);
        }

        PlaceItem(inventoryItem, posX, posY);
        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform itemRect = inventoryItem.GetComponent<RectTransform>();
        itemRect.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for (int y = 0; y < inventoryItem.itemData.height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePosOnGrid(inventoryItem, posX, posY);
        itemRect.localPosition = position;
    }

    public Vector2 CalculatePosOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizewidth + tileSizewidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeheight + tileSizeheight * inventoryItem.itemData.height / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overLapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                InventoryItem item = inventoryItemSlot[posX + x, posY + y];
                if (item != null)
                {
                    if (overLapItem == null)
                    {
                        overLapItem = item;
                    }
                    else if (overLapItem != item)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];
        if (toReturn == null) return null;

        CleanGrid(toReturn);
        return toReturn;
    }

    private void CleanGrid(InventoryItem toReturn)
    {
        for (int ix = 0; ix < toReturn.itemData.width; ix++)
        {
            for (int iy = 0; iy < toReturn.itemData.height; iy++)
            {
                inventoryItemSlot[toReturn.onGridPositionX + ix, toReturn.onGridPositionY + iy] = null;
            }
        }
    }

    bool PositionCheck(int posX, int posY)
    {
        return !(posX < 0 || posY < 0 || posX >= gridSizeWidth || posY >= gridSizeHeight);
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (!PositionCheck(posX, posY)) return false;
        posX += width - 1;
        posY += height - 1;
        return PositionCheck(posX, posY);
    }

    void Update()
    {
        
    }

    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.itemData.height + 1;
        int width = gridSizeWidth - itemToInsert.itemData.width + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.itemData.width, itemToInsert.itemData.height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
}