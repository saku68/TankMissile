using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerPresenter : MonoBehaviour
{
    private ShootBullet shootBullet;
    private AngleController angleController;
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private PlayerUiManager playerUiManager;
    void Start()
    {
        shootBullet = GetComponent<ShootBullet>();
        angleController = GetComponent<AngleController>();

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
    }
    public void LetsSetDeadText()
    {
        playerUiManager.SetDeadText();
    }


    //これ達ほんとに必要なのかな
    public void LetsShoot()
    {
        shootBullet.Shoot();
    }
    // 上下左右の入力を受け取り、角度を更新する
    public void LetsUpdateAngles(float horizontal, float vertical)
    {
        angleController.UpdateAngles(horizontal, vertical);
    }


}
