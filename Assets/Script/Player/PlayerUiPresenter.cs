using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerUiPresenter : MonoBehaviour
{
    private PlayerUiManager playerUiManager;
    private EnemySpawn enemySpawn;
    void Start()
    {
        playerUiManager = GetComponent<PlayerUiManager>();
        enemySpawn = GetComponent<EnemySpawn>();

        // ショップから退出したときのイベントを購読し、EnemySpawnクラスの処理を実行
        _ = playerUiManager.OutShopFlagChanged
        .Where(flag => !flag) // outShopFlagがfalseになったときのみ購読
        .Subscribe(_ => {
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
    }
}
