using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class UnitUISystem : MonoBehaviour
{
    [Header("General Things")]
    public GameObject buttonPanel;
    public GameObject buttonPanelSkill;
    public GameObject nameBoxSel;
    public Text nameTextSel;
    public GameObject nameBoxMove;
    public Text nameTextMove;
    public GameObject dialogueBox;
    public Text dialogueText;
    public BattleSystem BattleSystem;
    public Unit curUnit;
    // [Header("Unit Management")]
    // public bool moveActive;
    // public int curSelectPosX;
    // public int curSelectPosY;
    // public Unit curSelectedUnit;
    // public List<Unit> AllyUnitList;
    // public List<Unit> EnemyUnitList;
    void Start()
    {
        curUnit = BattleSystem.curPlayerUnit;
    }

    void Update()
    {
        curUnit = BattleSystem.curPlayerUnit;
    }
    public void SetCurrentUnit(Unit unit)
    {
        curUnit = unit;
    }
    public void SetBoxColor()
    {
        switch(curUnit.unitName)
        {
            case "Zach":
                dialogueBox.GetComponent<Image>().color = new Color32(255,255,255,255);
                break;
            case "Jack":
                dialogueBox.GetComponent<Image>().color = new Color32(156,190,255,255);
                break;
            case "Deri":
                dialogueBox.GetComponent<Image>().color = new Color32(157,130,227,255);
                break;
            case "Ethan":
                dialogueBox.GetComponent<Image>().color = new Color32(110,155,234,255);
                break;
            case "Natasha":
                dialogueBox.GetComponent<Image>().color = new Color32(255,158,121,255);
                break;
        }
    }
    public void ResetBoxColor()
    {
        dialogueBox.GetComponent<Image>().color = new Color32(255,255,255,255);
    }
}