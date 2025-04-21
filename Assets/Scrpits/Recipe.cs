using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    public string resultItemName;
    public int resultAmount = 1;

    [System.Serializable]
    public class Ingredient
    {
        public string itemName;
        public int amount;
    }

    public List<Ingredient> ingredients;
}