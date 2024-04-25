using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    private EnemySpawn enemySpawn;
    public Slider hpSlider;
    public GameObject deadText;
    public GameObject scorePanel;
    public GameObject shopPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject waveClearText;
    public Text scoreText;
    public Text moneyText;
    public int finalScore;

    public bool isShopPanelActive = false;
    void Start()
    {
        enemySpawn= GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
        deadText.SetActive(false);
        scorePanel.SetActive(false);
        pauseButton.SetActive(true);
        waveClearText.SetActive(false);
        moneyText.text = "Gold:" + 0;
    }
    void Update()
    {
        UpdateMoney();
    }
    public void UpdateScore()
    {
        finalScore = enemySpawn.playerScore + enemySpawn.playerMoney;
        scoreText.text = "Score:" + finalScore;
    }
    public void UpdateMoney()
    {
        moneyText.text = "Gold:" + enemySpawn.playerMoney;
    }
    public void UpdateHp(int hp)
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
        pauseButton.SetActive(false);
        UpdateScore();
        scorePanel.SetActive(true);
    }
    public void SetShopPanel()
    {
        shopPanel.SetActive(true);
        pauseButton.SetActive(false);
        isShopPanelActive = true;
        Time.timeScale = 0;
    }
    public bool IsShopPanelActive()
    {
        return isShopPanelActive;
    }
    public void OutShopPanel()
    {
        shopPanel.SetActive(false);
        pauseButton.SetActive(true);
        isShopPanelActive = true;
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void SetWaveClearText()
    {
        waveClearText.SetActive(true);
    }
    public void OutWaveClearText()
    {
        waveClearText.SetActive(false);
    }
}
