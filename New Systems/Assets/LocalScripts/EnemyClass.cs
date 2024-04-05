using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyClass : MonoBehaviour
{
    [Header("Stats")]
    public int Health = 25;
    [HideInInspector] public int baseHealth;
    public float Speed = 10;
    float baseSpeed;
    public float stopRange = 1;
    public float loosePlayerRange = 30;
    public Transform Player;
    [Header("attackStats")]
    public int Damage = 5;
    public float attackRange = 5;
    public float attackSpeed = 3;
    public float projectileSpeed = 5;
    [SerializeField]float roamTime = 5;
    float minRoamTime;
    [HideInInspector] public float minAttackSpeed;
    [SerializeField] float attackSize = .2f;
    [Header("Other")]

    [SerializeField] LayerMask EnemyMask;
    [SerializeField] GameObject deathParticle;
    public SoundScript SFX;
    public Animator anim;
    [HideInInspector]public Rigidbody2D rb;
    [SerializeField] Transform spawnSpot;
    [HideInInspector]public NavMeshAgent agent;
    [SerializeField] bool debug;
    [SerializeField] bool inSight;
    [SerializeField] bool Agrod;
    [SerializeField] Image healthBar;
    [Header("GFX")]
    public Transform gfx;
    [SerializeField] SpriteRenderer _spriteR;
    [Header("Item Drops")]
    [SerializeField] ItemDrops[] Drops;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //
        anim = GetComponent<Animator>();
    }

    public virtual void Start()
    {

        //setting stats
        baseSpeed = Speed;
        agent.speed = Speed;
        agent.stoppingDistance = 0;
        //
        minAttackSpeed = attackSpeed;
        baseHealth = Health;
        minRoamTime = roamTime;
        if (Player == null) Player = PlayerMovement.instance.transform;
    }

    private void FixedUpdate()
    {
        //moveTowardsPlayer();
    }
    private void Update()
    {
        Animatons();
        Debug.Log("Stuff");
    }


    public virtual void moveTowardsPlayer()
    {


        if (Player == null) return;

        if (Agrod)
        {
            agent.SetDestination(Player.position);
        }
        else
        {
            //Roaming
            if(Vector2.Distance(transform.position, agent.destination) <= 1)
            {
                if(roamTime > 0)
                {
                    roamTime -= 1 * Time.deltaTime;
                }
            }
            if(roamTime <= 0)
            {
                agent.SetDestination(new Vector2(UnityEngine.Random.Range(transform.position.x - attackRange, transform.position.x + attackRange), UnityEngine.Random.Range(transform.position.y - attackRange, transform.position.y + attackRange)));
                roamTime = UnityEngine.Random.Range(0, minRoamTime);
            }
            //Flip
            if (agent.destination.x < transform.position.x)
            {
                gfx.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gfx.localScale = new Vector3(1, 1, 1);
            }
        }


        //flipping
        if (Agrod)
        {
            if (Player.position.x < transform.position.x)
            {
                gfx.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gfx.localScale = new Vector3(1, 1, 1);
            }
        }

        //Loose Player
        if (Vector2.Distance(transform.position, Player.position) >= loosePlayerRange & Agrod)
        {
            Agrod = false;
            agent.SetDestination(transform.position);
        }

        //Vision Ray
        RaycastHit2D visonRay = Physics2D.CircleCast(transform.position,attackSize, (Player.position - transform.position), attackRange, EnemyMask);
            
        if (visonRay.collider == null) return;
        if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.yellow); //Remove Soond

        if (visonRay.collider.tag == "Player")
        {
            if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.green);//Remove Soon
            inSight = true;
            Agrod = true;
        }
        else
        {
            inSight = false;
            if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.red);//Remove Soon
        }

        //Stoping Range
        if (Vector2.Distance(transform.position, Player.position) <= stopRange & inSight)
        {
            if(debug)Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.blue);//Remove Soon
            agent.velocity = Vector2.zero;
            agent.speed = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            agent.speed = baseSpeed;
        }


    }
    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        anim.Play("Hurt");
        SFX.playSound("Hurt");
        if (Health <= 0)
        {
            //Drops
            for (int i = 0; i < Drops.Length; i++)
            {
                var randomNumber = UnityEngine.Random.Range(0, Drops[i].Rarity);
                if (randomNumber == 0)
                {
                    GameObject droppedItem = Instantiate(Resources.Load<GameObject>("Item On Ground"), transform.position, quaternion.identity);
                    droppedItem.GetComponent<PickUpItem>().Item = Drops[i].Item;
                }
            }

            //DeathEffect
            if (deathParticle == null) return;
            GameObject newParticle = Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(newParticle, 5);
            //deathSound
            SFX.Source = Player.GetComponent<PlayerMovement>().sfx.Source;
            SFX.playSound("Died");

            Destroy(gameObject);
        }
    }
    public void spawnEntity(GameObject entity)
    {
        GameObject newEntity = Instantiate(entity, spawnSpot.position, spawnSpot.rotation);
        Destroy(newEntity, 10);
    }
    public virtual void Attack()
    {
        Debug.Log("Attack");
    }
    public virtual void AttackCooldown()
    {
        if (Player == null) return;
        if (Vector2.Distance(transform.position, Player.position) <= attackRange & inSight)
        {
            //CoolDowns
            if (attackSpeed > 0)
            {
                attackSpeed -= 1 * Time.deltaTime;
            }
            //Attacking / Shooting
            if (attackSpeed <= 0)
            {
                Attack();
            }

        }
    }
    public virtual void UIstuff()
    {
        healthBar.transform.parent.gameObject.SetActive(Health < baseHealth);
        if (healthBar != null) healthBar.fillAmount = (float)Health / (float)baseHealth;
    }

    public virtual void Animatons()
    {
        if ((Vector2)agent.velocity != Vector2.zero)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }
    public void playSound(string soundName)
    {
        SFX.playSound(soundName);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
        }
    }

    private void OnDestroy()
    {
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, loosePlayerRange);

    }
    //
}

[System.Serializable]
public class ItemDrops
{
    [SerializeField] string DropName;
    public int Rarity;
    public ItemScriptableObject Item;
}
