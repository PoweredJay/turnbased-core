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

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "LVL" + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.curHP;
        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.curMP;
    }

    public void UpdateHUD(Unit unit)
    {
        hpSlider.value = unit.curHP;
        mpSlider.value = unit.curMP;
    }

    public void SetHP(int HP)
    {
        hpSlider.value = HP;
    }
    public void SetMP(int MP)
    {
        mpSlider.value = MP;
    }
}
