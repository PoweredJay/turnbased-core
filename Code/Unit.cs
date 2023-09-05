using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int atkStat;
    public int magStat;
    public int defStat;
    public int spdStat;

    public int maxHP;
    public int curHP;
    public int maxMP;
    public int curMP;

    public Skill skill1;
    public Skill skill2;
    public Skill skill3;
    public Skill skill4;
    public Skill skill5;
    public Skill skill6;

    public bool TakeDamage(int dmg)
    {
        curHP -= dmg;

        if (curHP <= 0)
        {
            return true;
        } else if (curHP > maxHP)
        { 
            curHP = maxHP;
            return false;
        } else
        {
            return false;
        }
    }

    public void HealDamage(int heal)
    {
        curHP += heal;

        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
    }
}
