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
    public List<GameObject> abilityButtons; // 9つの能力アップボタンのリスト
    private Vector3[] positions = new Vector3[]
    {
        new Vector3(-240, 30, 0),
        new Vector3(-10, 30, 0),
        new Vector3(215, 30, 0)
    };
    [SerializeField]
    private AudioClip SelectButtonSound1;
    public int playerMoney;
    public int playerFinalMoney;

    public int playerScore;
    public Slider hpSlider;
    public Slider sensitivitySlider;
    public GameObject deadText;
    public GameObject finalScorePanel;
    public GameObject shopPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject waveClearText;
    public GameObject waveStartTextPanel;
    public GameObject settingPanel;
    public Text waveStartText;
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
        waveStartTextPanel.SetActive(false);
        moneyText.text = "Gold:" + 0;
        //ここに監視が来るのはアリなのか？
        sensitivitySlider.OnValueChangedAsObservable()
            .Subscribe(value => PlayerUiPresenter.Instance.OnSensitivityChanged(value))
            .AddTo(this);
    }
    void Update()
    {
        //出来ればこの処理にここにいて欲しくはない
        // デス後のテキストからスコア画面への移行
        if (playerDeadFlag && !isScorePanelActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            SetScorePanel();
            UnityEngine.Debug.Log("スコア画面起動");
        }
        UpdateMoney();
    }
    public void UpdateFinalScore()
    {
        finalScore = playerScore + playerFinalMoney;
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
        SoundManager.Instance.PlaySound(SelectButtonSound1);
        shopPanel.SetActive(false);
        pauseButton.SetActive(true);
        isShopPanelActive = false;
        Time.timeScale = 1;
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
        SoundManager.Instance.PlaySound(SelectButtonSound1);
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        SoundManager.Instance.PlaySound(SelectButtonSound1);
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void SetSettingPanel()
    {
        SoundManager.Instance.PlaySound(SelectButtonSound1);
        pausePanel.SetActive(false);
        settingPanel.SetActive(true);
    }
    public void OutSettingPanel()
    {
        SoundManager.Instance.PlaySound(SelectButtonSound1);
        settingPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void SetWaveClearText()
    {
        waveClearText.SetActive(true);
    }
    public void OutWaveClearText()
    {
        waveClearText.SetActive(false);
    }
    public void ChangeWaveStartText(int waveNumber)
    {
        shopFlag = false;
        waveStartTextPanel.SetActive(true);
        waveStartText.text = "WAVE " + waveNumber;
    }
    public void OutWaveStartText()
    {
        waveStartTextPanel.SetActive(false);
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
        if (bulletRangeLevel == 30)
        {
            upShootRangeText.text = "射程増加LvMax";
        }
        else
        {
            upShootRangeText.text = "射程増加Lv" + bulletRangeLevel + ":" + bulletRangeMuch + "G";
        }
    }
    //弾のサイズ上昇の価格表示
    public void ChangeBulletSizeMuch(int bulletSizeMuch, int bulletSizeLevel)
    {
        if (bulletSizeLevel == 20)
        {
            upBulletSizeText.text = "弾のサイズLvMax";
        }
        else
        {
            upBulletSizeText.text = "弾のサイズLv" + bulletSizeLevel + ":" + bulletSizeMuch + "G";
        }
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
    public void ChangeUpShootBulletModeText(int shootBulletModeMuch, int shootBulletModeLevel)
    {
        if (shootBulletModeLevel == 3)
        {
            upShootBulletText.text = "発射弾数増加LvMax";
        }
        else
        {
            upShootBulletText.text = "発射弾数増加Lv" + shootBulletModeLevel + ":" + shootBulletModeMuch + "G";
        }
    }
    // 9つのボタンからランダムに3つ選んで表示し、指定した座標に配置するメソッド
    public void DisplayRandomAbilityButtons()
    {
        if (abilityButtons.Count < 3)
        {
            UnityEngine.Debug.LogError("Not enough ability buttons in the list.");
            return;
        }

        // リストからランダムに3つのボタンを選ぶ
        List<GameObject> selectedButtons = new List<GameObject>();
        List<int> indices = new List<int>();
        while (selectedButtons.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, abilityButtons.Count);
            if (!indices.Contains(randomIndex))
            {
                indices.Add(randomIndex);
                selectedButtons.Add(abilityButtons[randomIndex]);
            }
        }

        // 選んだボタンを指定した座標に配置する
        for (int i = 0; i < selectedButtons.Count; i++)
        {
            selectedButtons[i].SetActive(true);
            selectedButtons[i].transform.localPosition = positions[i];
        }

        // 選ばれなかったボタンは非表示にする
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            if (!indices.Contains(i))
            {
                abilityButtons[i].SetActive(false);
            }
        }
    }
}
