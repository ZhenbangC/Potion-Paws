using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop }
    public enum ItemType { Static, Consumables }

    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;

    [Header("道具名称（用于堆叠识别）")]
    public string itemName; // 统一命名识别

    public Sprite icon; // 用于UI显示

    [Header("Description")]
    [TextArea(2, 5)]
    public string descriptionText;

    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.isTrigger = true;
        gameObject.layer = 7;
    }

    public void Interact()
    {
        InteractionSystem interactionSystem = FindObjectOfType<InteractionSystem>();
        if (interactionSystem == null) return;

        switch (interactType)
        {
            case InteractionType.PickUp:
                InventorySystem inventorySystem = FindObjectOfType<InventorySystem>();
                if (inventorySystem != null)
                {
                    inventorySystem.PickUp(this); // 改为传递Item
                    gameObject.SetActive(false);
                }
                break;

            case InteractionType.Examine:
                interactionSystem.ExamineItem(this);
                break;

            case InteractionType.GrabDrop:
                interactionSystem.GrabDrop();
                break;
        }

        customEvent?.Invoke();
    }
}