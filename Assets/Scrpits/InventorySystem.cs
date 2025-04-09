using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ÒýÈë TextMeshPro

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    public List<GameObject> items = new List<GameObject>();
    public bool isOpen;

    [Header("UI Items Section")]
    public GameObject ui_Window;
    public Image[] items_images;

    [Header("Item Description")]
    public TextMeshProUGUI description_Title;  
    public TextMeshProUGUI description_Text;   

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
        Update_UI();
    }

    public void PickUp(GameObject item)
    {
        items.Add(item);
        Update_UI();
    }

    void Update_UI()
    {
        HideAll();
        for (int i = 0; i < items.Count; i++)
        {
            items_images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            items_images[i].gameObject.SetActive(true);
        }
    }

    void HideAll()
    {
        foreach (var i in items_images) { i.gameObject.SetActive(false); }
        HideDescription();
    }

    public void ShowDescription(int id)
    {
        description_Title.text = items[id].name;
        description_Text.text = items[id].GetComponent<Item>().descriptionText;
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
            Debug.Log($"CONSUMED {items[id].name}");
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            Destroy(items[id], 0.1f);
            items.RemoveAt(id);
            Update_UI();
        }
    }
}
