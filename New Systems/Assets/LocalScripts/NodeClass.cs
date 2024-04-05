using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeClass : MonoBehaviour
{
    [Header("Node Specs")]
    [SerializeField] int Health = 100;
    int maxHealth;
    [SerializeField] ToolType RequiredTool;
    [SerializeField] int neededLevel;

    [Header("Items")]
    [SerializeField] ItemDrops[] Items;
    [SerializeField] Transform dropPos;
    [SerializeField] float dropRadius = 2;
    [SerializeField] int dropsAroundNode = 5;
    [Header("Effects")]
    [SerializeField] SoundScript sfx;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] GameObject destroyEffect;
    [Header("UI")]
    [SerializeField] GameObject healthBar;
    [SerializeField] Image healthBarFill;
    //

    private void Start()
    {
        maxHealth = Health;
        SnapToGrid.Snap(transform);
    }


    int damageAmount;
    public void TakeDamage(int amount, WeaponScriptable Tool)
    {
        int rarityPlus = 0;
        if(Tool.Level >= neededLevel)
        {
            if (Tool.toolType == RequiredTool)
            {
                Health -= amount;
                damageAmount = amount;
                rarityPlus -= amount / 3;
            }
            else
            {
                Health -= amount / 3;
                damageAmount = amount / 3;
                rarityPlus += 10;
            }
        }


        //Effects
        sfx.playSound("Hit");
        if (hitEffect != null) hitEffect.Play();
        StartCoroutine(shakeObject(.1f, .2f, gameObject.transform));


        //Drop Item
        if(damageAmount > 0)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                int randomNumber = Random.Range(0, Items[i].Rarity + rarityPlus);
                if (randomNumber == 0)
                {
                    var randomDropPos = new Vector2(Random.Range(dropPos.position.x - dropRadius, dropPos.position.x + dropRadius), Random.Range(dropPos.position.y - dropRadius, dropPos.position.y + dropRadius));
                    GameObject droppedItem = Instantiate(Resources.Load<GameObject>("Item On Ground"), randomDropPos, Quaternion.identity);
                    droppedItem.GetComponent<PickUpItem>().Item = Items[i].Item;
                    Debug.Log("Drop");
                }
            }

        }
        //Healthbar
        healthBar.SetActive(Health < maxHealth);
        healthBarFill.fillAmount = (float)Health / (float)maxHealth;


        if (Health <= 0)
        {
            if (destroyEffect != null)
            {
                GameObject breakEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
                Destroy(breakEffect, 6);
            }
            Destroy(gameObject);
        }
    }



    public static IEnumerator shakeObject(float duration, float magnitude, Transform whatToShake)
    {
        float elapsed = 0f;

        Vector2 orginalPos = whatToShake.position;

        while (elapsed < duration)
        {
            float x = Random.Range(orginalPos.x + magnitude, orginalPos.x - magnitude);
            float y = Random.Range(orginalPos.y + magnitude, orginalPos.y - magnitude);

            whatToShake.position = new Vector2(x, y);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        whatToShake.position = orginalPos;
        yield return new WaitForEndOfFrame();
        whatToShake.position = orginalPos;
    }



    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(dropPos.position, dropRadius);
    }
}
