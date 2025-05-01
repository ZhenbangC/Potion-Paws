using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class MaterialSlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;

    public void SetSlot(Sprite icon, int count)
    {
        if (iconImage == null)
        {
            Debug.LogError("MaterialSlot：iconImage 没有绑定！");
            return;
        }

        if (icon != null)
        {
            iconImage.sprite = icon;

            iconImage.enabled = true;
            iconImage.raycastTarget = true;

            // 确保 CanvasGroup 存在并允许拖拽时事件生效
            var cg = iconImage.GetComponent<CanvasGroup>();
            if (cg == null) cg = iconImage.gameObject.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }
        else
        {
            Debug.LogWarning("MaterialSlot：传入的 icon 是空的！");
            iconImage.sprite = null;

            iconImage.enabled = true;
            iconImage.color = new Color(1, 1, 1, 0.2f); // 设为半透明代替关闭
        }

        if (countText != null)
        {
            countText.text = count > 1 ? count.ToString() : "";
            countText.enabled = true;
        }
    }

    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0.2f); // 半透明状态
        }

        if (countText != null)
        {
            countText.text = "";
            countText.enabled = false;
        }
    }
}