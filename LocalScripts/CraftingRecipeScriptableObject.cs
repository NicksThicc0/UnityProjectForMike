using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Items/Crafting Recipe")]
public class CraftingRecipeScriptableObject : ScriptableObject
{
    public CraftingItem[] neededItems = new CraftingItem[4];

    public ItemScriptableObject craftedItem;
    public int amountCrafted = 1;
}
