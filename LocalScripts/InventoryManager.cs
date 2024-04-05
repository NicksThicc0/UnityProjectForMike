using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventorySlot[] invSlots;
    public bool inventoryOpen;
    public bool inventoryFull;


    //
    [Header("Sprites")]
    public Sprite unselectedHotbar;
    public Sprite selectedHotbar;
    [Header("Current Hotbar")]
    public HotbarSlot currentHotbar;
    [Header("Tool Tips")]
    public GameObject toolTip;
    public TMP_Text _tipTxt;
    public Image toolTipIcon;
    [Header("Dragging Items")]
    public Transform DraggedParent;
    public Transform currentItemBeingDragged;
    public InventorySlot currentInvSlotBeingDragged;
    public InventorySlot invSlotCurrentlyOver;
    public InventorySlot ghostInvSlot;
    [Header("Effects")]
    public SoundScript SFX;
    [Header("Other")]
    public bool mouseOverUi;
    [SerializeField] TMP_Text fpsTxt;
    [SerializeField] int fps;

    Animator anim;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void pickUpItem(ItemScriptableObject pickedUpItem, int amountInStack)
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            /*for (int checkForSameItem = 0; checkForSameItem < invSlots.Length; checkForSameItem++)
            {
                if (invSlots[checkForSameItem].currentItem != null)
            }*/


            if (!invSlots[i].Full & invSlots[i].currentItem == pickedUpItem)
            {
                for (int L = 0; L < amountInStack; L++)
                {
                    //Checking if full
                    if (invSlots[i].Full)
                    {
                        var resetItem = pickedUpItem;
                        pickUpItem(resetItem, amountInStack - L);
                        return;
                    }
                    //Else
                    invSlots[i].NewItem(pickedUpItem);
                    if(L == amountInStack - 1)
                    {
                        return;
                    }
                }
            }

            else if (!invSlots[i].Full & invSlots[i].currentItem == null)
            {
                for (int L = 0; L < amountInStack; L++)
                {
                    //Checking if full
                    if (invSlots[i].Full)
                    {
                        var resetItem = pickedUpItem;
                        pickUpItem(resetItem, amountInStack - L);
                        return;
                    }
                    //Else
                    invSlots[i].NewItem(pickedUpItem);
                    if (L == amountInStack - 1)
                    {
                        return;
                    }
                }
            }
            checkIfInventoryIsFull();

        }
    }

    // Update is called once per frame
    void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsTxt.text = fps.ToString() + "FPS";

        //
        PlayerMovement.instance.canAttack = !mouseOverUi;
        if (Input.GetKeyDown(KeyCode.Tab) & currentItemBeingDragged == null)
        {
            var Open = !anim.GetBool("Open");
            inventoryOpen = Open;
            anim.SetBool("Open", Open);
            //Checking Tooltip
            if (toolTip.activeInHierarchy & !Open) toolTip.SetActive(false);
        }
        if(currentItemBeingDragged != null)
        {
            //Setting icon to mouse Pos
            currentItemBeingDragged.transform.position = Input.mousePosition;

            //Dropping item in slot
            if (Input.GetMouseButtonUp(0))
            {
                //Checking if ur over the same hot bar ur dragging
                if (invSlotCurrentlyOver == currentInvSlotBeingDragged)
                {
                    currentItemBeingDragged.transform.SetParent(currentInvSlotBeingDragged.transform);
                    currentItemBeingDragged.localPosition = Vector2.zero;
                    currentItemBeingDragged = null;
                    currentInvSlotBeingDragged.checkSlot();

                    return;
                }

                if(invSlotCurrentlyOver != null)
                {
                    //If over invslot
                    if(invSlotCurrentlyOver.currentItem != null)
                    {

                        if (!currentInvSlotBeingDragged.canPutItemInSlot)
                        {
                            pickUpItem(currentInvSlotBeingDragged.currentItem, currentInvSlotBeingDragged.amountInSlot);
                            currentItemBeingDragged.transform.SetParent(currentInvSlotBeingDragged.transform);
                            currentInvSlotBeingDragged.Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
                            currentItemBeingDragged.localPosition = Vector2.zero;
                            currentItemBeingDragged = null;
                            currentInvSlotBeingDragged = null;
                            return;
                        }

                        if(invSlotCurrentlyOver.currentItem == currentInvSlotBeingDragged.currentItem & !invSlotCurrentlyOver.Full)
                        {
                            Debug.Log("Same item");
                            for (int i = 0; i < currentInvSlotBeingDragged.amountInSlot; i++)
                            {
                                //Debug.Log("Amount in Stack" + i);
                                if (!invSlotCurrentlyOver.Full)
                                {
                                    invSlotCurrentlyOver.NewItem(currentInvSlotBeingDragged.currentItem);
                                }
                                else
                                {
                                    pickUpItem(currentInvSlotBeingDragged.currentItem, i - currentInvSlotBeingDragged.amountInSlot);
                                }
                            }
                            currentInvSlotBeingDragged.currentItem = null;
                            currentInvSlotBeingDragged.checkSlot();
                            currentItemBeingDragged.transform.SetParent(currentInvSlotBeingDragged.transform);
                            currentItemBeingDragged.localPosition = Vector2.zero;
                            currentItemBeingDragged = null;
                            return;
                        }

                        var ghostItem = currentInvSlotBeingDragged.currentItem;
                        var ghostAmount = currentInvSlotBeingDragged.amountInSlot;
                        //Swaping Items from current Inventory slot being dragged

                        currentInvSlotBeingDragged.currentItem = invSlotCurrentlyOver.currentItem;
                        currentInvSlotBeingDragged.amountInSlot = invSlotCurrentlyOver.amountInSlot;
                        currentInvSlotBeingDragged.checkSlot();

                        //Swaping Current inventory slot ur dropping the items on stats

                        invSlotCurrentlyOver.currentItem = ghostItem;
                        invSlotCurrentlyOver.amountInSlot = ghostAmount;
                        invSlotCurrentlyOver.checkSlot();
                        //
                        //Reset Icon
                        currentItemBeingDragged.transform.SetParent(currentInvSlotBeingDragged.transform);
                        currentItemBeingDragged.localPosition = Vector2.zero;
                        currentItemBeingDragged = null;
                        //
                        invSlotCurrentlyOver.checkSlot();
                    }
                    else
                    {
                        //Setting Current inventory slot ur dropping the items on stats
                        invSlotCurrentlyOver.currentItem = currentInvSlotBeingDragged.currentItem;
                        invSlotCurrentlyOver.amountInSlot = currentInvSlotBeingDragged.amountInSlot;
                        invSlotCurrentlyOver.checkSlot();
                        //Removing Items from current Inventory slot being dragged
                        currentInvSlotBeingDragged.currentItem = null;
                        currentInvSlotBeingDragged.checkSlot();
                        //
                        //Reset Icon
                        currentItemBeingDragged.transform.SetParent(currentInvSlotBeingDragged.transform);
                        currentItemBeingDragged.localPosition = Vector2.zero;
                        currentItemBeingDragged = null;
                    }
                    //play sound
                    SFX.playSound("putItemInSlot");
                }
                else
                {
                    //Dropping item

                    GameObject droppedItem = Instantiate(Resources.Load<GameObject>("Item On Ground"), new Vector2(Random.Range(PlayerMovement.instance.transform.position.x -2, PlayerMovement.instance.transform.position.x + 2), Random.Range(PlayerMovement.instance.transform.position.y - 2, PlayerMovement.instance.transform.position.y + 2)), Quaternion.identity);
                    droppedItem.GetComponent<PickUpItem>().Item = currentInvSlotBeingDragged.currentItem;
                    droppedItem.GetComponent<PickUpItem>().amountInStack = currentInvSlotBeingDragged.amountInSlot;
                    droppedItem.GetComponent<PickUpItem>().Dropped = true;
                    currentInvSlotBeingDragged.currentItem = null;
                    currentInvSlotBeingDragged.amountInSlot = 0;
                    currentInvSlotBeingDragged.checkSlot();

                    //Reset Icon
                    currentItemBeingDragged.transform.SetParent(currentInvSlotBeingDragged.transform);
                    currentItemBeingDragged.localPosition = Vector2.zero;
                    currentItemBeingDragged = null;
                }
                //Setting Scale
                if(invSlotCurrentlyOver != null)
                {
                    invSlotCurrentlyOver.Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
                    invSlotCurrentlyOver.checkSlot();
                }
                if(currentInvSlotBeingDragged != null)
                {
                    currentInvSlotBeingDragged.Icon.transform.localScale = new Vector2(0.6729401f, 0.6729401f);
                    currentInvSlotBeingDragged.checkSlot();
                }

            }
        }
    }

    void checkIfInventoryIsFull()
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            if (!invSlots[i].Full)
            {
                inventoryFull = false;
                break;
            }
            else
            {
                inventoryFull = true;
            }
        }
    }
}
