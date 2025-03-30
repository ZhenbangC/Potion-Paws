using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public const float tileSizewidth = 30;
    public const float tileSizeheight = 30;

    InvestoryItem[,] investoryItemSlot;

    [SerializeField] int gridSizeWidth=10;
    [SerializeField] int gridSizeHeight=10;

    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);


    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    private void Init(int width,int height)
    {
        investoryItemSlot = new InvestoryItem[width,height];
        Vector2 size = new Vector2(width*tileSizeheight,height*tileSizeheight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetGridPosition(Vector2 mousPosition)
    {
        positionOnTheGrid.x = mousPosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousPosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizewidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeheight);

        return tileGridPosition;
    }

    public bool PlaceItem(InvestoryItem investoryItem, int posX, int posY, ref InvestoryItem overLapItem)
    {
        if (boundryCheck(posX, posY, investoryItem.itemData.width, investoryItem.itemData.height) == false)
            return false;

        if (overLapCheck(posX, posY, investoryItem.itemData.width, investoryItem.itemData.height, ref overLapItem) == false)
        {
            //Debug.Log("lap");
            overLapItem = null;
            return false;
        }

        if (overLapItem != null)
        {
            cleanGrid(overLapItem);
        }

        PlaceItem(investoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InvestoryItem investoryItem, int posX, int posY)
    {
        RectTransform rectTransform = investoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < investoryItem.itemData.width; x++)
        {
            for (int y = 0; y < investoryItem.itemData.height; y++)
            {
                investoryItemSlot[posX + x, posY + y] = investoryItem;
            }
        }

        investoryItem.onGridPositionX = posX;
        investoryItem.onGridPositionY = posY;
        Vector2 position = CalculatePosOnGrid(investoryItem, posX, posY);
        //position.x = posX * tileSizewidth ;
        //position.y = -(posY * tileSizeheight);
        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePosOnGrid(InvestoryItem investoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizewidth + tileSizewidth * investoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeheight + tileSizeheight * investoryItem.itemData.height / 2);
        return position;
    }

    private bool overLapCheck(int posX, int posY, int width, int height, ref InvestoryItem overLapItem)
    {
        //Debug.Log(posX); Debug.Log(posY); Debug.Log(width); Debug.Log(height);
        for (int x =0;x<width;x++)
        {
            for (int y =0;y<height;y++)
            {
                if (investoryItemSlot[posX+x,posY+y]!=null)
                {
                    if (overLapItem == null)
                    {
                        overLapItem = investoryItemSlot[posX+x,posY+y];
                       // Debug.Log(overLapItem);
                    }

                    else
                    {
                        if (overLapItem != investoryItemSlot[posX + x, posY + y])
                            return false;
                    }
                }
                
            }
        }

        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        //Debug.Log(posX); Debug.Log(posY); Debug.Log(width); Debug.Log(height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (investoryItemSlot[posX + x, posY + y] != null)
                {
                            return false;
                }

            }
        }

        return true;
    }

    public InvestoryItem PickUpItem(int x , int y)
    {
        InvestoryItem toReturn = investoryItemSlot[x, y];

        if (toReturn == null) return null;

        cleanGrid(toReturn);

        return toReturn;
    }

    private void cleanGrid(InvestoryItem toReturn)
    {
        for (int ix = 0; ix < toReturn.itemData.width; ix++)
        {
            for (int iy = 0; iy < toReturn.itemData.height; iy++)
            {
                investoryItemSlot[toReturn.onGridPositionX + ix, toReturn.onGridPositionY + iy] = null;
            }
        }
    }

    bool positionCheck(int posX, int posY)
    {
        if(posX < 0 || posY<0) return false;

        if (posX>=gridSizeWidth|| posY>=gridSizeHeight) return false;

        return true;
    }

    public bool boundryCheck(int posX,int posY,int width,int height)
    {
        if (positionCheck(posX, posY) == false) return false;
        posX += width-1; posY += height-1;
        if (positionCheck(posX, posY) == false) return false;

        return true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    internal InvestoryItem GetItem(int x, int y)
    {
        return investoryItemSlot[x,y];
    }

    public Vector2Int? FindSpaceForObject(InvestoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.itemData.height +1;
        int width = gridSizeWidth - itemToInsert.itemData.width+1;

        for(int y=0;y<height;y++)
        {
            for(int x=0;x<width;x++)
            {
                if(CheckAvailableSpace(x,y,itemToInsert.itemData.width,itemToInsert.itemData.height) == true)
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return null;
    }
}
