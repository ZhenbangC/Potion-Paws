using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop }
    public enum ItemType { Static, Consumables }
    public Sprite icon;

    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;

    [Header("数据源")]
    public itemData data;

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
                    inventorySystem.PickUp(this); // 传递当前物体
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

    // ----------------------
    // 属性访问器（方便 UI 调用）
    // ----------------------

    public string itemName => data != null ? data.name : "未命名";
    public Sprite itemIcon => data != null ? data.itemIcon : null;
    public int width => data != null ? data.width : 1;
    public int height => data != null ? data.height : 1;
}