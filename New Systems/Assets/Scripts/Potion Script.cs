using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    enum potionType { Health}
    [SerializeField]potionType Potion;
    [SerializeField] int Amount;
    Animator animator;
    PlayerMovement player;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if(Potion == potionType.Health)
        {
            if(Amount == 999)
            {
                animator.Play("MegaPotionWobble");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        player = collision.gameObject.GetComponent<PlayerMovement>();

        if (Potion == potionType.Health)
        {
            player.Health += Amount;
            player.sfx.playSound("Heal");
            if (player.Health >= player.maxHealth)
            {
                player.Health = player.maxHealth;
            }

        }

        Destroy(gameObject);

    }

}
