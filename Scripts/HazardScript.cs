using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardScript : MonoBehaviour
{
    public enum hazardType { spiderWeb, Hurtbox, Slippery}
    [SerializeField] hazardType Harzard;
    [SerializeField] bool Trigger = true;
    [Header("Other")]
    public int Damage;
    PlayerMovement player;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Trigger) return;
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerMovement>();
        }

        if (Harzard == hazardType.spiderWeb)
        {
            if(collision.gameObject.tag == "Player")
            {
                player.movementSpeed = player.baseSpeed / 2; 
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Trigger) return;
        if (collision.gameObject.tag == "Player")
        {
            if (Harzard == hazardType.Hurtbox)
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
            }
            if(Harzard == hazardType.Slippery)
            {
                player.slipping = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Trigger) return;
        if (Harzard == hazardType.spiderWeb)
        {
            if (collision.gameObject.tag == "Player")
            {
                player.movementSpeed = player.baseSpeed;
            }
        }
        if (Harzard == hazardType.Slippery)
        {
            player.slipping = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Trigger) return;

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
        }
    }
}
