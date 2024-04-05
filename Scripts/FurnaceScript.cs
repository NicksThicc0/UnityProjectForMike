using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceScript : MonoBehaviour
{
    [Header("Furnace Stats")]
    [SerializeField] float cookSpeed = 1f;
    [SerializeField] float amountSmelted = 0;
    [SerializeField] float fuelLeft;

    [Header("UI")]
    [SerializeField] GameObject Ui;
    [SerializeField] Image arrowFill;
    [SerializeField] Image fuelFill;
    [SerializeField] GameObject uiBoxes;
    float lastFuelAmount;

    [Header("Slots")]
    [SerializeField] InventorySlot smeltingSlot;
    [SerializeField] InventorySlot fuelSlot;
    [SerializeField] InventorySlot finishedSlot;
    [Header("Effects")]
    [SerializeField] ParticleSystem Smoke;

    //
    Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SnapToGrid.Snap(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (smeltingSlot.currentItem != null & fuelSlot.currentItem != null)
        {
            if(smeltingSlot.currentItem.smeltedInto != null & fuelSlot.currentItem.amountOfFuel > 0)
            //

            if(finishedSlot.currentItem != null)
            {
               // return;
            }
            //else if (finishedSlot.currentItem == smeltingSlot.currentItem.smeltedInto)

            //Checking if Fuel is = to 0
            if(fuelLeft <= 0 & fuelSlot.currentItem.amountOfFuel > 0)
            {
                //Removing 1 Item
                fuelSlot.amountInSlot -= 1;
                //Checking Slot
                fuelSlot.checkSlot();
                //Setting Fuel left
                if (fuelSlot.currentItem != null)
                {
                    fuelLeft = fuelSlot.currentItem.amountOfFuel;
                    lastFuelAmount = fuelSlot.currentItem.amountOfFuel;
                }
            }
            //Burning the fuel
            else if(fuelLeft > 0)
            {
                fuelLeft -= 1 * Time.deltaTime;
                amountSmelted += 1 * cookSpeed * Time.deltaTime;
            }
            //Checking if amount smelted is = smelt time

            if(amountSmelted >= smeltingSlot.currentItem.timeToSmelt)
            {
                amountSmelted = 0;
                //Removing 1 Item
                smeltingSlot.amountInSlot -= 1;
                //adding 1 item to finished slot
                finishedSlot.currentItem = smeltingSlot.currentItem.smeltedInto;
                finishedSlot.amountInSlot += 1;
                //Checking Slots
                finishedSlot.checkSlot();
                smeltingSlot.checkSlot();
                //
            }

        }
        if (smeltingSlot.currentItem == null | fuelSlot.currentItem == null)
        {
            if (fuelLeft > 0) fuelLeft -= 1 * Time.deltaTime;
            if (amountSmelted > 0 & fuelLeft <= 0) amountSmelted -= 1 * Time.deltaTime;
        }
        // Bars
        if (smeltingSlot.currentItem != null)
        {
            arrowFill.fillAmount = amountSmelted / smeltingSlot.currentItem.timeToSmelt;
        }
        fuelFill.fillAmount = fuelLeft / lastFuelAmount;
        //Anim
        anim.SetBool("Smelting", fuelLeft > 0);
        Smoke.gameObject.SetActive(fuelLeft > 0);
    }

    public void TurnOnUi()
    {
        if (uiBoxes.activeInHierarchy)
        {
            uiBoxes.SetActive(false);
        }
        else
        {
            uiBoxes.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetBool("UI Open", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("UI Open", false);
        }
    }
}
