using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;

public class ShootBullet : MonoBehaviour
{
    public int shootMode = 1; // 発射モード切替
    public float fireRate = 1f; // 発射レート（1秒あたりの発射回数）
    public float offset = 0.5f; // 同時発射位置の間隔
    public float angle = 15f; // 同時発射角度
    private float nextFireTime = 0f; // 次に発射できる時刻

    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject bulletPrefab;

    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    private Vector3 instantiatePosition;

    public Vector3 InstantiatePosition => instantiatePosition;

    [Range(3.0F, 100.0F), Tooltip("弾の射出する速さ")]
    public float bulletSpeed = 9.0F;

    private Vector3 shootVelocity;

    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
    }
    private Dameger dameger;
    private BulletImpact bulletImpact;

    void Start()
    {
        dameger = bulletPrefab.GetComponent<Dameger>();
        bulletImpact = bulletPrefab.GetComponent<BulletImpact>();
        dameger.damage2 = 1;
        dameger = bulletPrefab.GetComponent<Dameger>();
        bulletPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        bulletImpact.ResetBulletImpactSize();
    }

    void Update()
    {
        // 弾の初速度を更新
        shootVelocity = barrelObject.transform.forward * bulletSpeed;
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
        GameObject obj = Instantiate(bulletPrefab, instantiatePosition, barrelObject.transform.rotation);
        Rigidbody rid = obj.GetComponent<Rigidbody>();
        rid.AddForce(shootVelocity * rid.mass, ForceMode.Impulse);
        nextFireTime = Time.time + 1f / fireRate;
        Destroy(obj, 5.0F);
    }

    private void ShootHorizontal()
    {
        Vector3 positionOffsetRight = barrelObject.transform.right * offset;

        GameObject obj1 = Instantiate(bulletPrefab, instantiatePosition + positionOffsetRight, barrelObject.transform.rotation);
        GameObject obj2 = Instantiate(bulletPrefab, instantiatePosition - positionOffsetRight, barrelObject.transform.rotation);

        Rigidbody rid1 = obj1.GetComponent<Rigidbody>();
        Rigidbody rid2 = obj2.GetComponent<Rigidbody>();

        rid1.AddForce(shootVelocity * rid1.mass, ForceMode.Impulse);
        rid2.AddForce(shootVelocity * rid2.mass, ForceMode.Impulse);

        nextFireTime = Time.time + 1f / fireRate;
        Destroy(obj1, 5.0F);
        Destroy(obj2, 5.0F);
    }

    private void ShootTriple()
    {
        Vector3 positionOffsetRight = barrelObject.transform.right * offset;

        Quaternion rotationRight = barrelObject.transform.rotation * Quaternion.Euler(0, angle, 0);
        Quaternion rotationLeft = barrelObject.transform.rotation * Quaternion.Euler(0, -angle, 0);

        GameObject obj1 = Instantiate(bulletPrefab, instantiatePosition + positionOffsetRight, rotationRight);
        GameObject obj2 = Instantiate(bulletPrefab, instantiatePosition - positionOffsetRight, rotationLeft);
        GameObject obj3 = Instantiate(bulletPrefab, instantiatePosition, barrelObject.transform.rotation);

        Rigidbody rid1 = obj1.GetComponent<Rigidbody>();
        Rigidbody rid2 = obj2.GetComponent<Rigidbody>();
        Rigidbody rid3 = obj3.GetComponent<Rigidbody>();

        rid1.AddForce(rotationRight * shootVelocity * rid1.mass, ForceMode.Impulse);
        rid2.AddForce(rotationLeft * shootVelocity * rid2.mass, ForceMode.Impulse);
        rid3.AddForce(shootVelocity * rid3.mass, ForceMode.Impulse);

        nextFireTime = Time.time + 1f / fireRate;
        Destroy(obj1, 5.0F);
        Destroy(obj2, 5.0F);
        Destroy(obj3, 5.0F);
    }

    public void ChangeBulletDamage(int newDamage)
    {
        dameger.damage2 += newDamage;
    }

    public void ChangeBulletSize(Vector3 newSize)
    {
        bulletImpact.ChangeBulletImpactSize(newSize);
        bulletPrefab.transform.localScale = newSize;
    }
}