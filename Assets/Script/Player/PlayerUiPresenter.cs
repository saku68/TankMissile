using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;
using System;

public class PlayerUiPresenter : MonoBehaviour
{
    public static PlayerUiPresenter Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private PlayerUiManager playerUiManager;
    private PlayerManager playerManager;
    private ShopManager shopManager;
    private EnemySpawn enemySpawn;
    private AngleController angleController;
    private bool pauseFlag;
    void Start()
    {
        pauseFlag = false;
        playerUiManager = GetComponent<PlayerUiManager>();
        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        shopManager = GetComponent<ShopManager>();
        angleController = GameObject.Find("Player").GetComponent<AngleController>();

        //PlayerのHpを監視
        _ = playerManager.Hp
        .Subscribe(x =>
        {
            //Viewに反映
            playerUiManager.UpdateHp(playerManager.Hp.Value);
        }).AddTo(this);
        //MaxHpも更新
        _ = playerManager.MaxHp
        .Subscribe(x =>
        {
            //Viewに反映
            playerUiManager.UpdateMaxHp(playerManager.MaxHp.Value);
        }).AddTo(this);


        // ショップから退出したときのイベントを購読し、EnemySpawnクラスの処理を実行
        _ = playerUiManager.OutShopFlagChanged
        .Where(flag => !flag) // outShopFlagがfalseになったときのみ購読
        .Subscribe(_ =>
        {
            EnemySpawn.Instance.OnWaveStart();
            PlayerUiManager.Instance.OutShopFlagCahge();
        })
        .AddTo(this); // Dispose管理

        playerUiManager = GetComponent<PlayerUiManager>();

        // 倒した敵のスコアの変化を検知し、PlayerUiManagerに通知
        _ = EnemySpawn.Instance.ScoreReactive
        .Subscribe(score => playerUiManager.playerScore = score)
        .AddTo(this);
        // 倒した敵から得たお金の変化を検知し、PlayerUiManagerに通知
        _ = EnemySpawn.Instance.MoneyReactive
        .Subscribe(money => playerUiManager.playerFinalMoney = money)
        .AddTo(this);
    }
    //左右感度の値の受け渡し
    public void OnSensitivityChanged(float newSensitivity)
    {
        if (angleController != null)
        {
            angleController.SetSensitivity(newSensitivity);
        }
    }

    public void LetsSetDeadText()
    {
        playerUiManager.SetDeadText();
    }
    public void LetsOnShopFlag()
    {
        playerUiManager.OnShopFlag();
    }
    public void OnEsckey()
    {
        //*エスケープでの*ショップ退出の処理
        if (playerUiManager.shopFlag && playerUiManager.IsShopPanelActive())
        {
            playerUiManager.OutShopButton();
            UnityEngine.Debug.Log("Escでショップを退出した");
        }

        if (!pauseFlag && !playerUiManager.IsShopPanelActive() && !playerUiManager.shopFlag)
        {
            playerUiManager.PauseGame();
            pauseFlag = true;
            UnityEngine.Debug.Log("Escでポーズした");
        }
        else if (pauseFlag)
        {
            playerUiManager.ResumeGame();
            pauseFlag = false;
            UnityEngine.Debug.Log("Escでポーズを退出した");
        }
    }
    public void LetsSetWaveClearText()
    {
        playerUiManager.SetWaveClearText();
    }
    public void LetsOutWaveSetShopPanel()
    {
        playerUiManager.OutWaveClearText();
        playerUiManager.SetShopPanel();
        playerUiManager.DisplayRandomAbilityButtons();
        //表示の初期化
        //この引数の渡し方なんか嫌だ
        playerUiManager.ChangeFireRateMuch(shopManager.fireRateMuch, shopManager.fireRateLevel);
        playerUiManager.ChangeUpMaxHpMuch(shopManager.upMaxHpMuch, shopManager.upMaxHpLevel);
        playerUiManager.ChangeBulletRangeMuch(shopManager.upBulletRangeMuch, shopManager.bulletRangeLevel);
        playerUiManager.ChangeBulletSizeMuch(shopManager.upBulletSizeMuch, shopManager.upBulletSizeLevel);
        playerUiManager.ChangeBulletDamageMuch(shopManager.upBulletDamageMuch, shopManager.upBulletDamageLevel);
        playerUiManager.ChangeAntiDamageMuch(shopManager.antiDamageMuch, shopManager.antiDamageLevel);
        playerUiManager.ChangeMoveSpeedMuch(shopManager.moveSpeedMuch, shopManager.moveSpeedLevel);
        playerUiManager.ChangeUpShootBulletModeText(shopManager.upShootBulletModeMuch, shopManager.upShootBulletModeLevel);
    }
    public void LetsChangePlayerDieFlag()
    {
        playerUiManager.playerDeadFlag = true;
    }
    public void LetsBuyAnyAbility(int much)
    {
        playerUiManager.BuyAnyAbility(much);
    }
    public void LetsChangeFireRateMuch(int fireRateMuch, int fireRateLevel)
    {
        playerUiManager.ChangeFireRateMuch(fireRateMuch, fireRateLevel);
    }
    public void LetsChangeUpMaxHpMuch(int upMaxHpMuch, int upMaxHpLevel)
    {
        playerUiManager.ChangeUpMaxHpMuch(upMaxHpMuch, upMaxHpLevel);
    }
    public void LetsChangeBulletRangeMuch(int bulletRangeMuch, int bulletRangeLevel)
    {
        playerUiManager.ChangeBulletRangeMuch(bulletRangeMuch, bulletRangeLevel);
    }
    public void LetsChangeBulletSizeMuch(int bulletSizeMuch, int bulletSizeLevel)
    {
        playerUiManager.ChangeBulletSizeMuch(bulletSizeMuch, bulletSizeLevel);
    }
    public void LetsChangeBulletDamageMuch(int bulletDamageMuch, int bulletDamageLevel)
    {
        playerUiManager.ChangeBulletDamageMuch(bulletDamageMuch, bulletDamageLevel);
    }
    public void LetsChangeAntiDamageMuch(int antiDamageMuch, int antiDamageLevel)
    {
        playerUiManager.ChangeAntiDamageMuch(antiDamageMuch, antiDamageLevel);
    }
    public void LetsChangeMoveSpeedMuch(int moveSpeedMuch, int moveSpeedLevel)
    {
        playerUiManager.ChangeMoveSpeedMuch(moveSpeedMuch, moveSpeedLevel);
    }
    public void LetsWaveStartText(int waveNumber)
    {
        playerUiManager.ChangeWaveStartText(waveNumber);
    }
    public void LetsUpShootBulletModeText(int shootBulletModeMuch, int shootBulletModeLevel)
    {
        playerUiManager.ChangeUpShootBulletModeText(shootBulletModeMuch, shootBulletModeLevel);
    }
    public void LetsOutWaveStartText()
    {
        playerUiManager.OutWaveStartText();
    }
}
