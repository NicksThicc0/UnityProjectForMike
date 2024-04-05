using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BlockLogicClass : MonoBehaviour
{
    bool[] blockChecks = new bool[4];
    Vector2[] rays = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0) };

    [SerializeField] blockLogic[] Logic;
    [SerializeField] LayerMask blockLayer;

    [SerializeField] SpriteRenderer _SpriteR;
    [SerializeField] BoxCollider2D collisions;
    [SerializeField] GameObject checker;

    [SerializeField] GameObject Shadow;

    [SerializeField] string blockCode;


    private void Start()
    {
        SnapToGrid.Snap(transform);
        CheckBlocks();
        checkBlocksAround();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            CheckBlocks();
        }
        //
        for (int i = 0; i < blockChecks.Length; i++)
        {

            //
            UnityEngine.Debug.DrawRay(checker.transform.position, rays[i] * 1, Color.yellow);
            //
        }
    }

    public void updateBlock()
    {
         UnityEngine.Debug.Log("Update");

        for (int i = 0; i < Logic.Length; i++)
        {
            UnityEngine.Debug.Log("Checking");

            bool hasTheRightLogic = checkingLogicChecks(blockChecks, Logic[i].logicChecks);

            if (hasTheRightLogic)
            {
                _SpriteR.sprite = Logic[i].blockSprite;

                if(collisions != null)
                {
                    collisions.size = Logic[i].BlockCollisions;
                    collisions.offset = new Vector2(0, Logic[i].blockOffset);
                    collisions.enabled = Logic[i].hasCollisions;
                }


                if(Shadow != null) Shadow.SetActive(Logic[i].hasShadow);


                return;
            }
        }
    }

    public void CheckBlocks()
    {
        for (int i = 0; i < blockChecks.Length; i++)
        {
           
            //
            RaycastHit2D logicRay = Physics2D.Raycast(checker.transform.position, rays[i], 1f, blockLayer);


            if(logicRay.collider == null)
            {
                blockChecks[i] = false;
            }
            if (logicRay.collider != null)
            {
                if (logicRay.collider.gameObject == checker) break;
                if(logicRay.collider.GetComponentInParent<BlockLogicClass>().blockCode == blockCode)
                {
                      blockChecks[i] = true;
                }
            }
            //
        }
        updateBlock();
    }

    public void checkBlocksAround()
    {
        Collider2D[] test = Physics2D.OverlapCircleAll(transform.position, 3, blockLayer);

        for (int i = 0; i < test.Length; i++)
        {
            test[i].GetComponentInParent<BlockLogicClass>().CheckBlocks();
        }

    }



    bool checkingLogicChecks(bool[] array1, bool[] array2)
    {
        // Check if arrays have the same length
        if (array1.Length != array2.Length)
        {
            return false;
        }

        // Compare each element in the arrays
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                return false;
            }
        }

        // If all elements are the same, arrays are equal
        return true;
    }

}

[System.Serializable]
public class blockLogic
{
    [SerializeField] string name;

    [Header("Sprite")]
    public Sprite blockSprite;

    [Header("Bools")]
    public bool[] logicChecks = new bool[4];
    //0 is Up 
    //1 is Down
    //2 is Left
    //3 is Right

    [Header("Settings")]
    public bool hasShadow;
    public bool hasCollisions = true;

    public float blockOffset = -1;
    public Vector2 BlockCollisions;

}