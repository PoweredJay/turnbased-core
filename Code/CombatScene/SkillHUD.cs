using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillHUD : MonoBehaviour, ISelectHandler
{
    public Text skillName;
    public Text descriptionText;
    public Text costText;
    public GameObject typeSprite;
    public int buttonID;

    public string whatAmI(Unit unit, int skillNum)
    {
        return unit.Skills[skillNum].skillName;
    }
    public void SetSkillHUD(Unit unit, int skillNum)
    {
        Skill curSkill = unit.Skills[skillNum];
        if(curSkill.passive)
        {
            return;
        } else
        {
            skillName.text = curSkill.skillName;
            descriptionText.text = curSkill.desc;
            costText.text = "COST: " + curSkill.cost.ToString() + " MP";
            SpriteRenderer spriteRend = typeSprite.GetComponent<SpriteRenderer>();
            spriteRend.sprite = curSkill.typeSprite;
        }
    }
    public void OnSelect(BaseEventData eventData)
    {
        SetSkillHUD(BattleSystem.curPlayerUnit, buttonID);
    }
}
