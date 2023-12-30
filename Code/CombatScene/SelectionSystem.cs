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
    public MovementSystem moveSystem;
    public bool selectionEnemy;
    public bool selectionPlayer;
    public bool skillAll;
    public int curSelectPosX;
    public int curSelectPosY;
    public int curSelectIndex;
    public int curActionCode;
    public int curSkillNum;
    public Unit curSelectedUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    [Header("Player Grid Transforms")]
    public List<Transform> PlayerTransformList;
    public List<Transform> EnemyTransformList;
    public Transform[ , ] PlayerTransformArray = new Transform[3,3];
    public Transform[ , ] EnemyTransformArray = new Transform[3,3];
    void Start()
    {
        AllyUnitList = BattleSystem.AllyUnitList;
        EnemyUnitList = BattleSystem.EnemyUnitList;
        GameObject selector = Instantiate(cursorPrefab);
        selectCursor = selector;
        selectCursor.SetActive(false);
    }
    public void UpdateTransforms()
    {
        PlayerTransformList = moveSystem.PlayerTransformList;
        EnemyTransformList = moveSystem.EnemyTransformList;
        PlayerTransformArray = moveSystem.PlayerTransformArray;
        EnemyTransformArray = moveSystem.EnemyTransformArray;
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
            selectCursor.SetActive(true);
            if(Input.GetKeyDown(KeyCode.UpArrow))
                curSelectPosY = (curSelectPosY + 2) % 3;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                curSelectPosY = (curSelectPosY + 1) % 3;
            if(Input.GetKeyDown(KeyCode.LeftArrow)) 
                curSelectPosX = (curSelectPosX + 2) % 3;
            if(Input.GetKeyDown(KeyCode.RightArrow))
                curSelectPosX = (curSelectPosX + 1) % 3;
            if(Input.GetKeyDown(KeyCode.X))
            {
                selectionEnemy = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(curActionCode == 0)
                {
                    if(UnitAtGridEnemy(EnemyTransformArray[curSelectPosX,curSelectPosY]))
                    {
                        DoAction();
                    } else
                    {
                        nameText.text = "Nothing there to target.";
                        return;
                    }
                } else if (curActionCode == 1)
                {
                    if(skillAll)
                    {
                        DoAction();
                    } else
                    {
                        if(UnitAtGridEnemy(EnemyTransformArray[curSelectPosX,curSelectPosY]))
                        {
                            DoAction();
                            Debug.Log("yeah");
                        } else
                        {
                            nameText.text = "Nothing there to target.";
                            return;
                        }
                    }
                }
            }
            if(skillAll)
            {
                nameText.text = "All enemies";
            } else
            {
                if(UnitAtGridEnemy(EnemyTransformArray[curSelectPosX,curSelectPosY]))
                {
                    curSelectIndex = curSelectPosX + curSelectPosY*3;
                    curSelectedUnit = EnemyAtPoint();
                    nameText.text = curSelectedUnit.unitName;
                }
            }
            selectCursor.transform.position = EnemyTransformArray[curSelectPosX,curSelectPosY].transform.position;
        } else if(selectionPlayer)
        {
            nameBox.SetActive(true);
            selectCursor.SetActive(true);
            if(Input.GetKeyDown(KeyCode.UpArrow))
                curSelectPosY = (curSelectPosY + 2) % 3;
            if(Input.GetKeyDown(KeyCode.DownArrow))
                curSelectPosY = (curSelectPosY + 1) % 3;
            if(Input.GetKeyDown(KeyCode.LeftArrow)) 
                curSelectPosX = (curSelectPosX + 2) % 3;
            if(Input.GetKeyDown(KeyCode.RightArrow))
                curSelectPosX = (curSelectPosX + 1) % 3;
            if(Input.GetKeyDown(KeyCode.X))
            {
                selectionPlayer = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(curActionCode == 0)
                {
                    if(UnitAtGridAlly(PlayerTransformArray[curSelectPosX,curSelectPosY]))
                    {
                        DoAction();
                    } else
                    {
                        nameText.text = "Nothing there to target.";
                        return;
                    }
                } else if (curActionCode == 1)
                {
                    if(skillAll)
                    {
                        DoAction();
                    } else
                    {
                        if(UnitAtGridAlly(PlayerTransformArray[curSelectPosX,curSelectPosY]))
                        {
                            DoAction();
                        } else
                        {
                            nameText.text = "Nothing there to target.";
                            return;
                        }
                    }
                }
            }
            if(skillAll)
            {
                nameText.text = "All allies";
            } else
            {
                if(UnitAtGridAlly(PlayerTransformArray[curSelectPosX,curSelectPosY]))
                {
                    curSelectIndex = curSelectPosX + curSelectPosY*3;
                    curSelectedUnit = AllyAtPoint();
                    nameText.text = curSelectedUnit.unitName;
                }
            }
        selectCursor.transform.position = PlayerTransformArray[curSelectPosX,curSelectPosY].transform.position;
        } else
        {
            nameBox.SetActive(false);
            selectCursor.SetActive(false);
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
        nameText.text = "Select a target.";
    }
    public void DoAction()
    {
        switch(curActionCode)
        {
            //Attack
            case 0:
                BattleSystem.curPlayerUnit.targetUnit = curSelectedUnit;
                BattleSystem.curPlayerUnit.action = ActionType.Attack;
                BattleSystem.AdvanceTurn();
                // StartCoroutine(BattleSystem.PlayerAttack(curSelectedUnit));
                break;
            //Skill
            case 1:
                BattleSystem.curPlayerUnit.targetUnit = curSelectedUnit;
                BattleSystem.curPlayerUnit.actionSkillNum = curSkillNum;
                BattleSystem.curPlayerUnit.action = ActionType.Skill;
                BattleSystem.AdvanceTurn();
                // StartCoroutine(BattleSystem.SkillUsage(curSkillNum, curSelectedUnit));
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
        nameText.text = "Select a target.";
    }
    public bool UnitAtGridAlly(Transform pos)
    {
        foreach(Unit ally in AllyUnitList)
        {
            if(PlayerTransformList[ally.gridPos] == pos)
            {
                return true;
            }
        }
        return false;
    }
    public bool UnitAtGridEnemy(Transform pos)
    {
        foreach(Unit enemy in EnemyUnitList)
        {
            if(EnemyTransformList[enemy.gridPos] == pos)
            {
                return true;
            }
        }
        return false;
    }
    public Unit AllyAtPoint()
    {
        foreach(Unit ally in AllyUnitList)
        {
            if(ally.gridPos == curSelectIndex)
            {
                return ally;
            }
        }
        return null;
    }
    public Unit EnemyAtPoint()
    {
        foreach(Unit enemy in EnemyUnitList)
        {
            if(enemy.gridPos == curSelectIndex)
            {
                return enemy;
            }
        }
        return null;
    }
}