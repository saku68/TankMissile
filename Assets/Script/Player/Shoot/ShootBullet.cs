using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class ShootBullet : MonoBehaviour
{
    public float fireRate = 1f; // 発射速度（1秒あたりの発射回数）

    private float nextFireTime = 0f; // 次に発射できる時刻

    /// 弾のPrefab
    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject bulletPrefab;

    /// 砲身のオブジェクト
    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    /// 弾を生成する位置情報
    private Vector3 instantiatePosition;

    /// 弾の生成座標(読み取り専用)
    public Vector3 InstantiatePosition => instantiatePosition;

    /// 弾の速さ
    [Range(3.0F, 100.0F), Tooltip("弾の射出する速さ")]
    public float bulletSpeed = 16.0F;

    /// 弾の初速度
    private Vector3 shootVelocity;

    /// 弾の初速度(読み取り専用)
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
    }
    void Update()
    {
        //どうにかUniRxでUpdateから出せないか？
        // 弾の初速度を更新
        shootVelocity = barrelObject.transform.up * bulletSpeed;
        // 弾の生成座標を更新
        instantiatePosition = barrelObject.transform.position;
    }
    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            // 弾を生成して飛ばす
            GameObject obj = Instantiate(bulletPrefab, instantiatePosition, Quaternion.identity);
            Rigidbody rid = obj.GetComponent<Rigidbody>();
            rid.AddForce(shootVelocity * rid.mass, ForceMode.Impulse);
            // 次の発射可能時刻を更新する
            nextFireTime = Time.time + 1f / fireRate;
            // 5秒後に消える
            Destroy(obj, 5.0F);
        }
    }
}