using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionSystem : MonoBehaviour
{
    public GameObject cursorPrefab;
    public GameObject selectCursor;
    public BattleSystem BattleSystem;
    public bool selectionEnemy;
    public bool selectionPlayer;
    public int curSelect;
    public int curActionCode;
    public int curSkillNum;
    public Unit curSelectedUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    void Start()
    {
        AllyUnitList = BattleSystem.AllyUnitList;
        EnemyUnitList = BattleSystem.EnemyUnitList;
        GameObject selector = Instantiate(cursorPrefab);
        selectCursor = selector;
        selectCursor.SetActive(false);
    }
    void Update()
    {
        if(AllyUnitList != BattleSystem.AllyUnitList || EnemyUnitList != BattleSystem.EnemyUnitList)
        {
            AllyUnitList = BattleSystem.AllyUnitList;
            EnemyUnitList = BattleSystem.EnemyUnitList;
        }
        if(selectionEnemy)
        {
            selectCursor.SetActive(true);
            if(Input.GetKeyDown(KeyCode.UpArrow))
                curSelect = ((curSelect - 1) + EnemyUnitList.Count) % EnemyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                curSelect = (curSelect + 1) % EnemyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.X))
            {
                selectionEnemy = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }
            if(Input.GetKeyDown(KeyCode.Return))
                DoAction();
            curSelectedUnit = EnemyUnitList[curSelect];
            selectCursor.transform.position = curSelectedUnit.transform.position;
        } else if(selectionPlayer)
        {
            selectCursor.SetActive(true);
            if(Input.GetKeyDown(KeyCode.UpArrow))
                curSelect = ((curSelect - 1) + EnemyUnitList.Count) % EnemyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                curSelect = (curSelect + 1) % AllyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.X))
            {
                selectionPlayer = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }   
            if(Input.GetKeyDown(KeyCode.Return))
                DoAction();
            curSelectedUnit = AllyUnitList[curSelect];
            selectCursor.transform.position = curSelectedUnit.transform.position;
        } else
        {
            selectCursor.SetActive(false);
        }
    }
    public IEnumerator SelectorEnemy(int actionCode, int skillNum)
    {
        curActionCode = actionCode;
        curSkillNum = skillNum;
        yield return new WaitForSeconds(0.1f);
        selectionEnemy = true;
    }
    public void DoAction()
    {
        switch(curActionCode)
        {
            //Attack
            case 0:
                StartCoroutine(BattleSystem.PlayerAttack(curSelectedUnit));
                break;
            //Skill
            case 1:
                StartCoroutine(BattleSystem.SkillUsage(curSkillNum, curSelectedUnit));
                break;
        }
        selectionEnemy = false;
        selectionPlayer = false;
        selectCursor.SetActive(false);

    }
    public IEnumerator SelectorPlayer(int actionCode, int skillNum)
    {
        curActionCode = actionCode;
        curSkillNum = skillNum;
        yield return new WaitForSeconds(0.1f);
        selectionPlayer = true;
    }
}