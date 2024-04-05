using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour
{
    public enum fireType { Semi,Full,Burst,Spread}
    public enum gunType { Pistol, Ar}

    [Header("Gun Stats")]
    [SerializeField] fireType ShootMode;
    [SerializeField] gunType whatGun;
    [SerializeField] float FireRate;
    [SerializeField] bool canShoot = true;
    [SerializeField] int bulletSpeed = 160;
    [SerializeField] int Damage = 1;
    [Header("Extra Stats")]
    [SerializeField] int amountOfBullets = 2;
    [SerializeField] float burstSpeed;
    [SerializeField] float spreadAmount = 10;
    [SerializeField] float bulletLife = 10;
    [SerializeField] Vector2 cameraShake = new Vector2(.1f, .1f);
    float shootCooldown;
    float baseSpread;

    [Header("Ammo")]
    [SerializeField] GameObject bulletType;
    public int Ammo = 16;
    [SerializeField] float reloadTime = 3;
    [SerializeField] bool canReload = true;
    [HideInInspector]public int maxAmmo = 16;

    [Header("Gun Parts")]
    [SerializeField] Transform shootSpot;
    [SerializeField] Transform Lazer;
    [Header("Sprites")]
    [SerializeField] Material whiteOut;
    [SerializeField] SpriteRenderer _spriteR;
    [Header("Other")]
    [SerializeField] SoundScript sounds;
    [SerializeField] Animator anim;
    [SerializeField] PlayerMovement player;
    [SerializeField] GameObject Mag;
    [SerializeField] Camera Camera;
    [SerializeField] ParticleSystem muzzleFlash;





    //
    private void Awake()
    {
        maxAmmo = Ammo;
        player = GetComponentInParent<PlayerMovement>();
        Camera = Camera.main;
    }
    void Start()
    {
        if(whatGun == gunType.Pistol)
        {
            anim.SetFloat("Speed", .3f / FireRate * 2);
            Debug.Log("pistol");
        }
        else if(whatGun == gunType.Ar)
        {
            anim.SetFloat("Speed", .1f / FireRate * 2);
        }
        baseSpread = spreadAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)& canReload & Ammo != maxAmmo)
        {
            StartCoroutine(Reload());
        }


        Shoot();

    }



    void Shoot()
    {
        if (Input.GetMouseButtonDown(0)& shootCooldown <= 0 & Ammo >= 1 & canShoot)
        {
            if (ShootMode == fireType.Semi)
            {
                Bullet();
            }
            else if (ShootMode == fireType.Burst)
            {
                StartCoroutine(Burst());
            }
            else if (ShootMode == fireType.Spread)
            {
                Bullet();
            }
        }
        else if (Input.GetMouseButton(0) & shootCooldown <= 0 & Ammo >= 1 & canShoot)
        {
            if (ShootMode == fireType.Full)
            {
                Bullet();
            }
        }

        if(Input.GetMouseButtonDown(0) & Ammo <= 0 & canReload)
        {
            sounds.playSound("OutOfAmmo");
            StartCoroutine(Reload());
        }

        //FireRate
        if(shootCooldown >= 0)
        {
            shootCooldown -= 1 * Time.deltaTime;
        }
    }

    void Bullet()
    {
        Ammo--;
        sounds.playSound("Shoot");
        //Gun Anim
        if (whatGun == gunType.Pistol)
        {
            //anim.Play("Pistol Shoot");
        }
        else if (whatGun == gunType.Ar)
        {
            //anim.Play("Ar Shoot");
        }
        //
        shootCooldown = FireRate;
        if (ShootMode != fireType.Spread)
        {
            GameObject spawntBullet = Instantiate(bulletType, shootSpot.position, shootSpot.rotation);
            Rigidbody2D bulletRb = spawntBullet.GetComponent<Rigidbody2D>();
            spawntBullet.transform.Rotate(Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount), 0);
            bulletRb.AddForce(spawntBullet.transform.up * bulletSpeed * 10);
            Destroy(spawntBullet, bulletLife);
            //
            spawntBullet.GetComponent<Bullet>().Damage = Damage;
            spawntBullet.GetComponent<Bullet>().sfx.Source = sounds.Source;
        }
        else
        {
            for (int i = 0; i < amountOfBullets -1; i++)
            {
                GameObject spawntBullet = Instantiate(bulletType, shootSpot.position, shootSpot.rotation);
                Rigidbody2D bulletRb = spawntBullet.GetComponent<Rigidbody2D>();
                spawntBullet.transform.Rotate(Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount), 0);
                bulletRb.AddForce(spawntBullet.transform.up * bulletSpeed * 10);
                Destroy(spawntBullet, bulletLife);
                //
                spawntBullet.GetComponent<Bullet>().Damage = Damage;

            }
        }
        //checking ammo
        if(Ammo == 0 & Mag != null)
        {
            Mag.SetActive(false);
        }
        StartCoroutine(Shake(cameraShake.x, cameraShake.y));

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        /*pushBack
        if(pushBack != 0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(player.knockBack(.2f, (Vector2)player.transform.position - mousePos, pushBack));
        }*/
    }


    public static IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        yield return new WaitForEndOfFrame();
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }

    IEnumerator Reload()
    {
        canShoot = false;
        canReload = false;
        player.cursorAnim.SetFloat("Speed", 1/ reloadTime);
        player.cursorAnim.Play("Reload");
        

        yield return new WaitForSecondsRealtime(reloadTime);

        //Adding the ammo
        Ammo = maxAmmo;
        //

        if (Mag != null)
        {
            Mag.SetActive(true);
        }
        canShoot = true;
        canReload = true;
        //
        var oldMaterail = _spriteR.material;
        _spriteR.material = whiteOut;
        sounds.playSound("Reloaded");
        yield return new WaitForSeconds(.15f);
        _spriteR.material = oldMaterail;

    }
    IEnumerator Burst()
    {
        canShoot = false;
        for (int i = 0; i < amountOfBullets; i++)
        {
            if (Ammo >= 1)
            {
                Bullet();
            }
            if (i == amountOfBullets - 1)
            {
                canShoot = true;
            }
            yield return new WaitForSeconds(burstSpeed); // wait till the next round
        }

    }

}
