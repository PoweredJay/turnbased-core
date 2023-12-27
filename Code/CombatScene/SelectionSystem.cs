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
    public GameObject nameBox;
    public Text nameText;
    public BattleSystem BattleSystem;
    public bool selectionEnemy;
    public bool selectionPlayer;
    public bool skillAll;
    public int curSelectEnemy;
    public int curSelectAlly;
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
            nameBox.SetActive(true);
            if(curSelectEnemy < 0 || curSelectEnemy >= EnemyUnitList.Count)
            {
                curSelectEnemy = 0;
            }
            selectCursor.SetActive(true);
            if(Input.GetKeyDown(KeyCode.UpArrow))
                curSelectEnemy = ((curSelectEnemy - 1) + EnemyUnitList.Count) % EnemyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                curSelectEnemy = (curSelectEnemy + 1) % EnemyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.X))
            {
                selectionEnemy = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }
            if(Input.GetKeyDown(KeyCode.Return))
                DoAction();
            curSelectedUnit = EnemyUnitList[curSelectEnemy];
            if(skillAll)
            {
                nameText.text = "All enemies";
            } else
            {
                nameText.text = curSelectedUnit.unitName;
            }

            selectCursor.transform.position = curSelectedUnit.transform.Find("CenterLocator").position;
        } else if(selectionPlayer)
        {
            nameBox.SetActive(true);
            if(curSelectAlly < 0 || curSelectAlly >= AllyUnitList.Count)
            {
                curSelectAlly = 0;
            }
            selectCursor.SetActive(true);
            if(Input.GetKeyDown(KeyCode.UpArrow))
                curSelectAlly = ((curSelectAlly - 1) + AllyUnitList.Count) % AllyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                curSelectAlly = (curSelectAlly + 1) % AllyUnitList.Count;
            if(Input.GetKeyDown(KeyCode.X))
            {
                selectionPlayer = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }   
            if(Input.GetKeyDown(KeyCode.Return))
                DoAction();
            curSelectedUnit = AllyUnitList[curSelectAlly];
            if(skillAll)
            {
                nameText.text = "All allies";
            } else
            {
                nameText.text = curSelectedUnit.unitName;
            }
            selectCursor.transform.position = curSelectedUnit.transform.Find("CenterLocator").position;
        } else
        {
            selectCursor.SetActive(false);
            nameBox.SetActive(false);
        }
    }
    public IEnumerator SelectorEnemy(int actionCode, int skillNum)
    {
        curActionCode = actionCode;
        curSkillNum = skillNum;
        yield return new WaitForSeconds(0.1f);
        selectionEnemy = true;
        if(skillNum >= 0 && BattleSystem.curPlayerUnit.Skills[skillNum].All)
        {
            skillAll = true;
        } else
        {
            skillAll = false;
        }
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
        EventSystem.current.SetSelectedGameObject(null);
        selectionEnemy = false;
        selectionPlayer = false;
        selectCursor.SetActive(false);
        nameBox.SetActive(false);
    }
    public IEnumerator SelectorPlayer(int actionCode, int skillNum)
    {
        curActionCode = actionCode;
        curSkillNum = skillNum;
        yield return new WaitForSeconds(0.1f);
        selectionPlayer = true;
        if(skillNum >= 0 && BattleSystem.curPlayerUnit.Skills[skillNum].All)
        {
            skillAll = true;
        } else
        {
            skillAll = false;
        }
    }
}