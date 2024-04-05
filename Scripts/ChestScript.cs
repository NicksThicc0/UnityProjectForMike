using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] GameObject Item;
    SpriteRenderer _spriteR;
    Animator anim;


    private void Awake()
    {
        _spriteR = GetComponent<SpriteRenderer>();
        anim = GetComponentInParent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _spriteR.sprite = itemSprite;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            anim.Play("Open");
        }
    }

    public void spawnItem(Transform spawnSpot)
    {
        Instantiate(Item, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }


}
