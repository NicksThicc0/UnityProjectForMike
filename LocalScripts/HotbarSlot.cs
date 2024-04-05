using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GridBrushBase;

public class HotbarSlot : InventorySlot
{
    [Header("Hotbar specs")]
    [SerializeField] KeyCode neededKey;
    public GameObject toolPrefab;

    //
    public Image _Image;


    public override void Awake()
    {
        base.Awake();
        _Image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        checkSlot();
    }

    // Update is called once per frame
    void Update()
    {
        clickKey();
    }

    void clickKey()
    {
        if (Input.GetKeyDown(neededKey))
        {
            //Image Stuff
            InventoryManager.instance.currentHotbar = this;
            _Image.sprite = InventoryManager.instance.selectedHotbar;
            //Tool Stuff
            if (toolPrefab != null)
            {
                toolPrefab.SetActive(true);
                PlayerMovement.instance.holdingTool = true;
            }
        }
        //
        if (InventoryManager.instance.currentHotbar != this)
        {
            //Image Stuff
            _Image.sprite = InventoryManager.instance.unselectedHotbar;
            //Tool Stuff
            if (toolPrefab != null)
            {
                toolPrefab.SetActive(false);
                PlayerMovement.instance.holdingTool = false;
                toolPrefab.transform.localRotation = Quaternion.identity;
            }
        }
        //
    }

    public override void NewItem(ItemScriptableObject newItem)
    {
        base.NewItem(newItem);
        if (currentItem != null & toolPrefab == null)
        {
            if (currentItem is WeaponScriptable)
            {
                //Creating The Object
                WeaponScriptable tool = (WeaponScriptable)currentItem;
                toolPrefab = Instantiate(tool.toolPrefab, PlayerMovement.instance.ToolSpot.localPosition, PlayerMovement.instance.rotateSpot.rotation);
                toolPrefab.SetActive(false);
                //Setting Postion And Rotation... again
                toolPrefab.transform.SetParent(PlayerMovement.instance.ToolSpot);
                toolPrefab.transform.localPosition = tool.toolPrefab.transform.position;
                toolPrefab.transform.localRotation = Quaternion.identity;
                //Setting Scale
                toolPrefab.transform.localScale = Vector3.one;
                if (InventoryManager.instance.currentHotbar == this) toolPrefab.SetActive(true);
            }
            if (currentItem is BlocksScriptableObject)
            {
                BlocksScriptableObject newBlock = (BlocksScriptableObject)currentItem;
                //
                toolPrefab = Instantiate(PlayerMovement.instance.blockPlacer.gameObject, Vector2.zero, Quaternion.identity);
                toolPrefab.transform.SetParent(PlayerMovement.instance.ToolSpot);
                toolPrefab.transform.localPosition = new Vector2(-0.44f, 0);
                toolPrefab.transform.localScale = Vector3.one;
                //
                toolPrefab.GetComponent<BlockPlacerClass>().currentBlock = newBlock;
                toolPrefab.GetComponent<BlockPlacerClass>().invSlot = this;
                toolPrefab.name = newBlock.name;
            }

        }
    }

    public override void checkSlot()
    {
        base.checkSlot();
        if(currentItem == null & toolPrefab != null)
        {
            Destroy(toolPrefab);
        }
        else if(currentItem != null)
        {
            if (toolPrefab == null)
            {
                if (currentItem is WeaponScriptable)
                {
                    //Creating The Object
                    WeaponScriptable tool = (WeaponScriptable)currentItem;
                    toolPrefab = Instantiate(tool.toolPrefab, PlayerMovement.instance.ToolSpot.localPosition, PlayerMovement.instance.rotateSpot.rotation);
                    toolPrefab.SetActive(false);
                    //Setting Postion And Rotation... again
                    toolPrefab.transform.SetParent(PlayerMovement.instance.ToolSpot);
                    toolPrefab.transform.localPosition = tool.toolPrefab.transform.position;
                    toolPrefab.transform.localRotation = Quaternion.identity;
                    //Setting Scale
                    toolPrefab.transform.localScale = Vector3.one;
                }
                else if (currentItem is BlocksScriptableObject)
                {
                    BlocksScriptableObject newBlock = (BlocksScriptableObject)currentItem;
                    toolPrefab = Instantiate(PlayerMovement.instance.blockPlacer, PlayerMovement.instance.ToolSpot.localPosition, PlayerMovement.instance.rotateSpot.rotation);
                    //
                    toolPrefab.GetComponent<BlockPlacerClass>().currentBlock = newBlock;
                    toolPrefab.GetComponent<BlockPlacerClass>().invSlot = this;
                    //
                    toolPrefab.transform.SetParent(PlayerMovement.instance.ToolSpot);
                    toolPrefab.transform.localPosition = new Vector2(-.44f, 0);
                    toolPrefab.transform.localRotation = Quaternion.identity;
                    toolPrefab.transform.localScale = Vector3.one;
                    //
                    PlayerMovement.instance.blockPlacer.SetActive(true);

                }
            }
            else
            {
                if (currentItem is WeaponScriptable)
                {
                    Destroy(toolPrefab);
                    //Creating The Object
                    WeaponScriptable tool = (WeaponScriptable)currentItem;
                    toolPrefab = Instantiate(tool.toolPrefab, PlayerMovement.instance.ToolSpot.localPosition, PlayerMovement.instance.rotateSpot.rotation);
                    toolPrefab.SetActive(false);
                    //Setting Postion And Rotation... again
                    toolPrefab.transform.SetParent(PlayerMovement.instance.ToolSpot);
                    toolPrefab.transform.localPosition = tool.toolPrefab.transform.position;
                    toolPrefab.transform.localRotation = Quaternion.identity;
                    //Setting Scale
                    toolPrefab.transform.localScale = Vector3.one;
                    if(InventoryManager.instance.currentHotbar == this) toolPrefab.SetActive(true);
                }
            }
        }
    }
}
