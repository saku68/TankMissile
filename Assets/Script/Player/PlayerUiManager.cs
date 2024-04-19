using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    public Slider HpSlider;

    void Start()
    {
        
    }
    public void UpdateHP(int Hp)
    {
        HpSlider.value = Hp;
    }
    public void UpdateMaxHp(int MaxHp)
    {
        HpSlider.maxValue = MaxHp;
    }
}
