using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    public Slider hpSlider;

    void Start()
    {
        
    }
    public void UpdateHP(int hp)
    {
        hpSlider.value = hp;
    }
    public void UpdateMaxHp(int maxHp)
    {
        hpSlider.maxValue = maxHp;
    }
}
