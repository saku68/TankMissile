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
    void Start()
    {
        shootBullet = GetComponent<ShootBullet>();
        angleController = GetComponent<AngleController>();
    }
    // 上下左右の入力を受け取り、角度を更新する
    public void LetsUpdateAngles(float horizontal, float vertical)
    {
        angleController.UpdateAngles(horizontal, vertical);
    }
    //この渡し方なんか微妙
    public void LetsShoot()
    {
        shootBullet.Shoot();
    }

}
