using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossClass : MonoBehaviour
{
    public Transform Player;
    [Header("Boss Stats")]
    public int Health = 250;
    [HideInInspector]public int maxHealth;
    public bool canHurt = true;
    public int Speed = 10;
    int baseSpeed;
    public float stopRange = 1;
    [Header("attackStats")]
    [SerializeField] bool inSight;
    public int Damage = 5;
    public float attackRange = 5;
    public float attackSpeed = 3;
    public float projectileSpeed = 5;
    [HideInInspector] public float minAttackSpeed;
    [SerializeField] float attackSize = .2f;
    [Header("Other")]
    public Transform gfx;
    [SerializeField] LayerMask EnemyMask;
    [SerializeField] GameObject deathParticle;
    public SoundScript SFX;
    public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public NavMeshAgent agent;
    [SerializeField] bool debug;
    [Header("Item Drops")]
    [SerializeField] ItemDrops[] Drops;
    [Header("UI")]
    [SerializeField] Image Icon;
    Image healthBar;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //
        anim = GetComponent<Animator>();
        //
    }

    public virtual void Start()
    {

        //setting stats
        baseSpeed = Speed;
        agent.speed = Speed;
        agent.stoppingDistance = 0;
        //
        minAttackSpeed = attackSpeed;
        maxHealth = Health;
        //
        EnemySpawner.Instance.bossIcon = Icon;
        EnemySpawner.Instance.bossUi.SetActive(true);
        healthBar = EnemySpawner.Instance.bossFillBar;
        //
        Player = PlayerMovement.instance.transform;
    }

    private void FixedUpdate()
    {
        moveTowardsPlayer();
    }
    private void Update()
    {
        Animatons();
        UIstuff();
        Debug.Log("Stuff");
    }


    public virtual void moveTowardsPlayer()
    {
        if (Player == null) return;
        agent.SetDestination(Player.position);

        //flipping
        if(Health > 0)
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
        //
        RaycastHit2D visonRay = Physics2D.CircleCast(transform.position, attackSize, (Player.position - transform.position), attackRange, EnemyMask);

        if (visonRay.collider == null) return;
        if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.yellow); //Remove Soond


        if (visonRay.collider.tag == "Player")
        {
            if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.green);//Remove Soon
            inSight = true;
        }
        else
        {
            inSight = false;
            if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.red);//Remove Soon
        }

        if (Vector2.Distance(transform.position, Player.position) <= stopRange & inSight)
        {
            if (debug) Debug.DrawRay(transform.position, (Player.position - transform.position).normalized * attackRange, Color.blue);//Remove Soon
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
        if (!canHurt) return;
        Health -= amount;
        anim.Play("Hurt");
        SFX.playSound("Hurt");
        if(Health <= 0)
        {
            healthBar.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < Drops.Length; i++)
            {
                var randomNumber = UnityEngine.Random.Range(0, Drops[i].Rarity);
                if (randomNumber == 0)
                {
                    Instantiate(Drops[i].Item, transform.position, Quaternion.identity);
                    return;
                }
            }
        }
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
        if (healthBar != null) healthBar.fillAmount = (float)Health / (float)maxHealth;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
        }
    }

    private void OnDestroy()
    {

    }

    public virtual void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackSize);

    }
}
