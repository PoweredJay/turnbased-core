using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public Slider mpSlider;
    public Text HPText;
    public Text MPText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "LVL" + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.curHP;
        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.curMP;
        HPText.text = unit.curHP + "/" + unit.maxHP;
        MPText.text = unit.curMP + "/" + unit.maxMP;
    }

    public void UpdateHUD(Unit unit)
    {
        hpSlider.value = unit.curHP;
        mpSlider.value = unit.curMP;
        HPText.text = unit.curHP + "/" + unit.maxHP;
        MPText.text = unit.curMP + "/" + unit.maxMP;
    }
}
