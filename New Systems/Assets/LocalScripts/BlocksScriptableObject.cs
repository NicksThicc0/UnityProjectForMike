using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Items/Blocks")]
public class BlocksScriptableObject : ItemScriptableObject
{
    [Header("Block Specs")]
    public GameObject blockPrefab;
    public int placeRange;
    public float blockSize = 1;
    public bool canAutoPlace;

}
