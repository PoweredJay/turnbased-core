using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public enum BattleState { START, PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, PLAYERTURN4, PLAYERTURN5, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerStation;
    public Transform enemyStation;

    public Transform grid11;
    public Transform grid12;
    public Transform grid13;
    public Transform grid21;
    public Transform grid22;
    public Transform grid23;
    public Transform grid31;
    public Transform grid32;
    public Transform grid33;
    /*
    The grid transforms are nine distinct points where you can place units in a 3x3 square, with 
    coordinates corresponding to the grid below.

    11 12 13
    21 22 23
    31 32 33
    Will be implemented in the future.
    */

    public BattleHUD playerHUD1;
    public BattleHUD playerHUD2;
    public BattleHUD playerHUD3;
    public BattleHUD playerHUD4;
    public BattleHUD playerHUD5;

    public SkillHUD skillHUD1;
    public SkillHUD skillHUD2;
    public SkillHUD skillHUD3;
    public SkillHUD skillHUD4;
    public SkillHUD skillHUD5;
    public SkillHUD skillHUD6;
    public SkillHUD skillHUD7;
    public SkillHUD skillHUD8;

    public Text dialogueText;

    public GameObject ActionMenu;

    public GameObject SkillMenu;
    private AudioSource battleMusic;

    Unit playerUnit;
    Unit enemyUnit;
    public BattleState state;
    GameObject lastSelect;
    public static Unit curPlayerUnit;
    public Unit selectedEnemyUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    public List<Unit> UnitList;
    void Start()
    {
        state = BattleState.START;
        Cursor.visible = false;
        battleMusic = GetComponent<AudioSource>();
        battleMusic.Play();
        battleMusic.loop = true;
        StartCoroutine(SetupBattle());
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(1).gameObject);
        lastSelect = new GameObject();
    }

    void Update()
    {
        Cursor.visible = false;
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelect);
        }
        else
        {
            lastSelect = EventSystem.current.currentSelectedGameObject;
        }
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStation);
        curPlayerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        dialogueText.text = "You find yourself face to face with " + enemyUnit.unitName;

        playerHUD1.SetHUD(curPlayerUnit);
        ActionMenu.SetActive(false);
        SkillMenu.SetActive(false);
        AllyUnitList.Add(curPlayerUnit);
        EnemyUnitList.Add(enemyUnit);
        UnitList.AddRange(AllyUnitList);
        UnitList.AddRange(EnemyUnitList);
        //List<Unit> UnitListSPD = UnitList.OrderByDescending(unit=>unit.spdStat).ToList();


        yield return new WaitForSeconds(2.5f);

        state = BattleState.PLAYERTURN1;
        playerTurn();
    }

    void playerTurn()
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(1).gameObject);
        dialogueText.text = "What would you like to do?";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN1)
        {
            return;
        }
        
        StartCoroutine(PlayerAttack());
    }
    public void OnSkillButton()
    {
        ActionMenu.SetActive(false);
        SkillMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SkillMenu.transform.GetChild(0).gameObject);
        skillHUD1.SetSkillHUD(curPlayerUnit, skillHUD1.buttonID);
        skillHUD2.skillName.text = skillHUD2.whatAmI(curPlayerUnit, skillHUD2.buttonID);
        
    }

    public void OnSkillUse()
    {
        if (state != BattleState.PLAYERTURN1)
        {
            return;
        }
        GameObject skillButton = EventSystem.current.currentSelectedGameObject;
        SkillHUD skillCaller = skillButton.gameObject.GetComponent<SkillHUD>();
        if(curPlayerUnit.curMP < curPlayerUnit.Skills[skillCaller.buttonID].cost)
        {
            dialogueText.text = "Not enough MP to use " + curPlayerUnit.Skills[skillCaller.buttonID].skillName + ".";
        } else
        {
            StartCoroutine(SkillUsage(skillCaller.buttonID));
        }
        
    }

    public void OnBackButton()
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(1).gameObject);
    }
    public void OnGuardButton()
    {
        if(state != BattleState.PLAYERTURN1)
        {
            return;
        }
        curPlayerUnit.guard = true;
        dialogueText.text = curPlayerUnit.unitName + " is guarding for the next attack.";
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }
    public static double damageModCalc(Unit atkr, Unit defr)
    {
        double damageMod = (1.0 + 0.2*atkr.ATKStatus) * (1.0 - 0.2*defr.DEFStatus);
        int defrAff = defr.GetAffinity((int)atkr.weapon.damageType);
        if(atkr.flow)
        {
            damageMod *= 1.1;
        }
        if(defr.guard)
        {
            damageMod *= 0.6 * Math.Min(1.0 - 0.5*defrAff, 1.0);
        } else
        {
            damageMod *= (1.0 - 0.5*defrAff);
            if(defrAff == -1)
            {
                atkr.flow = true;
            }
        }
        return damageMod;
    }
    public static double damageModCalc(Unit atkr, Unit defr, Skill skill)
    {
        double damageMod = (1.0 + 0.2*atkr.ATKStatus) * (1.0 - 0.2*defr.DEFStatus);
        int defrAff = defr.GetAffinity((int)skill.type);
        if(atkr.flow)
        {
            damageMod *= 1.1;
        }
        if(defr.guard)
        {
            damageMod *= 0.6 * Math.Min(1.0 - 0.5*defrAff, 1.0);
        } else
        {
            damageMod *= (1.0 - 0.5*defrAff);
            if(defrAff == -1)
            {
                atkr.flow = true;
            }
        }
        return damageMod;
    }
    

    IEnumerator PlayerAttack()
    {
        int incDmg = (int)(Unit.DamageCalc(curPlayerUnit.atkStat, enemyUnit.defStat, curPlayerUnit.weapon.power) * damageModCalc(curPlayerUnit, enemyUnit));
        bool isDead = enemyUnit.TakeDamage((int)(incDmg));

        dialogueText.text = "You attack, dealing " + incDmg + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        } else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator SkillUsage(int skillNum)
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        Skill curSkill = curPlayerUnit.Skills[skillNum];
        if(curSkill.All)
        {
            curSkill.SkillUseAll((int)curSkill.SkillCategory, curPlayerUnit, AllyUnitList, EnemyUnitList, dialogueText);
        } else
        {
            curSkill.SkillUseSingle((int)curSkill.SkillCategory, curPlayerUnit, enemyUnit, dialogueText, playerHUD1);
        }
        yield return new WaitForSeconds(2f);
        //Update HUDs
            if (enemyUnit.curHP <= 0)
            {
                state = BattleState.WON;
                EndBattle();
            } else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        
        //
    }

    IEnumerator EnemyTurn()
    {
        EventSystem.current.SetSelectedGameObject(null);
        int incDmg = (int)(Unit.DamageCalc(enemyUnit.atkStat, curPlayerUnit.defStat, enemyUnit.weapon.power) * damageModCalc(enemyUnit, curPlayerUnit));
        dialogueText.text = enemyUnit.unitName + " attacks, dealing " + incDmg + " damage.";

        bool isDead = curPlayerUnit.TakeDamage((int)(incDmg));

        playerHUD1.SetHP(curPlayerUnit.curHP);

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            state = BattleState.PLAYERTURN1;
            playerTurn();
        }
    }

}
