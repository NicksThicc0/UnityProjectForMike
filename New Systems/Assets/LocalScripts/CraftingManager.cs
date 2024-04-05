using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;
    [Header("Crafting Slots")]
    [Header("Recipes")]
    public CraftingRecipeScriptableObject[] allRecipes;
    //public CraftingRecipeScriptableObject currentCraftingRecipe;
    //[Header("Other")]
    //public List<bool> hasItemInSlot;
    //public bool canCraft;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public void CheckRecipes(CraftingSlot[] currentCraftingSlots, CraftingSlot CraftedSlot, CraftingRecipeScriptableObject currentCraftingRecipe, List<bool> hasItemInSlot, bool canCraft, CraftingBench craftingBench)
    {
        if (hasItemInSlot.Count != currentCraftingSlots.Length)
        {
            hasItemInSlot.Clear();
            canCraft = false;
        }

        if (canCraft)
        {

            CraftedSlot.currentItem = currentCraftingRecipe.craftedItem;
            CraftedSlot.amountInSlot = currentCraftingRecipe.amountCrafted;
            CraftedSlot.checkSlot();

        }
        else if(canCraft)
        {
            CraftedSlot.currentItem = null;
            CraftedSlot.amountInSlot = 0;
            CraftedSlot.checkSlot();
        }
        //checkAllRecipesForMatch();
        checkAllRecipes(craftingBench);
        //checkSlots(craftingBench);

    }

    public void checkSlots(CraftingBench craftingBench)
    {
        if (craftingBench.currentRecipe == null) return;
        if (craftingBench.craftingSlots.Length > 0)
        {
            for (int craftingSlots = 0; craftingSlots < craftingBench.craftingSlots.Length; craftingSlots++)
            {
                if (craftingSlots < craftingBench.currentRecipe.neededItems.Length)
                {
                    if (craftingBench.craftingSlots[craftingSlots].currentItem != craftingBench.currentRecipe.neededItems[craftingSlots].Item || craftingBench.craftingSlots[craftingSlots].amountInSlot < craftingBench.currentRecipe.neededItems[craftingSlots].amountNeeded)
                    {
                        craftingBench.hasItemInSlot[craftingSlots] = false;
                    }
                    if (craftingBench.craftingSlots[craftingSlots].currentItem == craftingBench.currentRecipe.neededItems[craftingSlots].Item && craftingBench.craftingSlots[craftingSlots].amountInSlot >= craftingBench.currentRecipe.neededItems[craftingSlots].amountNeeded)
                    {
                        craftingBench.hasItemInSlot[craftingSlots] = true;
                    }
                }
                else
                {
                    if (craftingBench.craftingSlots[craftingSlots].currentItem != null)
                    {
                        craftingBench.hasItemInSlot[craftingSlots] = false;
                    }
                    else
                    {
                        craftingBench.hasItemInSlot[craftingSlots] = true;
                    }
                }
            }
            //
            for (int i = 0; i < craftingBench.hasItemInSlot.Count; i++)
            {
                if (!craftingBench.hasItemInSlot[i])
                {
                    craftingBench.canCraft = false;
                    craftingBench.craftedSlot.currentItem = null;
                    craftingBench.craftedSlot.amountInSlot = 0;
                    craftingBench.craftedSlot.checkSlot();
                    return;
                }
                else
                {
                    craftingBench.canCraft = true;
                }
            }
        }
    }

    public void checkAllRecipes(CraftingBench craftingBench)
    {

        if (craftingBench.canCraft) return;
        foreach (var recipe in instance.allRecipes)
        {

            for (int slot = 0; slot < craftingBench.craftingSlots.Length; slot++)
            {
                for (int neededItems = 0; neededItems < recipe.neededItems.Length; neededItems++)
                {
                    if (craftingBench.craftingSlots[slot].currentItem != null)
                    {
                        if(slot < recipe.neededItems.Length)
                        {

                            if (craftingBench.craftingSlots[slot].currentItem != recipe.neededItems[slot].Item)
                            {
                                break;
                            }
                            if (craftingBench.craftingSlots[slot].currentItem == recipe.neededItems[slot].Item)
                            {
                                if (craftingBench.canCraft) return;
                                if(craftingBench.hasItemInSlot.Count >= recipe.neededItems.Length)
                                {
                                    craftingBench.currentRecipe = recipe;
                                    craftingBench.craftingSlots[slot].checkSlot();
                                }
                            }
                        }
                    }
                }
            }
            
        }

    }

    public void TakeCraftedItem(CraftingBench Crafter)
    {
        for (int i = 0; i < Crafter.currentRecipe.neededItems.Length; i++)
        {
            Crafter.craftingSlots[i].amountInSlot -= Crafter.currentRecipe.neededItems[i].amountNeeded;
            Crafter.craftingSlots[i].checkSlot();
        }
    }


    private void OnApplicationQuit()
    {
        //hasItemInSlot.Clear();
    }
}


[System.Serializable]
public class CraftingItem
{
    [SerializeField] string itemName;
    public ItemScriptableObject Item;
    public int amountNeeded;
}
