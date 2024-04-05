using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;

    [SerializeField] GameObject breakEffect;
    [SerializeField] float destroyTime = 10;
    [SerializeField] bool Ricochet;
    public SoundScript sfx;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Ricochet)
        {
            bulletStuff(collision);
        }
        else if (Ricochet)
        {
            bulletStuff(collision);
        }
    }


    void bulletStuff(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            if (collision.gameObject.tag == "Object")
            {
                collision.gameObject.GetComponent<ObjectHealth>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            if(collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyClass>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            if(collision.gameObject.tag == "Boss")
            {
                collision.gameObject.GetComponent<BossClass>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            if(collision.gameObject.tag == "Ground")
            {
                sfx.playSound("HitWall");
            }
            if (Ricochet & rb.velocity.magnitude < 5)
            {
                Destroy(gameObject);
            }
            if (!Ricochet)
            {
                Destroy(gameObject);
            }

        }
    }

    private void OnDestroy()
    {
        if (breakEffect != null)
        {
            GameObject effect = Instantiate(breakEffect, transform.position, transform.rotation);
            Destroy(effect, 2);
        }
    }
}
