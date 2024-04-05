using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{
    [Header("Crafting Slots")]
    public CraftingSlot[] craftingSlots;
    public CraftingSlot craftedSlot;
    public CraftingRecipeScriptableObject currentRecipe;
    [Header("other")]
    [SerializeField]bool hasPlayer;
    public List<bool> hasItemInSlot;
    public bool canCraft;

    private void Start()
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            hasItemInSlot.Add(new bool());
        }
        
    }

    private void Update()
    {
        if (hasPlayer)
        {
            CraftingManager.instance.CheckRecipes(craftingSlots, craftedSlot,currentRecipe,hasItemInSlot, canCraft, this);

        }
    }

    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        hasPlayer = true;
        /*CraftingManager.instance.currentCraftingSlots = new CraftingSlot[craftingSlots.Length];
        CraftingManager.instance.currentCraftingSlots = craftingSlots;
        CraftingManager.instance.CraftedSlot = craftedSlot;*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        hasPlayer = false;
    }
}
