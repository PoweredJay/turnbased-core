using System.Collections;
using System.Collections.Generic;
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
    The grid transforms are nine distinct points where you can place units in a 3x3 square

    11 12 13
    21 22 23
    31 32 33
    Coordinates cooresponding to this grid ^
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

    public Text dialogueText;

    public GameObject ActionMenu;

    public GameObject SkillMenu;
    private AudioSource battleMusic;

    Unit playerUnit;
    Unit enemyUnit;
    public BattleState state;
    GameObject lastSelect;
    public static Unit curPlayerUnit;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        Cursor.visible = false;
        battleMusic = GetComponent<AudioSource>();
        battleMusic.loop = true;
        battleMusic.Play();
        StartCoroutine(SetupBattle());
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(1).gameObject);
        lastSelect = new GameObject();
    }

    void Update()
    {
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
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        state = BattleState.PLAYERTURN1;
        playerTurn();
    }

    void playerTurn()
    {
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

    public int DamageCalc(int atkStat, int defStat, int thingPow, float dmgMod)
    {
        int finalDam = (int)(thingPow * ((Mathf.Pow((float)atkStat/defStat, 2) + (float)atkStat/defStat)/4));
        return finalDam;
    }

    public int HealCalc(int magStat, int healPow)
    {
        int finalHeal = (int)(healPow - ((float)healPow/magStat));
        return finalHeal;
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
        StartCoroutine(SkillUsage(skillCaller.buttonID));
    }

    public void OnBackButton()
    {
        ActionMenu.SetActive(true);
        SkillMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ActionMenu.transform.GetChild(1).gameObject);
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

    IEnumerator PlayerAttack()
    {
        int incDmg = DamageCalc(curPlayerUnit.atkStat, enemyUnit.defStat, curPlayerUnit.wepAtk, 1);
        bool isDead = enemyUnit.TakeDamage(incDmg);

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
        if(curPlayerUnit.Skills[skillNum].Heal == true)
        {
            int incHeal = HealCalc(curPlayerUnit.magStat, curPlayerUnit.Skills[skillNum].power);
            curPlayerUnit.HealDamage(incHeal);
            dialogueText.text = "You cast " + curPlayerUnit.Skills[skillNum].skillName + " , healing " + incHeal + " HP.";

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        } else 
        {
            int incDmg = DamageCalc(curPlayerUnit.magStat, enemyUnit.defStat, curPlayerUnit.Skills[skillNum].power, 1);
            bool isDead = enemyUnit.TakeDamage(incDmg);

            dialogueText.text = "You cast " + curPlayerUnit.Skills[skillNum].skillName + " , dealing " + incDmg + " damage!";

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
        //
    }

    IEnumerator EnemyTurn()
    {
        int incDmg = DamageCalc(enemyUnit.atkStat, curPlayerUnit.defStat, enemyUnit.wepAtk, 1);
        dialogueText.text = enemyUnit.unitName + " attacks, dealing " + incDmg + " damage.";

        bool isDead = curPlayerUnit.TakeDamage(incDmg);

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
