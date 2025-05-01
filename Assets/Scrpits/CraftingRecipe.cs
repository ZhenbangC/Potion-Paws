using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string resultItemName;
    public int requiredItemCountToFillGrid = 0; // 如果填满格子奖励额外物品

    [System.Serializable]
    public struct Ingredient
    {
        public string itemName;
        public int count;
    }

    public Ingredient[] ingredients;

    /// <summary>
    /// 用于与拼接网格中的物品进行匹配检查。
    /// </summary>
    /// <param name="inputItems">当前拼接格中所有物品的数量统计</param>
    /// <returns>是否完全匹配配方需求</returns>
    public bool CheckMatch(Dictionary<string, int> inputItems)
    {
        foreach (var ingredient in ingredients)
        {
            if (!inputItems.ContainsKey(ingredient.itemName) || inputItems[ingredient.itemName] < ingredient.count)
            {
                return false;
            }
        }

        return true;
    }
}