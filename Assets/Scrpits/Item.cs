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
        {
            collider.isTrigger = true;
        }
        gameObject.layer = 7;
    }

    public void Interact()
    {
        InteractionSystem interactionSystem = FindObjectOfType<InteractionSystem>();

        if (interactionSystem == null)
        {
            Debug.LogError("InteractionSystem not found in the scene!");
            return;
        }

        switch (interactType)
        {
            case InteractionType.PickUp:
                InventorySystem inventorySystem = FindObjectOfType<InventorySystem>();
                if (inventorySystem != null)
                {
                    inventorySystem.PickUp(gameObject);

                  
                    int index = inventorySystem.items.IndexOf(gameObject);
                    if (index >= 0)
                    {
                        inventorySystem.ShowDescription(index);
                    }

                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogError("InventorySystem not found in the scene!");
                }
                break;

            case InteractionType.Examine:
                interactionSystem.ExamineItem(this);
                break;

            case InteractionType.GrabDrop:
                interactionSystem.GrabDrop();
                break;

            default:
                Debug.LogWarning("No valid interaction type set for: " + gameObject.name);
                break;
        }

       
        if (customEvent != null)
        {
            customEvent.Invoke();
        }
        else
        {
            Debug.LogWarning("Custom event is null on: " + gameObject.name);
        }
    }
}