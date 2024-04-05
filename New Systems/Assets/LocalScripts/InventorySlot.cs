using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public ItemScriptableObject currentItem;
    public int amountInSlot;
    public bool Full;
    public bool canPutItemInSlot = true;
    //
    public Image Icon;
    [SerializeField] TMP_Text itemAmountTxt;
    public Transform Parent;

    public bool isInventorySlot;
    public bool blockUi;

    public virtual void Awake()
    {
        if(!Icon) Icon = transform.GetChild(0).GetComponent<Image>();
        if(!itemAmountTxt)itemAmountTxt = GetComponentInChildren<TMP_Text>();
        if (!Parent) Parent = gameObject.transform;
        //Disabling Icon        
        Icon.gameObject.SetActive(false);
        Icon.raycastTarget = false;
        //disabling Text
        itemAmountTxt.gameObject.SetActive(false);
        itemAmountTxt.transform.SetParent(Icon.transform);
        //
    }

    private void Start()
    {
        checkSlot();
    }

    public virtual void NewItem(ItemScriptableObject newItem)
    {
        if(currentItem != null & !Full)
        {
            if(newItem == currentItem)
            {
                amountInSlot++;

                if (amountInSlot > currentItem.maxStackAmount)
                {
                    amountInSlot = currentItem.maxStackAmount;
                }
            }
            //Setting Sprite
            Icon.gameObject.SetActive(true);
            Icon.sprite = currentItem.itemIcon;
            //Checking if full
            Full = amountInSlot >= currentItem.maxStackAmount;
        }
        else if(currentItem == null & !Full)
        {
            //Setting Stats
            currentItem = newItem;
            amountInSlot++;
            //setting Sprite
            Icon.sprite = currentItem.itemIcon;
            Icon.gameObject.SetActive(true);
            //Checking if slot full
            Full = amountInSlot >= currentItem.maxStackAmount;
            //
        }

        //Setting text
        itemAmountTxt.gameObject.SetActive(amountInSlot > 1);
        itemAmountTxt.text = amountInSlot.ToString();
        Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);

        /* Nothing in slot
            itemAmountTxt.gameObject.SetActive(false);
            Icon.gameObject.SetActive(false);
            Full = false;
        */
    }


    public virtual void checkSlot()
    {
        if(currentItem == null)
        {
            amountInSlot = 0;
            itemAmountTxt.gameObject.SetActive(false);
            Icon.gameObject.SetActive(false);
            Full = false;
            Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
            //
        }
        else
        {
            Full = amountInSlot >= currentItem.maxStackAmount;
            Icon.sprite = currentItem.itemIcon;
            Icon.gameObject.SetActive(true);
            itemAmountTxt.gameObject.SetActive(amountInSlot > 1);
            itemAmountTxt.text = amountInSlot.ToString();
            if(amountInSlot <= 0)
            {
                currentItem = null;
                amountInSlot = 0;
                itemAmountTxt.gameObject.SetActive(false);
                Icon.gameObject.SetActive(false);
                Full = false;
            }
            Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
        }
        //reseting scale
        Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
    }

    //Checking Mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.instance.mouseOverUi = true;

        if(currentItem != null & InventoryManager.instance.inventoryOpen)
        {
            //ToolTip
            InventoryManager.instance.toolTip.SetActive(true);
            InventoryManager.instance._tipTxt.text = currentItem.toolTip;
            InventoryManager.instance.toolTipIcon.sprite = currentItem.itemIcon;
            InventoryManager.instance.toolTipIcon.gameObject.SetActive(true);
            //
        }
        //Dragging Item
        if(InventoryManager.instance.currentItemBeingDragged != null & canPutItemInSlot)
        {
            InventoryManager.instance.invSlotCurrentlyOver = this;
        }
        if(currentItem != null)InventoryManager.instance.SFX.playSound("PickUpItem");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.instance.mouseOverUi = false;

        if (!InventoryManager.instance.inventoryOpen) return;
        if(currentItem != null)
        {
            //ToolTip
            InventoryManager.instance.toolTip.SetActive(false);
        }
        //Dragging Item
        if (InventoryManager.instance.currentItemBeingDragged != null)
        {
            InventoryManager.instance.invSlotCurrentlyOver = null;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(Input.GetMouseButtonDown(0) & Input.GetKey(KeyCode.LeftShift) & currentItem != null & !isInventorySlot)// Shift clicking
        {
            InventoryManager.instance.pickUpItem(currentItem, amountInSlot);
            amountInSlot = 0;
            currentItem = null;
            checkSlot();
            return;
        } //Shift Clicking

        if (Input.GetMouseButtonDown(1)& currentItem != null & amountInSlot > 1) // Spliting items
        {
            //Resetting the amount / setting Leftovers amount to whats = to current amount in slot
            var leftOvers = amountInSlot;
            InventoryManager.instance.ghostInvSlot.amountInSlot = 0;
            //removing 1 left over and then adding it to the ghost slot(split slot)
            for (int i = 0; i < amountInSlot /2; i++)
            {
                leftOvers -= 1;
                InventoryManager.instance.ghostInvSlot.amountInSlot += 1;
            }
            //Setting Gameobject / Sprite;
            InventoryManager.instance.ghostInvSlot.currentItem = currentItem;
            InventoryManager.instance.ghostInvSlot.Icon.sprite = Icon.sprite;
            //Setting current drag slot and dragged item
            InventoryManager.instance.currentItemBeingDragged = InventoryManager.instance.ghostInvSlot.Icon.transform;
            InventoryManager.instance.currentInvSlotBeingDragged = InventoryManager.instance.ghostInvSlot;
            //CheckingSlots
            InventoryManager.instance.ghostInvSlot.checkSlot();
            //checking slots
            amountInSlot = leftOvers;
            checkSlot();
        } // splitting items

        if (Input.GetMouseButtonDown(2) & currentItem != null) // taking 1 item
        {
            //Resetting the amount / setting Leftovers amount to whats = to current amount in slot
            InventoryManager.instance.ghostInvSlot.amountInSlot = 0;
            //removing 1 left over and then adding it to the ghost slot(split slot)
            amountInSlot -= 1;
            InventoryManager.instance.ghostInvSlot.amountInSlot = 1;
            //Setting Gameobject / Sprite;
            InventoryManager.instance.ghostInvSlot.currentItem = currentItem;
            InventoryManager.instance.ghostInvSlot.Icon.sprite = Icon.sprite;
            //Setting current drag slot and dragged item
            InventoryManager.instance.currentItemBeingDragged = InventoryManager.instance.ghostInvSlot.Icon.transform;
            InventoryManager.instance.currentInvSlotBeingDragged = InventoryManager.instance.ghostInvSlot;
            //CheckingSlots
            InventoryManager.instance.ghostInvSlot.checkSlot();
            //checking slots
            checkSlot();
        } // taking 1 item


        if (Input.GetMouseButtonDown(0)& currentItem != null & InventoryManager.instance.currentItemBeingDragged == null) 
        {
            InventoryManager.instance.currentItemBeingDragged = Icon.transform;
            InventoryManager.instance.currentInvSlotBeingDragged = this;
            Icon.transform.SetParent(InventoryManager.instance.DraggedParent);
            Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
            if (currentItem != null) InventoryManager.instance.SFX.playSound("PickUpItem");
        }
    }
}
