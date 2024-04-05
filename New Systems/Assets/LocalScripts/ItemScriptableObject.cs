using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemScriptableObject : ScriptableObject
{
    [Header("Item Specs")]
    public Sprite itemIcon;
    public int maxStackAmount = 99;
    [TextArea(5,10)]
    public string toolTip;
    [Header("Smelting")]
    public ItemScriptableObject smeltedInto;
    public float timeToSmelt;
    public float amountOfFuel;

}
