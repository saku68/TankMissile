using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEditor;

public class PlayerPresenter : MonoBehaviour
{
    private ShootBullet shootBullet;
    private AngleController angleController;
    private DrawArc drawArc;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        shootBullet = GetComponent<ShootBullet>();
        angleController = GetComponent<AngleController>();
        drawArc = GetComponent<DrawArc>();
    }

    public void LetsShoot()
    {
        shootBullet.Shoot();
    }

    // 上下左右の入力を受け取り、角度を更新する
    public void LetsUpdateAngles(float horizontal, float vertical)
    {
        angleController.UpdateAngles(horizontal, vertical);
    }

    // 左右回転を制御する
    public void LetsRotate(float rotationInput)
    {
        angleController.Rotate(rotationInput);
    }

    // 放物線の停止
    public void LetsOffDrawArc()
    {
        drawArc.OffDrawArc();
    }

    // Hpの増加
    public void LetsUpMaxHp(int upMaxHp)
    {
        playerManager.UpMaxHp(upMaxHp);
    }

    // 回復
    public void LetsUpHp(int upHp)
    {
        playerManager.UpHp(upHp);
    }

    // 全回復
    public void LetsUpHpMax()
    {
        playerManager.UpHpMax();
    }
    public void LetsUpMoveSpeed(float upMoveSpeed)
    {
        playerManager.UpMoveSpeed(upMoveSpeed);
    }

    // 防御力の変更
    public void UpAntiDamage(int upAntiDamage)
    {
        playerManager.antiDamage += upAntiDamage;
    }

    public void DownAntiDamage(int downAntiDamage)
    {
        playerManager.antiDamage += downAntiDamage;
    }

    // 発射レートの変更
    public void UpFirerate(float upFireRate)
    {
        shootBullet.fireRate += upFireRate;
    }

    public void DownFirerate(float downFireRate)
    {
        shootBullet.fireRate -= downFireRate;
    }

    // 射程の増減
    public void UpBulletRange(int upBulletRange)
    {
        shootBullet.bulletSpeed += upBulletRange;
    }

    public void DownBulletRange(int downBulletRange)
    {
        shootBullet.bulletSpeed -= downBulletRange;
    }

    // 発射モードの変更
    public void ChangeShootMode(int shootMode)
    {
        shootBullet.shootMode = shootMode;
    }

    // 攻撃力の変更
    public void LetsChangeBulletDamage(int newBulletDamage)
    {
        shootBullet.ChangeBulletDamage(newBulletDamage);
    }

    // 弾のサイズの変更
    public void LetsChangeBulletSize(Vector3 newSize)
    {
        shootBullet.ChangeBulletSize(newSize);
    }
}