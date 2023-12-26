using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum BattleState { START, PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, PLAYERTURN4, PLAYERTURN5, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    public GameObject playerPrefab5;
    public GameObject enemyPrefab;
    public Button skillButtonPrefab;
    public GameObject cursorPrefab;
    public GameObject skillButtonPanel;

    public Transform playerStation;
    public Transform playerStation2;
    public Transform playerStation3;
    public Transform playerStation4;
    public Transform playerStation5;
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
    public Text costText;
    public GameObject typeSprite;

    public GameObject ActionMenu;

    public GameObject SkillMenu;
    private AudioSource battleMusic;
    Unit enemyUnit;
    Unit enemyUnit2;
    public BattleState state;
    public GameObject lastSelect;
    public static Unit curPlayerUnit;
    Unit PlayerUnit1;
    Unit PlayerUnit2;
    Unit PlayerUnit3;
    Unit PlayerUnit4;
    Unit PlayerUnit5;
    public Unit selectedEnemyUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    public List<Unit> UnitList;
    public List<BattleHUD> PlayerHUDList;
    public List<BattleHUD> EnemyHUDList;
    public SelectionSystem selectSystem;
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
        if (EventSystem.current.currentSelectedGameObject == null && !(selectSystem.selectionEnemy == true || selectSystem.selectionPlayer == true))
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
        GameObject playerGO1 = Instantiate(playerPrefab, playerStation);
        curPlayerUnit = playerGO1.GetComponent<Unit>();
        PlayerUnit1 = curPlayerUnit;
        PlayerUnit1.SetPlayerHUD(playerHUD1);
        // GameObject playerGO2 = Instantiate(playerPrefab2, playerStation2);
        // PlayerUnit2 = playerGO2.GetComponent<Unit>();
        // GameObject playerGO3 = Instantiate(playerPrefab3, playerStation3);
        // PlayerUnit3 = playerGO3.GetComponent<Unit>();
        // GameObject playerGO4 = Instantiate(playerPrefab4, playerStation4);
        // PlayerUnit4 = playerGO4.GetComponent<Unit>();
        // GameObject playerGO5 = Instantiate(playerPrefab5, playerStation5);
        // PlayerUnit5 = playerGO5.GetComponent<Unit>();

        GameObject enemyGO1 = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGO1.GetComponent<Unit>();
        GameObject enemyGO2 = Instantiate(enemyPrefab, playerStation);
        enemyUnit2 = enemyGO2.GetComponent<Unit>();
        AllyUnitList.Add(PlayerUnit1);
        // AllyUnitList.Add(PlayerUnit2);
        // AllyUnitList.Add(PlayerUnit3);
        // AllyUnitList.Add(PlayerUnit4);
        // AllyUnitList.Add(PlayerUnit5);
        EnemyUnitList.Add(enemyUnit);
        EnemyUnitList.Add(enemyUnit2);
        UnitList.AddRange(AllyUnitList);
        UnitList.AddRange(EnemyUnitList);
        PlayerHUDList.Add(playerHUD1);
        // PlayerHUDList.Add(playerHUD2);
        // PlayerHUDList.Add(playerHUD3);
        // PlayerHUDList.Add(playerHUD4);
        // PlayerHUDList.Add(playerHUD5);
        playerHUD1.SetHUD(curPlayerUnit);
        dialogueText.text = "You find yourself face to face with " + enemyUnit.unitName + ".";
        ActionMenu.SetActive(false);
        SkillMenu.SetActive(false);
        //List<Unit> UnitListSPD = UnitList.OrderByDescending(unit=>unit.spdStat).ToList();


        yield return new WaitForSeconds(2.5f);

        state = BattleState.PLAYERTURN1;
        playerTurn();
    }

    public void playerTurn()
    {
        for (int i = SkillMenu.transform.GetChild(0).childCount - 1; i >= 0; i--)
        {
            if(SkillMenu.transform.GetChild(0).GetChild(i).gameObject.tag != "Back Button")
            {
                Destroy(SkillMenu.transform.GetChild(0).GetChild(i).gameObject);
            }
        }
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(2).gameObject);
        for(int i = 0; i < curPlayerUnit.HowManySkills(); i++)
        {
            Button skillButton = Instantiate(skillButtonPrefab, skillButtonPanel.transform);
            skillButton.transform.SetParent(skillButtonPanel.transform);
            SkillHUD skHUD = skillButton.GetComponent<SkillHUD>();
            skHUD.buttonID = i;
            skHUD.descriptionText = dialogueText;
            skHUD.costText = costText;
            skHUD.typeSprite = typeSprite;
            skHUD.skillName.text = skHUD.whatAmI(curPlayerUnit, skHUD.buttonID);
            skillButton.onClick.AddListener(OnSkillUse);
            skillButton.transform.SetSiblingIndex(i);
        }
        dialogueText.text = "What would you like to do?";

    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN1)
        {
            return;
        }
        
        StartCoroutine(selectSystem.SelectorEnemy(0,-1));
    }
    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN1)
        {
            return;
        }
        ActionMenu.SetActive(false);
        SkillMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SkillMenu.transform.GetChild(0).GetChild(0).gameObject);
    }

    public void OnSkillUse()
    {
        if (state != BattleState.PLAYERTURN1)
        {
            return;
        }
        GameObject skillButton = EventSystem.current.currentSelectedGameObject;
        // Button curButton = skillButton.gameObject.GetComponent<Button>();
        SkillHUD skillCaller = skillButton.gameObject.GetComponent<SkillHUD>();
        Skill curSkill = curPlayerUnit.Skills[skillCaller.buttonID];
        if(curPlayerUnit.curMP < curSkill.cost)
        {
            dialogueText.text = "Not enough MP to use " + curSkill.skillName + ".";
            return;
        } else if(curPlayerUnit.curHP < curSkill.costHP)
        {
            dialogueText.text = "Not enough HP to use " + curSkill.skillName + ".";
            return;
        } else
        {
            if((int)curSkill.SkillCategory == 0 || (int)curSkill.SkillCategory == 3 || (int)curSkill.SkillCategory == 4)
                StartCoroutine(selectSystem.SelectorEnemy(1,skillCaller.buttonID));
            else
                StartCoroutine(selectSystem.SelectorPlayer(1, skillCaller.buttonID));
            EventSystem.current.SetSelectedGameObject(null);
        }
        
    }

    public void OnBackButton()
    {
        if(state != BattleState.PLAYERTURN1)
        {
            return;
        }
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(2).gameObject);
        dialogueText.text = "What would you like to do?";
    }
    public void OnGuardButton()
    {
        if(state != BattleState.PLAYERTURN1)
        {
            return;
        }
        StartCoroutine(GuardActive());
    }

    IEnumerator GuardActive()
    {
        curPlayerUnit.guard = true;
        dialogueText.text = curPlayerUnit + " is guarding for the next attack.";

        yield return new WaitForSeconds(1.1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("menuScene");
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
    

    public IEnumerator PlayerAttack()
    {
        int incDmg = (int)(Unit.DamageCalc(curPlayerUnit.atkStat, enemyUnit.defStat, curPlayerUnit.weapon.power) * damageModCalc(curPlayerUnit, enemyUnit));
        bool isDead = enemyUnit.TakeDamage((int)(incDmg));

        dialogueText.text = "You attack, dealing " + incDmg + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        } else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    public IEnumerator PlayerAttack(Unit target)
    {
        int incDmg = (int)(Unit.DamageCalc(curPlayerUnit.atkStat, target.defStat, curPlayerUnit.weapon.power) * damageModCalc(curPlayerUnit, target));
        bool isDead = enemyUnit.TakeDamage((int)(incDmg));

        dialogueText.text = "You attack, dealing " + incDmg + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        } else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public IEnumerator SkillUsage(int skillNum)
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        Skill curSkill = curPlayerUnit.Skills[skillNum];
        if(curSkill.All)
        {
            curSkill.SkillUseAll((int)curSkill.SkillCategory, curPlayerUnit, AllyUnitList, EnemyUnitList, dialogueText, PlayerHUDList);
        } else
        {
            if(curSkill.Heal)
            {
                curSkill.SkillUseSingle((int)curSkill.SkillCategory, curPlayerUnit, curPlayerUnit, dialogueText, playerHUD1);
            } else
            {
                curSkill.SkillUseSingle((int)curSkill.SkillCategory, curPlayerUnit, enemyUnit, dialogueText, playerHUD1);
            }
        }
        yield return new WaitForSeconds(2f);
        //Update HUDs
            if (enemyUnit.curHP <= 0)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            } else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        
        //
    }
    public IEnumerator SkillUsage(int skillNum, Unit target)
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        Skill curSkill = curPlayerUnit.Skills[skillNum];
        if(curSkill.All)
        {
            curSkill.SkillUseAll((int)curSkill.SkillCategory, curPlayerUnit, AllyUnitList, EnemyUnitList, dialogueText, PlayerHUDList);
        } else
        {
            curSkill.SkillUseSingle((int)curSkill.SkillCategory, curPlayerUnit, target, dialogueText, curPlayerUnit.GetHUD());
        }
        yield return new WaitForSeconds(2f);
        //Update HUDs
            if (enemyUnit.curHP <= 0)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            } else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        
        //
    }

    IEnumerator EnemyTurn()
    {
        bool dead;
        EventSystem.current.SetSelectedGameObject(null);
        int incDmg = (int)(Unit.DamageCalc(enemyUnit.atkStat, curPlayerUnit.defStat, enemyUnit.weapon.power) * damageModCalc(enemyUnit, curPlayerUnit));
        dialogueText.text = enemyUnit.unitName + " attacks, dealing " + incDmg + " damage.";

        if(curPlayerUnit.guard)
        {
            bool isDead = curPlayerUnit.TakeDamage((int)(incDmg));

            playerHUD1.UpdateHUD(curPlayerUnit);
            yield return new WaitForSeconds(1f);
            curPlayerUnit.guard = false;
            dialogueText.text = curPlayerUnit + " is no longer guarding.";
            dead = isDead;
        } else
        {
            bool isDead = curPlayerUnit.TakeDamage((int)(incDmg));
            dead = isDead;

            playerHUD1.UpdateHUD(curPlayerUnit);
        }

        yield return new WaitForSeconds(2f);

        if(dead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        } else
        {
            state = BattleState.PLAYERTURN1;
            playerTurn();
        }
    }

}
