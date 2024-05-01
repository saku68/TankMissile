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
    private DrawArc drawArc;
    private PlayerManager playerManager;
    // [SerializeField]
    // private PlayerUiManager playerUiManager;
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        shootBullet = GetComponent<ShootBullet>();
        angleController = GetComponent<AngleController>();
        drawArc = GetComponent<DrawArc>();
    }
    //これ達ほんとに必要なんかな
    public void LetsShoot()
    {
        shootBullet.Shoot();
    }
    // 上下左右の入力を受け取り、角度を更新する
    public void LetsUpdateAngles(float horizontal, float vertical)
    {
        angleController.UpdateAngles(horizontal, vertical);
    }
    //放物線の停止
    public void LetsOffDrawArc()
    {
        drawArc.OffDrawArc();
    }



    //発射レートの変更
    public void UpFirerate(float upFireRate)
    {
        shootBullet.fireRate += upFireRate;
    }
    public void DownFirerate(float downFireRate)
    {
        shootBullet.fireRate -= downFireRate;
    }
    //Hpの増加
    public void LetsUpMaxHp(int upMaxHp)
    {
        playerManager.UpMaxHp(upMaxHp);
    }
    public void LetsUpHp(int upHp)
    {
        playerManager.UpHp(upHp);
    }
    public void UpBulletRange(int upBulletRange)
    {
        shootBullet.bulletSpeed += upBulletRange;
    }
    public void DownBulletRange(int downBulletRange)
    {
        shootBullet.bulletSpeed -= downBulletRange;
    }

}

