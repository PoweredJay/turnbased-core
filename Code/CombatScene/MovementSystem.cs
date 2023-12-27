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
    public BattleSystem BattleSystem;
    [Header("Unit Management")]
    public bool moveActive;
    public int curSelectEnemy;
    public int curSelectAlly;
    public Unit curSelectedUnit;
    public List<Unit> AllyUnitList;
    public List<Unit> EnemyUnitList;
    [Header("Player Grid Transforms")]
    public List<Transform> PlayerTransformList;
    public List<Transform> EnemyTransformList;
        /*
        Due to the way the transform lists are populated, this is how the transform positions in the 3x3 grid
        correspond to the indices in both of the transform lists.
        0  1  2
        3  4  5
        6  7  8
        This will be important for each Unit's gridPos attribute.
    */
    // Start is called before the first frame update
    void Start()
    {
        AllyUnitList = BattleSystem.AllyUnitList;
        EnemyUnitList = BattleSystem.EnemyUnitList;
        GameObject selector = Instantiate(moveCursorPrefab);
        moveCursor = selector;
        moveCursor.SetActive(false);
        for(int i = 0; i < playerTransformSet.transform.childCount-1; i++)
        {
            PlayerTransformList[i] = playerTransformSet.transform.GetChild(i);
        }
        for(int i = 0; i < enemyTransformSet.transform.childCount-1; i++)
        {
            EnemyTransformList[i] = enemyTransformSet.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(moveActive)
        {
            nameBox.SetActive(true);
            moveCursor.SetActive(true);
        } else
        {
            nameBox.SetActive(false);
            moveCursor.SetActive(false);
        }
    }
}
