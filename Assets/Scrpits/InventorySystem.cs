using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DefaultItem
{
    public string itemName;
    public int quantity;
}

public class InventorySystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject ui_Window;
    public Image[] items_images;
    public TextMeshProUGUI[] items_counts;

    [Header("Item Description")]
    public TextMeshProUGUI description_Title;
    public TextMeshProUGUI description_Text;

    [Header("默认初始物品")]
    public List<DefaultItem> defaultItems = new List<DefaultItem>();

    private bool isOpen;

    private List<GameObject> items = new List<GameObject>();
    private List<string> itemNames = new List<string>();
    private List<int> itemQuantities = new List<int>();

    private void Start()
    {
        // 加载默认道具
        foreach (var entry in defaultItems)
        {
            AddItemByName(entry.itemName, entry.quantity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);
        if (isOpen) Update_UI();
    }

    public Dictionary<string, int> GetAllItems()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        for (int i = 0; i < itemNames.Count; i++)
        {
            result[itemNames[i]] = itemQuantities[i];
        }
        return result;
    }


    public void PickUp(Item item)
    {
        string cleanName = item.itemName;
        int index = itemNames.IndexOf(cleanName);

        if (index >= 0)
        {
            itemQuantities[index]++;
        }
        else
        {
            items.Add(item.gameObject);
            itemNames.Add(cleanName);
            itemQuantities.Add(1);
        }

        Update_UI();
    }

    public void AddItemByName(string itemName, int amount)
    {
        int index = itemNames.IndexOf(itemName);
        if (index >= 0)
        {
            itemQuantities[index] += amount;
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>($"Items/{itemName}");
            if (prefab != null)
            {
                GameObject newItem = Instantiate(prefab);
                items.Add(newItem);
                itemNames.Add(itemName);
                itemQuantities.Add(amount);
            }
            else
            {
                Debug.LogWarning("未在 Resources/Items 下找到：" + itemName);
            }
        }

        Update_UI();
    }

    public void Consume(int id)
    {
        if (items[id].GetComponent<Item>().type == Item.ItemType.Consumables)
        {
            itemQuantities[id]--;

            if (itemQuantities[id] <= 0)
            {
                items[id].GetComponent<Item>().consumeEvent.Invoke();
                Destroy(items[id]);
                items.RemoveAt(id);
                itemNames.RemoveAt(id);
                itemQuantities.RemoveAt(id);
            }

            Update_UI();
        }
    }

    void Update_UI()
    {
        HideAll();

        for (int i = 0; i < items.Count && i < items_images.Length; i++)
        {
            if (items[i] != null)
            {
                SpriteRenderer renderer = items[i].GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    items_images[i].sprite = renderer.sprite;
                }

                items_images[i].gameObject.SetActive(true);

                if (itemQuantities[i] > 1)
                {
                    items_counts[i].text = itemQuantities[i].ToString();
                    items_counts[i].gameObject.SetActive(true);
                }
                else
                {
                    items_counts[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void HideAll()
    {
        for (int i = 0; i < items_images.Length; i++)
        {
            items_images[i].gameObject.SetActive(false);
            items_counts[i].gameObject.SetActive(false);
        }

        HideDescription();
    }

    public void ShowDescription(int id)
    {
        Item itemComponent = items[id].GetComponent<Item>();
        description_Title.text = itemComponent.itemName;
        description_Text.text = itemComponent.descriptionText;
        description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }
}