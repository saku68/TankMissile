using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    public Slider hpSlider;
    public GameObject deadText;
    public GameObject scorePanel;

    void Start()
    {
        deadText.SetActive(false);
        scorePanel.SetActive(false);
    }
    public void UpdateHP(int hp)
    {
        hpSlider.value = hp;
    }
    public void UpdateMaxHp(int maxHp)
    {
        hpSlider.maxValue = maxHp;
    }
    public void SetDeadText()
    {
        deadText.SetActive(true);
    }
    public void SetScorePanel()
    {
        deadText.SetActive(false);
        scorePanel.SetActive(true);
    }
    
}
