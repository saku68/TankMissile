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

    public int playerMoney;

    public int playerScore;
    public Slider hpSlider;
    public GameObject deadText;
    public GameObject finalScorePanel;
    public GameObject shopPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject waveClearText;
    public Text finalScoreText;
    public Text moneyText;
    public Text fireRateText;
    public Text upMaxHpText;
    public Text upHpText;

    public Text upShootRangeText;

    public Text upShootBulletText;

    public Text upBulletSizeText;

    public Text upDamageText;

    public Text upDifenceText;
    public Text upMoveSpeedText;

    public int finalScore;
    public bool pauseFlag = false;
    public bool shopFlag = false;
    public bool isScorePanelActive;
    public bool isShopPanelActive = false;
    public bool playerDeadFlag;

    // ショップから退出するフラグ
    [SerializeField]
    private ReactiveProperty<bool> outShopFlag = new ReactiveProperty<bool>(true);
    // outShopFlagが変更されたときのイベント
    public IReadOnlyReactiveProperty<bool> OutShopFlagChanged => outShopFlag;

    void Start()
    {
        deadText.SetActive(false);
        finalScorePanel.SetActive(false);
        pauseButton.SetActive(true);
        waveClearText.SetActive(false);
        moneyText.text = "Gold:" + 0;
    }
    void Update()
    {
        //出来ればこの処理にここにいて欲しくはない
        // デス後のテキストからスコア画面への移行
        if (playerDeadFlag && !isScorePanelActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            SetScorePanel();
            Debug.Log("スコア画面起動");
        }
        UpdateMoney();
    }
    public void UpdateFinalScore()
    {
        finalScore = playerScore + playerMoney;
        finalScoreText.text = "Score:" + finalScore;
    }
    public void UpdateMoney()
    {
        moneyText.text = "Gold:" + playerMoney;
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
        isScorePanelActive = true;
        deadText.SetActive(false);
        pauseButton.SetActive(false);
        UpdateFinalScore();
        finalScorePanel.SetActive(true);
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
    //購入処理
    public void BuyAnyAbility(int much)
    {
        playerMoney -= much;
    }
    //発射レートの価格表示
    public void ChangeFireRateMuch(int fireRatemuch, int fireRateLevel)
    {
        fireRateText.text = "連射速度Lv" + fireRateLevel + ":" + fireRatemuch + "G";
    }
    //最大HP増加の価格表示
    public void ChangeUpMaxHpMuch(int upMaxHpMuch, int upMaxHpLevel)
    {
        upMaxHpText.text = "最大HPLv" + upMaxHpLevel + ":" + upMaxHpMuch + "G";
    }
    //射程増加の価格表示
    public void ChangeBulletRangeMuch(int bulletRangeMuch, int bulletRangeLevel)
    {
        upShootRangeText.text = "射程増加Lv" + bulletRangeLevel + ":" + bulletRangeMuch + "G";
    }
    //弾のサイズ上昇の価格表示
    public void ChangeBulletSizeMuch(int bulletSizeMuch, int bulletSizeLevel)
    {
        upBulletSizeText.text = "弾のサイズLv" + bulletSizeLevel + ":" + bulletSizeMuch + "G";
    }
    //攻撃力増加の価格表示
    public void ChangeBulletDamageMuch(int bulletDamageMuch, int bulletDamageLevel)
    {
        upDamageText.text = "威力増加Lv" + bulletDamageLevel + ":" + bulletDamageMuch + "G";
    }
    //防御力の価格表示
    public void ChangeAntiDamageMuch(int antiDamageMuch, int antiDamageLevel)
    {
        upDifenceText.text = "防御増加Lv" + antiDamageLevel + ":" + antiDamageMuch + "G";
    }
    //移動速度の価格表示
    public void ChangeMoveSpeedMuch(int moveSpeedMuch, int moveSpeedLevel)
    {
        upMoveSpeedText.text = "移動速度Lv" + moveSpeedLevel + ":" + moveSpeedMuch + "G";
    }
}
