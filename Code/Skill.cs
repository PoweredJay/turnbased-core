using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType { Bludgeoning, Piercing, Slashing, Fire, Water, Air, Light, Dark, Support }

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string desc;
    public int power;
    public int cost;
    public int tier;
    public bool Heal;
    public bool All;
    public Sprite typeSprite;
    public DamageType type;

    public void SetSkillImage(DamageType type)
    {
        
    }
}
