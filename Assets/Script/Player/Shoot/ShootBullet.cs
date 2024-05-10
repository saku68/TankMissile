using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Diagnostics.CodeAnalysis;

public class ShootBullet : MonoBehaviour
{
    public int shootMode = 1;
    public float fireRate = 1f; // 発射速度（1秒あたりの発射回数）

    private float nextFireTime = 0f; // 次に発射できる時刻
    public float offset = 0.5f; // 発射位置の間隔
    public float angle = 15f; // 発射角度

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
        //どうにかUniRxでUpdateから出せないか
        // 弾の初速度を更新
        shootVelocity = barrelObject.transform.up * bulletSpeed;
        // 弾の生成座標を更新
        instantiatePosition = barrelObject.transform.position;
    }
    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            switch (shootMode)
            {
                case 1:
                    ShootSingle();
                    break;
                case 2:
                    ShootHorizontal();
                    break;
                case 3:
                    ShootTriple();
                    break;
            }
        }
    }
    private void ShootSingle()
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

    private void ShootHorizontal()
    {
        Vector3 positionOffsetRight = barrelObject.transform.right * offset; // バレルの右方向へのオフセット

        // 弾を生成して飛ばす
        GameObject obj1 = Instantiate(bulletPrefab, instantiatePosition + positionOffsetRight, Quaternion.identity);
        GameObject obj2 = Instantiate(bulletPrefab, instantiatePosition - positionOffsetRight, Quaternion.identity);

        Rigidbody rid1 = obj1.GetComponent<Rigidbody>();
        Rigidbody rid2 = obj2.GetComponent<Rigidbody>();

        rid1.AddForce(shootVelocity * rid1.mass, ForceMode.Impulse);
        rid2.AddForce(shootVelocity * rid2.mass, ForceMode.Impulse);

        // 次の発射可能時刻を更新する
        nextFireTime = Time.time + 1f / fireRate;
        // 5秒後に消える
        Destroy(obj1, 5.0F);
        Destroy(obj2, 5.0F);
    }

    private void ShootTriple()
    {
        Vector3 positionOffsetRight = barrelObject.transform.right * offset; // バレルの右方向へのオフセット

        // バレルの向きに基づいて角度を回転させる
        Quaternion rotationRight = Quaternion.AngleAxis(angle, instantiatePosition);
        Quaternion rotationLeft = Quaternion.AngleAxis(-angle, instantiatePosition);

        // 弾を生成して飛ばす
        GameObject obj1 = Instantiate(bulletPrefab, instantiatePosition + positionOffsetRight, Quaternion.identity);
        GameObject obj2 = Instantiate(bulletPrefab, instantiatePosition - positionOffsetRight, Quaternion.identity);
        GameObject obj3 = Instantiate(bulletPrefab, instantiatePosition, Quaternion.identity);

        Rigidbody rid1 = obj1.GetComponent<Rigidbody>();
        Rigidbody rid2 = obj2.GetComponent<Rigidbody>();
        Rigidbody rid3 = obj3.GetComponent<Rigidbody>();

        rid1.AddForce(rotationRight * shootVelocity * rid1.mass, ForceMode.Impulse);
        rid2.AddForce(rotationLeft * shootVelocity * rid2.mass, ForceMode.Impulse);
        rid3.AddForce(shootVelocity * rid3.mass, ForceMode.Impulse);

        // 次の発射可能時刻を更新する
        nextFireTime = Time.time + 1f / fireRate;
        // 5秒後に消える
        Destroy(obj1, 5.0F);
        Destroy(obj2, 5.0F);
        Destroy(obj3, 5.0F);
    }
}