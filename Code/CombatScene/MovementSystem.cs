using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovementSystem : MonoBehaviour
{
    [Header("General Things")]
    public GameObject moveCursorPrefab;
    public GameObject moveCursor;
    public GameObject nameBox;
    public Text nameText;
    public GameObject playerTransformSet;
    public GameObject enemyTransformSet;
    public GameObject HUDTransformSet;
    public BattleSystem BattleSystem;
    [Header("Unit Management")]
    public bool moveActive;
    public int curSelectPosX;
    public int curSelectPosY;
    public Unit curSelectedUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    [Header("Player Grid Transforms")]
    public List<Transform> PlayerTransformList;
    public List<Transform> EnemyTransformList;
    public List<Transform> HUDTransformList;
    public Transform[ , ] PlayerTransformArray = new Transform[3,3];
    /*
        Due to the way the transform lists are populated, this is how the transform positions in the 3x3 grid
        correspond to the indices in both of the transform lists.
        0  1  2
        3  4  5
        6  7  8
        This will be important for each Unit's gridPos attribute.
        The Lists are to initialize positions in SetupBattlePos(). The Array is to handle cursor movement in Update(), and also 
        to handle area-of-effect spells in the future (as a way to easily check surrounding tiles)
    */
    // Start is called before the first frame update
    void Start()
    {
        AllyUnitList = BattleSystem.AllyUnitList;
        EnemyUnitList = BattleSystem.EnemyUnitList;
        GameObject selector = Instantiate(moveCursorPrefab);
        moveCursor = selector;
        moveCursor.SetActive(false);
        foreach(Transform child in playerTransformSet.transform)
        {
            PlayerTransformList.Add(child);
        }
        foreach(Transform child in enemyTransformSet.transform)
        {
            EnemyTransformList.Add(child);
        }
        foreach(Transform child in HUDTransformSet.transform)
        {
            HUDTransformList.Add(child);
        }
        //For the 2D array
        for(int i = 0; i < PlayerTransformArray.GetLength(0); i++)
        {
            for(int j = 0; j < PlayerTransformArray.GetLength(1); j++)
            {
                PlayerTransformArray[j,i] = playerTransformSet.transform.GetChild(i*PlayerTransformArray.GetLength(0)+j);
            }
        }
        SetupBattlePos();
    }
    public void SetupBattlePos()
    {
        foreach(Unit ally in AllyUnitList)
        {
                ally.transform.position = PlayerTransformList[ally.gridPos].transform.position;
                BattleHUD allyHUD = ally.GetHUD();
                allyHUD.transform.position = HUDTransformList[ally.gridPos].transform.position;
        }
        foreach(Unit enemy in EnemyUnitList)
        {
            if(enemy.gridPos >= 0)
                enemy.transform.position = EnemyTransformList[enemy.gridPos].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
            moveActive = !moveActive;
        if(moveActive)
        {
            nameBox.SetActive(true);
            moveCursor.SetActive(true);

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
                moveActive = false;
                EventSystem.current.SetSelectedGameObject(BattleSystem.lastSelect);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                int pos = curSelectPosX + curSelectPosY*3;
                foreach(Unit friend in AllyUnitList)
                {
                    if(friend.gridPos == pos)
                    {
                        nameText.text = "Can't move there!";
                        return;
                    }
                }
                DoMove(curSelectedUnit);
                moveActive = false;
            }
            // if(curSelectPos < 0 || curSelectPos >= AllyUnitList.Count())
            //     curSelectPos = 0;
            moveCursor.transform.position = PlayerTransformArray[curSelectPosX,curSelectPosY].transform.position;
        } else
        {
            nameBox.SetActive(false);
            moveCursor.SetActive(false);
        }
    }
    public IEnumerator MoveAction(Unit unit)
    {
        yield return new WaitForSeconds(0.1f);
        nameText.text = "Move to...";
        moveActive = true;
        curSelectedUnit = unit;
    }
    public void DoMove(Unit unit)
    {
        unit.gridPos = curSelectPosX + curSelectPosY*3;
        unit.transform.position = PlayerTransformList[unit.gridPos].transform.position;
        BattleHUD unitHUD = unit.GetHUD();
        unitHUD.transform.position = HUDTransformList[unit.gridPos].transform.position;
        unit.moved = true;
    }
}
