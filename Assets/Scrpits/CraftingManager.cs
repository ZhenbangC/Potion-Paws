using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    public GridPosition craftingGrid;              // 左侧拼接Grid
    public CraftingRecipe[] recipes;               // 配方
    public InventorySystem inventory;              // 背包系统
    public Transform materialRoot;                 // 右侧素材展示区域
    public GameObject materialSlotPrefab;          // 一个素材Slot预制体

    void Start()
    {
        DisplayMaterials();
    }

    public void DisplayMaterials()
    {
        Dictionary<string, int> backpackItems = inventory.GetAllItems();

        // 先清空现有
        foreach (Transform child in materialRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (var pair in backpackItems)
        {
            // 创建新素材Slot
            GameObject go = Instantiate(materialSlotPrefab, materialRoot);

            // 设定图标
            var data = Resources.Load<itemData>($"Items/{pair.Key}");
            if (data != null)
            {
                Debug.Log("加载成功 itemData：" + data.name + " 图标是否存在：" + (data.itemIcon != null));

                var img = go.GetComponentInChildren<Image>();
                if (img != null)
                {
                    img.sprite = data.itemIcon;
                }
                else
                {
                    Debug.LogWarning("未找到 Image 组件！");
                }
            }
            else
            {
                Debug.LogWarning("未在 Resources/Items 中找到 itemData：" + pair.Key);
            }

            // 设定数量
            go.GetComponentInChildren<TextMeshProUGUI>().text = pair.Value.ToString();

            // 可选：给每个素材Slot加上点击逻辑，让玩家点一下选中素材，然后放到拼接Grid里（可以后面再加）
        }
    }

    public void TryCraft()
    {
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                InventoryItem item = craftingGrid.GetItem(x, y);
                if (item == null) continue;

                string name = item.itemData.name;
                if (!itemCounts.ContainsKey(name)) itemCounts[name] = 0;
                itemCounts[name]++;
            }
        }

        foreach (var recipe in recipes)
        {
            if (recipe.CheckMatch(itemCounts))
            {
                inventory.AddItemByName(recipe.resultItemName, 1);
                Debug.Log("合成成功：" + recipe.resultItemName);
                return;
            }
        }

        Debug.Log("合成失败");
    }
}