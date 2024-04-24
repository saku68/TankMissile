using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class ShootPresenter : MonoBehaviour
{
    private ShootBullet shootBullet;
    void Start()
    {
        shootBullet = GetComponent<ShootBullet>();
    }
    public void LetsShoot()
    {
        shootBullet.Shoot();
    }

}
