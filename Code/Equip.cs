using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipType { Weapon, Armor, Charm, None }

[CreateAssetMenu(fileName = "New Equip", menuName = "Equip")]
public class Equip : ScriptableObject
{
    public string equipName;
    public string desc;
    public int power;
    public Sprite equipSprite;
    public Skill skillGranted;
    public EquipType equipType;
    public DamageType damageType;
}
