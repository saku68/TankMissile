using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;


public class PlayerUiManager : MonoBehaviour
{
    //シングルトンのインスタンス処理？
    public static PlayerUiManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
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
    public bool pauseFlag = false;
    public bool shopFlag = false;

    public bool isShopPanelActive = false;

    // ショップから退出するフラグ
    [SerializeField]
    private ReactiveProperty<bool> outShopFlag = new ReactiveProperty<bool>(true);
    // outShopFlagが変更されたときのイベント
    public IReadOnlyReactiveProperty<bool> OutShopFlagChanged => outShopFlag;
   
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
    //*ボタンでの*ショップ画面退出の処理
    public void OutShopButton()
    {
        shopPanel.SetActive(false);
        pauseButton.SetActive(true);
        isShopPanelActive = true;
        Time.timeScale = 1;
        shopFlag = false;
        outShopFlag.Value = false;
    }
    public void OutShopFlagCahge()
    {
        outShopFlag.Value = true;
    }
    public void OnShopFlag()
    {
        shopFlag = true;
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
