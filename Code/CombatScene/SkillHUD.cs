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
            curSkill.typeSprite = Resources.Load<Sprite>("Damage Types/" + curSkill.type.ToString());
            if(curSkill.cost != 0)
            {
                costText.text = "COST: " + curSkill.cost.ToString() + " MP";
            } else if(curSkill.costHP != 0)
            {
                costText.text = "COST: " + curSkill.costHP.ToString() + " HP";
            } else if(curSkill.cost != 0 && curSkill.costHP != 0)
            {
                costText.text = "COST: " + curSkill.costHP.ToString() + " HP, " + curSkill.cost.ToString() + " MP";
            }
            SpriteRenderer spriteRend = typeSprite.GetComponent<SpriteRenderer>();
            spriteRend.sprite = curSkill.typeSprite;
        }
    }
    public void DestroyButton()
    {
        Destroy(gameObject);
    }
    public void OnSelect(BaseEventData eventData)
    {
        SetSkillHUD(BattleSystem.curPlayerUnit, buttonID);
    }
}
