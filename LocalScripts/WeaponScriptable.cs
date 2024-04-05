using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType { Sword, Axe, Pickaxe }
[CreateAssetMenu(fileName = "New Tool", menuName = "Items/Tool")]
public class WeaponScriptable : ItemScriptableObject
{
    [Header("Tool Specs")]
    public ToolType toolType;
    [Header("Stats")]
    public int Damage = 3;
    public float attackCooldown = 2;
    public int Level;
    public bool Auto;

    [Header("In Game Object")]
    public GameObject toolPrefab;
    public Vector2 basePos = new Vector2(-.44f, 0);

}
