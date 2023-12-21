using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType 
{ 
    Bludgeoning = 0, 
    Piercing = 1, 
    Slashing = 2, 
    Fire = 3, 
    Water = 4, 
    Air = 5, 
    Light = 6,
    Dark = 7, 
    Support = 8, 
    None 
}

public enum SkillType
{
    Attack = 0,
    Heal = 1,
    Buff = 2,
    Debuff = 3,
    Ailment = 4,
    Cure = 5
}

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string desc;
    public int power;
    public int cost;
    public int costHP;
    public int tier;
    public int ATKMod;
    public int DEFMod;
    public int SPDMod;
    public SkillType SkillCategory;
    
    public bool Heal;
    public bool All;
    public bool passive;
    public Sprite typeSprite;
    public DamageType type;

    public static int DamageCalc(int atkStat, int defStat, int thingPow)
    {
        int finalDam = (int)(thingPow * ((Mathf.Pow((float)atkStat/defStat, 0.5f) + (float)atkStat/defStat)/4));
        return finalDam;
    }

    public static int HealCalc(int magStat, int healPow)
    {
        int finalHeal = (int)(healPow - ((float)healPow/(magStat*7)));
        return finalHeal;
    }

    public void SkillUseSingle(int ID, Unit atkr, Unit defr, Text dialogueText, BattleHUD playerHUD)
    {
        switch(ID)
        {   
            //Attack
            case 0:
                int incDmg = (int)(Skill.DamageCalc(atkr.magStat, defr.defStat, power) * BattleSystem.damageModCalc(atkr, defr, this));
                bool isDead = defr.TakeDamage((incDmg));
                dialogueText.text = "You cast " + skillName + ", dealing " + incDmg + " damage!";
                atkr.BothCost(cost,costHP);
                break;
            //Heal
            case 1:
                int incHeal = Skill.HealCalc(atkr.magStat, power);
                defr.HealDamage(incHeal);
                dialogueText.text = "You cast " + skillName + ", healing " + defr.ToString() + " for " + incHeal + " HP.";
                atkr.MPCost(cost);
                break;
            //Buff
            case 2:
                string outputB = "You cast " + skillName + "!\n ";
                if(ATKMod != 0)
                {
                    if(atkr.ATKStatus == 3)
                    {
                        outputB += "ATK can't go any higher! ";
                    } else
                    {
                        outputB += "ATK increased by " + ATKMod + " stage";
                        if(ATKMod > 1)
                        {
                            outputB += "s";
                        }
                        outputB += ". ";
                    }
                }
                if(DEFMod != 0)
                {
                    if(atkr.DEFStatus == 3)
                    {
                        outputB += "DEF can't go any higher! ";
                    } else
                    {
                        outputB += "DEF increased by " + DEFMod + " stage";
                        if(DEFMod > 1)
                        {
                            outputB += "s";
                        }
                        outputB += ". ";
                    }
                }
                if(SPDMod != 0)
                {
                    if(atkr.SPDStatus == 3)
                    {
                        outputB += "SPD can't go any higher!";
                    } else
                    {
                        outputB += "SPD increased by " + SPDMod + " stage";
                        if(SPDMod > 1)
                        {
                            outputB += "s";
                        }
                        outputB += ".";
                    }                    
                }
                atkr.ModChange(ATKMod, DEFMod, SPDMod);
                dialogueText.text = outputB;
                atkr.MPCost(cost);
                break;
            //Debuff
            case 3:
                string outputD = "You cast " + skillName + "!\n ";
                if(ATKMod != 0)
                {
                    if(defr.ATKStatus == -3)
                    {
                        outputD += "ATK can't go any lower! ";
                    } else
                    {
                        outputD += "ATK decreased by " + (ATKMod*-1) + " stage";
                        if(ATKMod < -1)
                        {
                            outputD += "s";
                        }
                        outputD += ". ";
                    }
                }
                if(DEFMod != 0)
                {
                    if(defr.DEFStatus == -3)
                    {
                        outputD += "DEF can't go any lower! ";
                    } else
                    {
                        outputD += "DEF decreased by " + (DEFMod*-1) + " stage";
                        if(DEFMod < -1)
                        {
                            outputD += "s";
                        }
                        outputD += ". ";
                    }
                }
                if(SPDMod != 0)
                {
                    if(defr.SPDStatus == -3)
                    {
                        outputD += "SPD can't go any lower!";
                    } else
                    {
                        outputD += "SPD decreased by " + (SPDMod*-1) + " stage";
                        if(SPDMod < -1)
                        {
                            outputD += "s";
                        }
                        outputD += ".";
                    }              
                }
                defr.ModChange(ATKMod, DEFMod, SPDMod);
                dialogueText.text = outputD;
                atkr.MPCost(cost);
                break;
            //Ailment
            case 4:
                break;
            //Cure
            case 5:
                break;
        }
        playerHUD.UpdateHUD(atkr);
    }
    public void SkillUseAll(int ID, Unit atkr, List<Unit> allyList, List<Unit> enemyList, Text dialogueText, List<BattleHUD>FriendlyHUDList)
    {
        switch(ID)
        {   
            //Attack
            case 0:
                foreach(Unit recvr in enemyList)
                {
                    int incDmg = (int)(Skill.DamageCalc(atkr.magStat, recvr.defStat, power) * BattleSystem.damageModCalc(atkr, recvr, this));
                    bool isDead = recvr.TakeDamage((incDmg));
                }
                dialogueText.text = "You cast " + skillName + "!";
                atkr.BothCost(cost,costHP);
                break;
            //Heal
            case 1:
                int incHeal = Skill.HealCalc(atkr.magStat, power);
                foreach(Unit recvr in allyList)
                {
                    recvr.HealDamage(incHeal);
                }
                dialogueText.text = "You cast " + skillName + ", healing " + incHeal + " HP to all allies.";
                atkr.MPCost(cost);
                break;
            //Buff
            case 2:
                foreach(Unit recvr in allyList)
                {
                    recvr.ModChange(ATKMod, DEFMod, SPDMod);
                }
                string outputB = "You cast " + skillName + "!\n ";
                if(ATKMod != 0)
                {
                    outputB += "Party ATK increased by " + ATKMod + " stage. ";
                }
                if(DEFMod != 0)
                {
                    outputB += "Party DEF increased by " + DEFMod + " stage. ";
                }
                if(SPDMod != 0)
                {
                    outputB += "Party SPD increased by " + SPDMod + " stage.";                    
                }
                dialogueText.text = outputB;
                atkr.MPCost(cost);
                break;
            //Debuff
            case 3:
                foreach(Unit recvr in enemyList)
                {
                    recvr.ModChange(ATKMod, DEFMod, SPDMod);
                }
                string outputD = "You cast " + skillName + "!\n ";
                if(ATKMod != 0)
                {
                    outputD += "Enemy ATK decreased by " + ATKMod*-1 + " stage. ";
                }
                if(DEFMod != 0)
                {
                    outputD += "Enemy DEF decreased by " + DEFMod*-1 + " stage. ";
                } 
                if(SPDMod != 0)
                {
                    outputD += "Enemy SPD decreased by " + SPDMod*-1 + " stage.";                    
                }
                dialogueText.text = outputD;
                atkr.MPCost(cost);
                break;
            //Ailment
            case 4:
                break;
            //Cure
            case 5:
                break;
        }
        foreach(BattleHUD HUD in FriendlyHUDList)
        {
            foreach(Unit friend in allyList)
            {
                HUD.UpdateHUD(friend);
            }
        }
        // foreach(BattleHUD HUD in EnemyHUDList)
        // {
        //     foreach(Unit bad in enemyList)
        //     {
        //         HUD.UpdateHUD(bad);
        //     }
        // }
    }
}
