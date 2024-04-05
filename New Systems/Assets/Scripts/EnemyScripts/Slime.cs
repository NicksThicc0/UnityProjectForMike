using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyClass
{
    [Header("Slime Attack Specs")]
    public Transform ShootSpot;
    [SerializeField] GameObject Projectile;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Animatons();
        AttackCooldown();
        UIstuff();
    }

    private void FixedUpdate()
    {
        moveTowardsPlayer();
    }

    public override void Attack()
    {
        attackSpeed = minAttackSpeed;
        anim.Play("Shoot");
    }
    public override void AttackCooldown()
    {
        base.AttackCooldown();
    }
    public override void UIstuff()
    {
        base.UIstuff();
    }
    public override void moveTowardsPlayer()
    {
        base.moveTowardsPlayer();
    }
    public void ShootSlimeball()
    {
        GameObject newProjectile = Instantiate(Projectile, ShootSpot.position, ShootSpot.rotation);
        newProjectile.layer = 6;
        Rigidbody2D bulletRb = newProjectile.GetComponent<Rigidbody2D>();
        bulletRb.AddForce((Player.position - transform.position).normalized * projectileSpeed * 10);
    }

    public override void Animatons()
    {
        base.Animatons();
    }
}
