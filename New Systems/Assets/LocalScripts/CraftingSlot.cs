using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : InventorySlot, IPointerDownHandler
{
    public bool craftedSlot;
    public bool beingDragged;
    CraftingBench craftingBench;

    private void Start()
    {
        craftingBench = GetComponentInParent<CraftingBench>();
    }

    public override void checkSlot()
    {
        base.checkSlot();
        if(!craftedSlot) CraftingManager.instance.checkSlots(craftingBench);
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if(currentItem != null & /*CraftingManager.instance.canCraft &*/ craftedSlot)
        {
            beingDragged = true;
            //CraftingManager.instance.TakeCraftedItem();
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0) & craftedSlot & beingDragged)
        {
            //CraftingManager.instance.checkSlots();
            beingDragged = false;
            CraftingManager.instance.TakeCraftedItem(craftingBench);

        }
    }

}
