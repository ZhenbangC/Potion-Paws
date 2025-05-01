using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    public InventoryItem inventoryItem;
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("缺少 CanvasGroup 组件！");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("点击：" + gameObject.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("拖拽开始：" + gameObject.name);
        originalParent = transform.parent;

        if (canvas != null)
        {
            transform.SetParent(canvas.transform);  // 提到最上层
        }

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("拖拽结束：" + gameObject.name);

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        transform.SetParent(originalParent);
    }
}