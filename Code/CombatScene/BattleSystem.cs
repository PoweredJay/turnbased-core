using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum BattleState { PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, PLAYERTURN4, PLAYERTURN5, START, TURNGO, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [Header("Prefabs & GameObjects")]
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
    public Transform enemyStation;
    [Header("Presets")]
    public BattleHUD playerHUD1;
    public BattleHUD playerHUD2;
    public BattleHUD playerHUD3;
    public BattleHUD playerHUD4;
    public BattleHUD playerHUD5;
    public SelectionSystem selectSystem;
    public MovementSystem moveSystem;
    public UnitUISystem unitUISystem;

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
    [Header("Units")]
    public Unit selectedEnemyUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    public List<Unit> UnitList;
    public List<BattleHUD> PlayerHUDList;
    public List<BattleHUD> EnemyHUDList;
    public int curTurn;
    
    void Start()
    {
        state = BattleState.START;
        Cursor.visible = false;
        battleMusic = GetComponent<AudioSource>();
        battleMusic.Play();
        battleMusic.loop = true;
        StartCoroutine(SetupBattle());
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(0).GetChild(0).gameObject);
        lastSelect = new GameObject();
    }

    void Update()
    {
        bool somethingHappening;
        if(state == BattleState.START || state == BattleState.ENEMYTURN || state == BattleState.TURNGO || selectSystem.selectionEnemy == true || selectSystem.selectionPlayer == true || moveSystem.moveActive == true)
            somethingHappening = true;
        else
            somethingHappening = false;
        Cursor.visible = false;
        if (EventSystem.current.currentSelectedGameObject == null && !somethingHappening)
            EventSystem.current.SetSelectedGameObject(lastSelect);
        else if(somethingHappening)
            EventSystem.current.SetSelectedGameObject(null);
        else
            lastSelect = EventSystem.current.currentSelectedGameObject;
        if(ActionMenu.activeSelf)
        {

            if(!somethingHappening && Input.GetKeyDown(KeyCode.X))
            {
                revertTurn();
            }
        }
        if(SkillMenu.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.X))
                OnBackButton();
        }
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.START;
        GameObject playerGO1 = Instantiate(playerPrefab, playerStation);
        curPlayerUnit = playerGO1.GetComponent<Unit>();
        PlayerUnit1 = curPlayerUnit;
        PlayerUnit1.SetPlayerHUD(playerHUD1);
        GameObject playerGO2 = Instantiate(playerPrefab2, playerStation);
        PlayerUnit2 = playerGO2.GetComponent<Unit>();
        GameObject playerGO3 = Instantiate(playerPrefab3, playerStation);
        PlayerUnit3 = playerGO3.GetComponent<Unit>();
        GameObject playerGO4 = Instantiate(playerPrefab4, playerStation);
        PlayerUnit4 = playerGO4.GetComponent<Unit>();
        GameObject playerGO5 = Instantiate(playerPrefab5, playerStation);
        PlayerUnit5 = playerGO5.GetComponent<Unit>();

        GameObject enemyGO1 = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGO1.GetComponent<Unit>();

        // GameObject enemyGO2 = Instantiate(enemyPrefab, playerStation);
        // enemyUnit2 = enemyGO2.GetComponent<Unit>();
        AllyUnitList.Add(PlayerUnit1);
        AllyUnitList.Add(PlayerUnit2);
        AllyUnitList.Add(PlayerUnit3);
        AllyUnitList.Add(PlayerUnit4);
        AllyUnitList.Add(PlayerUnit5);
        EnemyUnitList.Add(enemyUnit);
        // EnemyUnitList.Add(enemyUnit2);
        UnitList.AddRange(AllyUnitList);
        UnitList.AddRange(EnemyUnitList);
        PlayerHUDList.Add(playerHUD1);
        PlayerHUDList.Add(playerHUD2);
        PlayerHUDList.Add(playerHUD3);
        PlayerHUDList.Add(playerHUD4);
        PlayerHUDList.Add(playerHUD5);
        PlayerUnit1.SetPlayerHUD(playerHUD1);
        PlayerUnit2.SetPlayerHUD(playerHUD2);
        PlayerUnit3.SetPlayerHUD(playerHUD3);
        PlayerUnit4.SetPlayerHUD(playerHUD4);
        PlayerUnit5.SetPlayerHUD(playerHUD5);
        playerHUD1.SetHUD(PlayerUnit1);
        playerHUD2.SetHUD(PlayerUnit2);
        playerHUD3.SetHUD(PlayerUnit3);
        playerHUD4.SetHUD(PlayerUnit4);
        playerHUD5.SetHUD(PlayerUnit5);
        dialogueText.text = "You find yourself face to face with " + enemyUnit.unitName + ".";
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);


        yield return new WaitForSeconds(2.5f);

        state = BattleState.PLAYERTURN1;
        playerTurn();
    }
    public bool checkPlayerTurn()
    {
        if(state == BattleState.PLAYERTURN1 || state == BattleState.PLAYERTURN2 || state == BattleState.PLAYERTURN3 || state == BattleState.PLAYERTURN4 || state == BattleState.PLAYERTURN5)
            return true;
        return false;
    }

    public void playerTurn()
    {
        switch(curTurn)
        {
            case 0:
                curPlayerUnit = PlayerUnit1;
                state = BattleState.PLAYERTURN1;
                break;
            case 1:
                curPlayerUnit = PlayerUnit2;
                state = BattleState.PLAYERTURN2;
                break;
            case 2:
                curPlayerUnit = PlayerUnit3;
                state = BattleState.PLAYERTURN3;
                break;
            case 3:
                curPlayerUnit = PlayerUnit4;
                state = BattleState.PLAYERTURN4;
                break;
            case 4:
                curPlayerUnit = PlayerUnit5;
                state = BattleState.PLAYERTURN5;
                break;
            case 5:
                StartCoroutine(TurnsGo());
                curTurn = 0;
                return;
        }
        unitUISystem.SetCurrentUnit(curPlayerUnit);
        unitUISystem.SetBoxColor();
        curPlayerUnit.moved = false;
        for (int i = SkillMenu.transform.GetChild(0).childCount - 1; i >= 0; i--)
        {
            if(SkillMenu.transform.GetChild(0).GetChild(i).gameObject.tag != "Back Button")
                Destroy(SkillMenu.transform.GetChild(0).GetChild(i).gameObject);
        }
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        lastSelect = ActionMenu.transform.GetChild(0).GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(0).GetChild(0).gameObject);
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
        dialogueText.text = "What will " + curPlayerUnit + " do?";
    }
    public void AdvanceTurn()
    {
        curTurn++;
        playerTurn();
    }
    public void revertTurn()
    {
        Unit theUnit = null;
        if(curTurn == 0)
            return;
        switch(curTurn)
        {
            case 1:
                theUnit = PlayerUnit1;
                break;
            case 2:
                theUnit = PlayerUnit2;
                break;
            case 3:
                theUnit = PlayerUnit3;
                break;
            case 4:
                theUnit = PlayerUnit4;
                break;
        }
        theUnit.targetUnit = null;
        theUnit.action = ActionType.None;
        theUnit.actionSkillNum = 0;
        curTurn--;
        playerTurn();
    }

    public void OnAttackButton()
    {
        if (!checkPlayerTurn())
            return;
        
        StartCoroutine(selectSystem.SelectorEnemy(0,-1));
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnSkillButton()
    {
        if (!checkPlayerTurn())
            return;
        ActionMenu.SetActive(false);
        SkillMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SkillMenu.transform.GetChild(0).GetChild(0).gameObject);
    }

    public void OnSkillUse()
    {
        if (!checkPlayerTurn())
            return;
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
        if(!checkPlayerTurn())
            return;
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(0).GetChild(0).gameObject);
        dialogueText.text = "What will " + curPlayerUnit + " do?";
    }
    public void OnGuardButton()
    {
        if(!checkPlayerTurn())
            return;
        curPlayerUnit.action = ActionType.Guard;
        AdvanceTurn();
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnMoveButton()
    {
        if(!checkPlayerTurn())
            return;
        if(curPlayerUnit.moved)
        {
            dialogueText.text = curPlayerUnit + " has already moved this turn.";
            return;
        }
            StartCoroutine(moveSystem.MoveAction(curPlayerUnit));
            EventSystem.current.SetSelectedGameObject(null);
    }

    IEnumerator GuardActive(Unit guarder)
    {
        guarder.guard = true;
        dialogueText.text = guarder + " is guarding for the next attack.";  
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1.1f);
    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
            dialogueText.text = "You won the battle!";
        else if (state == BattleState.LOST)
            dialogueText.text = "You were defeated.";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("menuScene");
    }
    public static double damageModCalc(Unit atkr, Unit defr)
    {
        double damageMod = (1.0 + 0.2*atkr.ATKStatus) * (1.0 - 0.2*defr.DEFStatus);
        int defrAff = defr.GetAffinity((int)atkr.weapon.damageType);
        if(atkr.flow)
            damageMod *= 1.1;
        if(defr.guard)
            damageMod *= 0.6 * Math.Min(1.0 - 0.5*defrAff, 1.0);
        else
        {
            damageMod *= (1.0 - 0.5*defrAff);
            if(defrAff == -1)
                atkr.flow = true;
        }
        return damageMod;
    }
    public static double damageModCalc(Unit atkr, Unit defr, Skill skill)
    {
        double damageMod = (1.0 + 0.2*atkr.ATKStatus) * (1.0 - 0.2*defr.DEFStatus);
        int defrAff = defr.GetAffinity((int)skill.type);
        if(atkr.flow)
            damageMod *= 1.1;
        if(defr.guard)
            damageMod *= 0.6 * Math.Min(1.0 - 0.5*defrAff, 1.0);
        else
        {
            damageMod *= (1.0 - 0.5*defrAff);
            if(defrAff == -1)
                atkr.flow = true;
        }
        return damageMod;
    }
    

    public IEnumerator PlayerAttack(Unit attack, Unit target)
    {
        int incDmg = (int)(Unit.DamageCalc(attack.atkStat, target.defStat, attack.weapon.power) * damageModCalc(attack, target));
        bool isDead = target.TakeDamage((int)(incDmg));
        EventSystem.current.SetSelectedGameObject(null);

        dialogueText.text = attack + " attacks " + target + ", dealing " + incDmg + " damage!";
        
        yield return new WaitForSeconds(2f);
        //Check Win/Loss/No
    }
    public IEnumerator SkillUsage(int skillNum, Unit attack, Unit target)
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        Skill curSkill = attack.Skills[skillNum];
        if(curSkill.All)
            curSkill.SkillUseAll((int)curSkill.SkillCategory, attack, AllyUnitList, EnemyUnitList, dialogueText, PlayerHUDList);
        else
            curSkill.SkillUseSingle((int)curSkill.SkillCategory, attack, target, dialogueText, attack.GetHUD());
        yield return new WaitForSeconds(2f);
        //Check Win/Loss/No
    }
    IEnumerator EnemyTurn(Unit enemy)
    {
        bool dead;
        EventSystem.current.SetSelectedGameObject(null);
        Unit selectedUnit = AllyUnitList[(int)UnityEngine.Random.Range(0f,4f)];
        while(selectedUnit.curHP <=0)
        {
            selectedUnit = AllyUnitList[(int)UnityEngine.Random.Range(0f,4f)];
        }
        int incDmg = (int)(Unit.DamageCalc(enemy.atkStat, selectedUnit.defStat, enemy.weapon.power) * damageModCalc(enemyUnit, curPlayerUnit));
        dialogueText.text = enemy + " attacks " + selectedUnit + ", dealing " + incDmg + " damage.";

        if(selectedUnit.guard)
        {
            bool isDead = selectedUnit.TakeDamage((int)(incDmg));

            selectedUnit.GetHUD().UpdateHUD(selectedUnit);
            yield return new WaitForSeconds(0.6f);
            selectedUnit.guard = false;
            dialogueText.text = curPlayerUnit + " is no longer guarding.";
            dead = isDead;
        } else
        {
            bool isDead = selectedUnit.TakeDamage((int)(incDmg));
            dead = isDead;

            selectedUnit.GetHUD().UpdateHUD(selectedUnit);
        }

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator TurnsGo()
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        unitUISystem.ResetBoxColor();
        state = BattleState.TURNGO;
        List<Unit> UnitListSPD = UnitList.OrderByDescending(unit=>unit.spdStat).ToList();
        foreach(Unit unit in UnitListSPD)
        {
            if(unit.action == ActionType.Guard)
                yield return GuardActive(unit);
        }
        foreach(Unit unit in UnitListSPD)
        {
            if(unit.ally)
            {
                switch(unit.action)
                {
                case ActionType.Attack:
                    yield return PlayerAttack(unit, unit.targetUnit);
                    break;
                case ActionType.Skill:
                    yield return SkillUsage(unit.actionSkillNum, unit, unit.targetUnit);
                    break;
                case ActionType.Leader:
                    break;
                case ActionType.Team:
                    break;
                case ActionType.Item:
                    break;
                case ActionType.Flee:
                    break;
                }
            } else
            {
                yield return EnemyTurn(unit);
            }
        }
        playerTurn();
    }
}
