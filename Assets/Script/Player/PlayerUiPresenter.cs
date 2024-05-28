using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerUiPresenter : MonoBehaviour
{
    private PlayerUiManager playerUiManager;
    private PlayerManager playerManager;
    private ShopManager shopManager;
    private EnemySpawn enemySpawn;
    void Start()
    {
        playerUiManager = GetComponent<PlayerUiManager>();
        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        shopManager = GetComponent<ShopManager>();

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
        .Subscribe(money => playerUiManager.playerMoney = money)
        .AddTo(this);
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
        }
        else
        {
            playerUiManager.PauseGame();
            playerUiManager.pauseFlag = true;
        }

        if (playerUiManager.pauseFlag)
        {
            playerUiManager.ResumeGame();
            playerUiManager.pauseFlag = false;
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

        //表示の初期化
        //この引数の渡し方なんか嫌だ
        playerUiManager.ChangeFireRateMuch(shopManager.fireRateMuch, shopManager.fireRateLevel);
        playerUiManager.ChangeUpMaxHpMuch(shopManager.upMaxHpMuch, shopManager.upMaxHpLevel);
        playerUiManager.ChangeBulletRangeMuch(shopManager.upBulletRangeMuch, shopManager.bulletRangeLevel);
        playerUiManager.ChangeBulletSizeMuch(shopManager.upBulletSizeMuch, shopManager.upBulletSizeLevel);
        playerUiManager.ChangeBulletDamageMuch(shopManager.upBulletDamageMuch, shopManager.upBulletDamageLevel);
        playerUiManager.ChangeAntiDamageMuch(shopManager.antiDamageMuch, shopManager.antiDamageLevel);
        playerUiManager.ChangeMoveSpeedMuch(shopManager.moveSpeedMuch, shopManager.moveSpeedLevel);
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
    public void LetsOutWaveStartText()
    {
        playerUiManager.OutWaveStartText();
    }
}
