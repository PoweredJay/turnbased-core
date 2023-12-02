using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { Consumable, Key }

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string desc;
    public int restoreAmt1;
    public int restoreAmt2;
    public Sprite itemSprite;
    public Skill skillGranted;
    public ItemType type;
}
