using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacerClass : MonoBehaviour
{
    public BlocksScriptableObject currentBlock;
    public HotbarSlot invSlot;

    [SerializeField] int mouseDistance;
    [SerializeField] Animator anim;
    [Header("Sprites")]
    [SerializeField] SpriteRenderer ghostBlock;
    [SerializeField] SpriteRenderer holdingBlock;
    [Header("Colors")]
    [SerializeField] Color canPlace;
    [SerializeField] Color cantPlace;
    [Header("Settings")]
    [SerializeField] LayerMask cantPlaceLayer;

    bool alreadyPlaced;

    PlayerMovement player;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        anim = GetComponent<Animator>();
        invSlot.toolPrefab = gameObject;
        transform.localPosition = new Vector2(-0.44f, 0);
        transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        mouseDistance = (int)Vector2.Distance(transform.position, player.mousePos);
        player.mouseSnapToGrid = true;

        if (currentBlock != null)
        {
            ghostBlock.sprite = currentBlock.itemIcon;
            holdingBlock.sprite = currentBlock.itemIcon;
            ghostBlock.transform.position = new Vector2((int)player.mousePos.x, (int)player.mousePos.y);
            ghostBlock.transform.rotation = Quaternion.identity;
            if (player.isRight)
            {
                ghostBlock.flipX = false;
            }
            else
            {
                ghostBlock.flipX = true;
            }
        }
        if(mouseDistance <= currentBlock.placeRange)
        {

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(ghostBlock.transform.position, currentBlock.blockSize, cantPlaceLayer);
            ghostBlock.color = cantPlace;
            if (hitColliders.Length > 0) return;
            ghostBlock.color = canPlace;
            if (Input.GetMouseButtonDown(0) & player.canAttack)
            {

                //anim.Play("BaseSwing");
                invSlot.amountInSlot--;
                Instantiate(currentBlock.blockPrefab, ghostBlock.transform.position, Quaternion.identity);
                invSlot.checkSlot();
                //StartCoroutine(player.cameraShake(.3f,.1f));
            }
        }
        else
        {
            ghostBlock.color = cantPlace;
        }
    }

    private void OnDisable()
    {
        player.mouseSnapToGrid = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(player.mousePos, currentBlock.blockSize);
    }
}
