using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillDisplay : MonoBehaviour
{
    public Skill skill;
    public Text skillName;
    public Text descriptionText;
    public Text costText;
    public Image typeImage;

    // Start is called before the first frame update

    public void SetSkillHUD(Skill skill)
    {
        skillName.text = skill.skillName;
        descriptionText.text = skill.desc;
        costText.text = skill.cost.ToString();
    }
}
