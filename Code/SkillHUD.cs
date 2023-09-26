using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillHUD : MonoBehaviour
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
        skillName.text = unit.Skills[skillNum].skillName;
        descriptionText.text = unit.Skills[skillNum].desc;
        costText.text = "COST: " + unit.Skills[skillNum].cost.ToString() + " MP";
        SpriteRenderer spriteRend = typeSprite.GetComponent<SpriteRenderer>();
        spriteRend.sprite = unit.Skills[skillNum].typeSprite;
    }
}
