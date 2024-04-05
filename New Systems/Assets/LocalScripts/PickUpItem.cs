using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public ItemScriptableObject Item;
    public int amountInStack = 1;

    public bool Dropped;

    SpriteRenderer _spriteR;

    private void Awake()
    {
        _spriteR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _spriteR.sprite = Item.itemIcon;
        gameObject.name = Item.name;
        Destroy(gameObject, 180);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        if (InventoryManager.instance.inventoryFull) return;

        if (Dropped) return;
        transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 10 * Time.fixedDeltaTime);

        float dist = Vector2.Distance(transform.position, collision.transform.position) / 2;

        transform.localScale = new Vector3(dist, dist, dist);

        //Picked Up Item
        if (Vector2.Distance(transform.position, collision.transform.position) <= .2f)
        {
            Destroy(gameObject);
            PlayerMovement.instance.sfx.playSound("PickUpItem");
            InventoryManager.instance.pickUpItem(Item, amountInStack);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        transform.localScale = new Vector3(1, 1, 1);

        if (Dropped)
        {
            Dropped = false;
        }
    }

}
