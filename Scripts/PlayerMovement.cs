using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    [Header("PlayerStats")]
    public float movementSpeed;
    [HideInInspector] public float baseSpeed;
    public int Health = 25;
    [HideInInspector]public int maxHealth;
    [SerializeField] float iFrams;
    [Header("Transforms")]
    [SerializeField] Transform gfx;
    public Transform rotateSpot;
    public Transform ToolSpot;
    [SerializeField] GameObject Arms;
    [SerializeField] Transform Feet;
    
    public Transform cameraFollow;
    [Header("Effects")]
    public SoundScript sfx;
    [SerializeField] ParticleSystem dashEffect;
    [SerializeField] ParticleSystem footPrints;
    float dashCooldown;
    [Header("Velocity's")]
    [SerializeField] Vector2 velocity;
    Vector2 slipVelocity;
    [Header("Bools")]
    public bool canMove = true;
    public bool isRight = true;
    public bool holdingTool;
    public bool canAttack = true;
    public bool slipping;
    [Header("Mouse")]
    [SerializeField] Transform Cursor;
    public Vector2 mousePos;
    public bool mouseSnapToGrid;
    public Animator cursorAnim;
//
    public GameObject blockPlacer;
    [Header("UI")]
    [SerializeField] GameObject deadScreen;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject testSpawn;
    [Header("Other")]
    [SerializeField] LayerMask groundLayer;

    //
    [HideInInspector] public Rigidbody2D rb;
    public Animator anim;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        instance = this;
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = movementSpeed;
        maxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            velocity.x = Input.GetAxisRaw("Horizontal");
            velocity.y = Input.GetAxisRaw("Vertical");
            if (velocity != Vector2.zero) slipVelocity = velocity;
            if(slipping)
            {
                velocity = slipVelocity;
            }
        }
        else
        {
            velocity = Vector2.zero;
        }
        MouseStuff();
        animations();

        if (Input.GetKeyDown(KeyCode.Space) & dashCooldown <= 0)
        {
            dashEffect.transform.localScale = gfx.localScale;
            StartCoroutine(knockBack(.1f, (mousePos - (Vector2)transform.position).normalized, 6000 * 20));
            dashEffect.Play();
            sfx.playSound("Dash");
            cursorAnim.Play("CursorDash");
            cursorAnim.SetFloat("DashTime", 1 / 1);
            dashCooldown = 1;
            iFrams = .8f;
            rotateSpot.rotation = Quaternion.identity;
        }

        if(iFrams > 0)
        {
            iFrams -= 1 * Time.deltaTime;
        }
        if(dashCooldown > 0)
        {
            dashCooldown -= 1 * Time.deltaTime;
        }
        //
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Break();
        }

        //Arms.SetActive(!holdingTool);


        //UI
        healthBar.transform.parent.gameObject.SetActive(Health < maxHealth);
        healthBar.fillAmount = (float)Health / (float)maxHealth;

        if (Input.GetKey(KeyCode.P))
        {
            GameObject testObject = Instantiate(testSpawn, mousePos, Quaternion.identity);
            //StartCoroutine(GunScript.Shake(.5f,1));
        }
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetKey(KeyCode.T))
        {
            transform.position = mousePos;
        }
    }

    public void RingOfBullets(int amount)
    {
        /*var rotateAmount = 360 / amount;
        var currentRotateAmount = 0;
        for (int i = 0; i < amount; i++)
        {
            GameObject newBullet = Instantiate(test, transform.position, Quaternion.identity);
            newBullet.layer = 6;
            newBullet.GetComponent<Bullet>().Damage = 3;
            //
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            newBullet.transform.rotation = Quaternion.Euler(0, 0, currentRotateAmount);
            bulletRb.AddForce(newBullet.transform.up * 160 * 10);
            currentRotateAmount += rotateAmount;
        }
        */
    }
    void MouseStuff()
    {
        //MousePos
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Cursor.position = Input.mousePosition;
        //
        if (!mouseSnapToGrid)
        {
            Cursor.gameObject.SetActive(true);
        }
        else
        {
            Cursor.gameObject.SetActive(false);
        }

        //Flipping Sprite
        if (mousePos.x < transform.position.x)
        {
            gfx.localScale = new Vector3(-1, 1, 1);
            isRight = false;
        }
        else if(mousePos.x > transform.position.x)
        {
            gfx.localScale = new Vector3(1, 1, 1);
            isRight = true;
        }

        //Rotating Arm
        Quaternion rotation = Quaternion.LookRotation(mousePos - (Vector2)rotateSpot.position, rotateSpot.TransformDirection(Vector3.up));
        rotateSpot.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        //Fix werid arm flip
        if(Input.GetMouseButtonUp(2)) rotateSpot.rotation = Quaternion.identity;
    }
    void animations()
    {
        if(velocity == Vector2.zero)
        {
            anim.SetBool("IsMoving", false);
        }
        else
        {
            anim.SetBool("IsMoving", true);
        }
        anim.SetFloat("Iframs", iFrams);



    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            rotateSpot.rotation = Quaternion.identity;
            UnityEngine.Cursor.visible = false;
        }
    }

    public void TakeDamage(int amount)
    {
        if (iFrams > 0) return;
        Health -= amount;
        
        //setting iFrams
        iFrams = 2;
        //Effects
        anim.Play("Hurt");
        sfx.playSound("Hurt");
        //Killing
        if (Health <= 0)
        {
            Health = 0;
            Destroy(gameObject);
        }
    }
    public void playSound(string soundName)
    {
        sfx.playSound(soundName);
    }


    public IEnumerator knockBack(float length, Vector2 dir, float amount)
    {
        canMove = false;
        rb.AddForce(dir * amount);
        yield return new WaitForSecondsRealtime(length);
        canMove = true;
    }
    public IEnumerator cameraShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(cameraFollow.position.x + magnitude, cameraFollow.position.x - magnitude);
            float y = Random.Range(cameraFollow.position.y + magnitude, cameraFollow.position.y - magnitude);

            cameraFollow.position = new Vector2(x, y);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        cameraFollow.localPosition = Vector2.zero;
        yield return new WaitForEndOfFrame();
        cameraFollow.localPosition = Vector2.zero;
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = velocity.normalized * movementSpeed;
        }
    }
    private void OnDestroy()
    {
        deadScreen.SetActive(true);
        UnityEngine.Cursor.visible = true;
        //SceneManager.LoadScene(0);
    }

}
