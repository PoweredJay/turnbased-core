using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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


    public Text dialogueText;

    public GameObject ActionMenu;

    public GameObject SkillMenu;
    private AudioSource battleMusic;

    Unit playerUnit;
    Unit enemyUnit;
    public BattleState state;
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
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        dialogueText.text = "You find yourself face to face with " + enemyUnit.unitName;

        playerHUD1.SetHUD(playerUnit);
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

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.atkStat);

        dialogueText.text = "You attack, dealing " + playerUnit.atkStat + " damage!";

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
        //
    }

    public void OnSkillButton()
    {
        ActionMenu.SetActive(false);
        SkillMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SkillMenu.transform.GetChild(0).gameObject);
    }

    public void OnSkillHover()
    {

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

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.atkStat);

        playerHUD1.SetHP(playerUnit.curHP);

        yield return new WaitForSeconds(1f);

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
