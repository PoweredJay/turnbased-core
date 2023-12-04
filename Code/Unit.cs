using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ActionType 
{
    Attack = 0,
    Skill = 1,
    Leader = 2,
    Team = 3,
    Guard = 4,
    Move = 5,
    Item = 6,
    Flee = 7
}
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
    public bool leader;
    public bool down = false;
    public bool flow = false;
    public bool guard = false;

    public int EXP;
    public Equip weapon;
    public Equip armor;
    public Equip charm;

    //Affinities
    public int bludgeoningAffinity;
    public int piercingAffinity;
    public int slashingAffinity;
    public int fireAffinity;
    public int waterAffinity;
    public int airAffinity;
    public int lightAffinity;
    public int darkAffinity;
    /*
        The Affinities set of values keeps track of the affinities of a unit to all 8 damage types. 
        Each element in the array corresponds to the affinity to its respective type.
        The value of an element corresponds to the affinity towards the damage type itself.
        A 0 is neutral. 1 is resistance (50% damage). 2 is immunity (0% damage). -1 is weakness (150% damage and attacking unit gains Flow state).
        For example, if fireAffinity = 2, then the unit is immune to fire damage.
    */

    //CombatStatus
    public int ATKStatus;
    public int DEFStatus;
    public int SPDStatus;
    /*
        Similar to the Affinities, the CombatStatus set of values keeps track of the combat status (buff/debuff) of the 3 stats.
        Each element corresponds to ATK/DEF/SPD respectively. 
        The value of an element corresponds to the status itself.
        The value ranges from -3 to 3, with a -3 being the most lowered a stat can become and a 3 being the most increased a stat can become.
        -3 is heavily reduced (-60%). -2 is moderately reduced (-40%). -1 is lightly reduced (-20%).
        0 is neutral.
        1 is lightly increased (+20%). 2 is moderately increased (+40%). 3 is heavily increased (+60%).
        For example, if DEFStatus = -2, then the DEF of that unit is reduced by 40% (moderately reduced).
    */
    public Skill[] Skills = new Skill[9];
    public List<Skill> SkillListActive;
    public List<Skill> SkillListPassive;
    public Status Ailment;
    public ActionType action;
    void Start()
    {
        foreach(Skill sk in Skills)
        {
            if(sk.passive)
            {
                SkillListPassive.Add(sk);
            } else
            {
                SkillListActive.Add(sk);
            }
        }
    }
    public static int DamageCalc(int aStat, int dStat, int thingPow)
    {
        int finalDam = (int)(thingPow * ((Mathf.Pow((float)aStat/dStat, 0.5f) + (float)aStat/dStat)/4));
        return finalDam;
    }

    public static int HealCalc(int mStat, int hPow)
    {
        int finalHeal = (int)(hPow - ((float)hPow/mStat));
        return finalHeal;
    }

    public bool TakeDamage(int dmg)
    {
        curHP -= (dmg);

        if (curHP <= 0)
        {
            return true;
        } else
        {
            return false;
        }
        guard = false;
    }

    public void HealDamage(int heal)
    {
        curHP += heal;

        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
    }

    public void MPCost(int mp)
    {
        curMP -= mp;
        if (curMP < 0)
        {
            curMP = 0;
        }
    }
    public void ModChange(int aMod, int dMod, int sMod)
    {
        ATKStatus += aMod;
        if(ATKStatus < -3)
        {
            ATKStatus = -3;
        }
        if(ATKStatus > 3)
        {
            ATKStatus = 3;
        }
        DEFStatus += dMod;
        if(DEFStatus < -3)
        {
            DEFStatus = -3;
        }
        if(DEFStatus > 3)
        {
            DEFStatus = 3;
        }
        SPDStatus += sMod;
        if(SPDStatus < -3)
        {
            SPDStatus = -3;
        }
        if(SPDStatus > 3)
        {
            SPDStatus = 3;
        }
    }

    public int GetAffinity(int aff)
    {
        switch(aff)
        {
            case 0:
                return bludgeoningAffinity;
            case 1:
                return piercingAffinity;
            case 2:
                return slashingAffinity;
            case 3:
                return fireAffinity;
            case 4:
                return waterAffinity;
            case 5:
                return airAffinity;
            case 6:
                return lightAffinity;
            case 7:
                return darkAffinity;
        }
        return -1;
        
    }
    public override string ToString()
    {
        return unitName;
    }
}
