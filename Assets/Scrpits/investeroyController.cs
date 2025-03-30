using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class investeroyController : MonoBehaviour
{
    private GridPosition gridPosition;
    public GridPosition GridPosition 
    { 
        get => gridPosition;
        set
        {
            gridPosition = value;
            InvestoryHighLight.Setparent(value);
        }
    }

    InvestoryItem SelectedItem;
    InvestoryItem overLapItem;
    RectTransform rectTransform;

    [SerializeField] List<itemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    investoryHighLight InvestoryHighLight;
    // Start is called before the first frame update
    private void Awake()

    {
        InvestoryHighLight = GetComponent<investoryHighLight>();
    }
    // Update is called once per frame
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
            InvestoryHighLight.Show(false);
            return;
        }

        //Debug.Log(gridPosition.GetGridPosition(Input.mousePosition));

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
        InvestoryItem itemToInsert = SelectedItem;
        SelectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InvestoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = gridPosition.FindSpaceForObject(itemToInsert);
        if(posOnGrid == null)
        {
            Debug.Log("NO Space");
            if (itemToInsert != null && itemToInsert.gameObject != null)
            {
                Destroy(itemToInsert.gameObject); // 销毁物品本体
            }
            return;
        }

        gridPosition.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    InvestoryItem itemToHighLighter;
    Vector2Int oldPosition;
    private void HandleHighLight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if(oldPosition == positionOnGrid) { return; }

        oldPosition = positionOnGrid;
        if (SelectedItem == null)
        {
            itemToHighLighter = gridPosition.GetItem(positionOnGrid.x, positionOnGrid.y);

            if(itemToHighLighter != null)
            {   
                InvestoryHighLight.SetSize(itemToHighLighter);
                InvestoryHighLight.SetPosition(gridPosition, itemToHighLighter);
                InvestoryHighLight.Show(true);
                //InvestoryHighLight.Setparent(gridPosition);
            }
            else
            {
                InvestoryHighLight.Show(false);
            }
        }
        else
        {
            InvestoryHighLight.Show(gridPosition.boundryCheck(positionOnGrid.x,positionOnGrid.y,SelectedItem.itemData.width,SelectedItem.itemData.height));
            InvestoryHighLight.SetSize(SelectedItem);
            InvestoryHighLight.SetPosition(gridPosition, SelectedItem, positionOnGrid.x, positionOnGrid.y);
            InvestoryHighLight.Show(true);
            //InvestoryHighLight.Setparent(gridPosition);
        }
    }

    private void creatRandomItem()
    {
        InvestoryItem item = Instantiate(itemPrefab).GetComponent<InvestoryItem>();
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
        bool complete =  gridPosition.PlaceItem(SelectedItem, tileGridPosition.x, tileGridPosition.y, ref overLapItem);
        if(complete)
        {
            SelectedItem = null;
            if(overLapItem != null)
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
