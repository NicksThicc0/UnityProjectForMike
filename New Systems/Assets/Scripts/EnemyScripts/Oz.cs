using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Oz : BossClass
{
    [Header("Oz Specs")]
    [SerializeField] bool canMove = true;
    [SerializeField] Vector2 playersLastPos;
    [SerializeField] Transform shootSpot;
    [SerializeField] bool secondPhase;
    [SerializeField] bool doingAnim;
    Vector2 lastPlayerPos;
    [Header("Ranges")]
    [SerializeField] float closeRange;
    [SerializeField] float MidRange;
    [Header("Attack Prefabs")]
    [SerializeField] GameObject magicRing;
    [SerializeField] GameObject magicTrap;
    [SerializeField] GameObject slimeBall;
    [Header("Effects")]
    [SerializeField] GameObject slimeEffect;
    [SerializeField] ParticleSystem teleportEffect;
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        moveTowardsPlayer();
        Animatons();
        UIstuff();
        AttackCooldown();
        //
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
            //
            if (Player.position.x < transform.position.x)
            {
                gfx.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gfx.localScale = new Vector3(1, 1, 1);
            }

        }
    }

    public override void AttackCooldown()
    {
        base.AttackCooldown();
    }

    public override void Attack()
    {
        if(secondPhase)
        {
            attackSpeed = minAttackSpeed / 2;
        }
        else
        {
            attackSpeed = minAttackSpeed;
        }
        playersLastPos = Player.position;
        //Ranges
        if (Vector2.Distance(transform.position, Player.position) <= closeRange & !doingAnim)
        {
            anim.Play("CloseAttack");
            return;
           
        }
        if (Vector2.Distance(transform.position, Player.position) <= MidRange & !doingAnim)
        {
            anim.Play("MidAttack");
            return;
        }
        if (Vector2.Distance(transform.position, Player.position) <= attackRange & !doingAnim)
        {
            anim.Play("FarRange");
            return;
        }
        Debug.Log("attack");
    }


    public override void Animatons()
    {
        base.Animatons();
    }
    public override void UIstuff()
    {
        base.UIstuff();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if(Health <= maxHealth/2 & !secondPhase)
        {
            secondPhase = true;
            anim.Play("HalfWay");
        }
        if(Health <= 0)
        {
            anim.Play("Die");
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, closeRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, MidRange);
        
    }

    //
    public void SpawnRing(int damageAmount = 10)
    {
        GameObject newObject = Instantiate(magicRing, transform.position, quaternion.identity);
        newObject.GetComponent<HazardScript>().Damage = damageAmount;
        StartCoroutine(GunScript.Shake(.1f, .1f));
        Destroy(newObject, 10);
    }

    public void ShootBullets(int amountOfBullets)
    {
        StartCoroutine(Burst(amountOfBullets));
    }

    public void RingOfBullets(int amount)
    {
        var rotateAmount = 360 / amount;
        var currentRotateAmount = 0;
        for (int i = 0; i < amount; i++)
        {
            GameObject newBullet = Instantiate(slimeBall, transform.position, Quaternion.identity);
            newBullet.layer = 6;
            newBullet.GetComponent<Bullet>().Damage = 3;
            //
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            newBullet.transform.rotation = Quaternion.Euler(0, 0, currentRotateAmount);
            bulletRb.AddForce(newBullet.transform.up * 160 * 10);
            currentRotateAmount += rotateAmount;
        }
    }

    public void teleportBehind()
    {
        transform.position = lastPlayerPos;
        teleportEffect.Play();
        SFX.playSound("Teleport");
    }
    public void spawnChest(GameObject chest)
    {
        Instantiate(chest, Vector2.zero, Quaternion.identity);
    }
    public void getLastPos()
    {
        lastPlayerPos = Player.position;
        GameObject newEffect = Instantiate(teleportEffect.gameObject, lastPlayerPos, Quaternion.identity);
        newEffect.GetComponent<ParticleSystem>().Play();
        Destroy(newEffect, 3);
    }

    IEnumerator Burst(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSpear = Instantiate(slimeBall, shootSpot.position, quaternion.identity);
            newSpear.layer = 6;
            newSpear.GetComponent<Bullet>().Damage = 2;
            //
            Rigidbody2D bulletRb = newSpear.GetComponent<Rigidbody2D>();
            newSpear.transform.Rotate(UnityEngine.Random.Range(-60, 60), UnityEngine.Random.Range(-60, 60), 0);
            bulletRb.AddForce((Player.position - transform.position).normalized * 195 * 10);
            SFX.playSound("Shoot");
            //Effect
            GameObject newObject = Instantiate(slimeEffect, shootSpot.position, quaternion.identity);
            Destroy(newObject, 2);
            yield return new WaitForSeconds(.5f); // wait till the next round
        }

    }

}
