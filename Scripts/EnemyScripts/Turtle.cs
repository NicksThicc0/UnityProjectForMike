using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turtle : EnemyClass
{
    [Header("Turtle Specs")]
    [SerializeField] float force = 100;
    [SerializeField] GameObject hitWallParticle;
    [SerializeField] TrailRenderer DashTrail;
    bool Dashed;
    int Hits;
    [SerializeField] float outOfShellRange = 8;

    public override void Start()
    {
        base.Start();
        DashTrail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        AttackCooldown();
        UIstuff();
        DashTrail.enabled = Dashed;
        if(Dashed & Vector2.Distance(transform.position, Player.position) >= outOfShellRange)
        {
            Dashed = false;
            Hits = 0;
            anim.SetBool("Spinng", false);
            rb.velocity = Vector2.zero;
        }
    }
    private void FixedUpdate()
    {
        moveTowardsPlayer();
    }


    public override void moveTowardsPlayer()
    {
        if (!Dashed)
        {
            base.moveTowardsPlayer();
        }
    }

    public override void Attack()
    {
        if (Dashed) return;
        Dashed = true;
        attackSpeed = minAttackSpeed;
        anim.SetBool("Spinng", true);
        rb.AddForce((Player.position - transform.position).normalized * force * 1000);
        Hits = 0;
        if(Health < baseHealth)Health++;
        StartCoroutine(GoOutOfShell());
    }
    public override void UIstuff()
    {
        base.UIstuff();
    }
    public override void AttackCooldown()
    {
        if (Dashed) return;
        base.AttackCooldown();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Hits <= 2 & collision.gameObject.tag != "Bullet") Hits++;

        if (Dashed)
        {
            if (collision.gameObject.tag == "Ground")
            {
                GameObject effect = Instantiate(hitWallParticle, transform.position, transform.rotation);
                StartCoroutine(GunScript.Shake(0.1f, .1f));
                SFX.playSound("HitWall");
                Destroy(effect, 2);
            }
            //Stopping if enough hits
            if (Hits >= 2 & collision.gameObject.tag != "Bullet")
            {
                Debug.Log("Turtle hit wall");
                anim.SetBool("Spinng", false);
                rb.velocity = Vector2.zero;
                Dashed = false;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
        }
    }

    IEnumerator GoOutOfShell()
    {
        yield return new WaitForSecondsRealtime(6);
        Dashed = false;
        Hits = 0;
        anim.SetBool("Spinng", false);
        rb.velocity = Vector2.zero;
    }
}
