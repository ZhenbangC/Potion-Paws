using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;


public class InventorySystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject ui_Window;
    public UnityEngine.UI.Image[] items_images;
    public TextMeshProUGUI[] items_counts;

    [Header("Item Description")]
    public TextMeshProUGUI description_Title;
    public TextMeshProUGUI description_Text;

    private bool isOpen;

    private List<GameObject> items = new List<GameObject>();
    private List<string> itemNames = new List<string>();
    private List<int> itemQuantities = new List<int>();

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

    public void PickUp(Item item)
    {
        string cleanName = item.itemName ;
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

    string GetCleanName(string rawName)
    {
        int parenIndex = rawName.IndexOf('（');
        return parenIndex >= 0 ? rawName.Substring(0, parenIndex) : rawName;
    }

    void Update_UI()
    {
        HideAll();

        for (int i = 0; i < items.Count && i < items_images.Length; i++)
        {
            if (items[i] != null)
            {
                items_images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
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
        description_Title.text = itemComponent.itemName;         // 使用 item.itemName，而非 itemNames[id]
        description_Text.text = itemComponent.descriptionText;
        description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }

    public void Consume(int id)
    {
        if (items[id].GetComponent<Item>().type == Item.ItemType.Consumables)
        {
            itemQuantities[id]--;

            if (itemQuantities[id] <= 0)
            {
                items[id].GetComponent<Item>().consumeEvent.Invoke();
                items.RemoveAt(id);
                itemNames.RemoveAt(id);
                itemQuantities.RemoveAt(id);
            }

            Update_UI();
        }
    }
}