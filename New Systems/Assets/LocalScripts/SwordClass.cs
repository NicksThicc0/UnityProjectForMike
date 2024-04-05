using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordClass : MonoBehaviour
{
    [SerializeField]PlayerMovement player;
    [SerializeField] ToolType ToolType;
    [Header("Sword Stats")]
    [SerializeField] WeaponScriptable Weapon;
    [SerializeField] float attackCooldown;
    [Header("Other")]
    public SoundScript sfx;
    public List<GameObject> hitList;
    [Header("Effects")]
    [SerializeField] SpriteRenderer _spriteR;
    [SerializeField] Material whiteOut;
    [SerializeField] Material defaultMaterial;
    [Header("Animation")]
    public Animator anim;
    [SerializeField] string attackAnim = "BaseSwing";

    public virtual void Awake()
    {
        player = PlayerMovement.instance;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(attackCooldown > 0)
        {
            attackCooldown -= 1 * Time.deltaTime;
        }
        //Clicking To Attack

        if(Input.GetMouseButtonDown(0) & attackCooldown <= 0 & !Weapon.Auto & player.canAttack)
        {
            hitList.Clear();
            Attack();
            StartCoroutine(finishCoolDown());
        }
        else if(Input.GetMouseButton(0) & attackCooldown <= 0 & Weapon.Auto & player.canAttack)
        {
            hitList.Clear();
            Attack();
            StartCoroutine(finishCoolDown());
        }
        //
        player.cursorAnim.SetFloat("Speed", 1 / Weapon.attackCooldown);

    }

    public virtual void Attack()
    {
        anim.Play(attackAnim);
        player.cursorAnim.SetTrigger("Reload");
        attackCooldown = Weapon.attackCooldown;
    }

    //Play Sounds
    public void playSound(string soundName)
    {
        sfx.playSound(soundName);
    }

    //Turning white
    IEnumerator finishCoolDown()
    {
        yield return new WaitForSeconds(Weapon.attackCooldown);
        sfx.playSound("CooldownOver");
        defaultMaterial = _spriteR.material;
        _spriteR.material = whiteOut;
        yield return new WaitForSeconds(.15f);
        _spriteR.material = defaultMaterial;
    }

    private void OnDisable()
    {
        _spriteR.material = defaultMaterial;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Weapon.basePos;
    }
    private void OnEnable()
    {
        PlayerMovement.instance.rotateSpot.rotation = Quaternion.identity;
    }

    //Dealing Damage
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" & !hitList.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<EnemyClass>().TakeDamage(Weapon.Damage);
            hitList.Add(collision.gameObject);
        }
        if (collision.gameObject.tag == "Boss" & !hitList.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<BossClass>().TakeDamage(Weapon.Damage);
            hitList.Add(collision.gameObject);
        }
        if (collision.gameObject.tag == "Node" & !hitList.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<NodeClass>().TakeDamage(Weapon.Damage, Weapon);
            hitList.Add(collision.gameObject);
        }

    }

}
