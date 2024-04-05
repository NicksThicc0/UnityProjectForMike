using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spider : EnemyClass
{
    [Header("Spiders Specs")]
    [SerializeField] bool canMove = true;
    [SerializeField] Vector2 playersLastPos;
    [SerializeField] GameObject webProjectile;
    [SerializeField] GameObject webHazard;
    [SerializeField] Transform poopSpot;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        Animatons();
        UIstuff();
        AttackCooldown();
        //
    }

    private void FixedUpdate()
    {
        moveTowardsPlayer();
    }


    public override void moveTowardsPlayer()
    {
        if (canMove)
        {
            base.moveTowardsPlayer();
        }
        else
        {
            agent.velocity = Vector2.zero;
            agent.speed = 0;
            rb.velocity = Vector2.zero;
        }
    }

    public override void AttackCooldown()
    {
        base.AttackCooldown();
    }

    public override void Attack()
    {
        anim.Play("Attack");
        attackSpeed = minAttackSpeed;
        playersLastPos = Player.position;
        Debug.Log("Attack");
        gfx.localScale = new Vector3(-gfx.localScale.x, gfx.localScale.y, gfx.localScale.z);
    }


    public override void Animatons()
    {
        base.Animatons();
    }
    public override void UIstuff()
    {
        base.UIstuff();
    }


    //
    public void shootWeb()
    {
        GameObject newProjectile = Instantiate(webProjectile, poopSpot.position, poopSpot.rotation);
        newProjectile.layer = 6;
        Rigidbody2D bulletRb = newProjectile.GetComponent<Rigidbody2D>();
        bulletRb.AddForce((Player.position - transform.position) * projectileSpeed * 10);
    }
}
