using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [SerializeField, Tooltip("敵に当たった時に生成するパーティクルエフェクトのPrefab")]
    private GameObject enemyHitImpactParticle;

    [SerializeField, Tooltip("地面に当たった時に生成するパーティクルエフェクトのPrefab")]
    private GameObject groundHitImpactParticle;

    [SerializeField, Tooltip("衝突後の弾の寿命（秒）")]
    private float destroyDelay = 1f;

    [SerializeField, Tooltip("パーティクルエフェクトの寿命（秒）")]
    private float particleLifetime = 2.0f;
    [SerializeField]
    private AudioClip impactEnemySound1;
    [SerializeField]
    private AudioClip impactGroundSound1;
    [SerializeField]
    private AudioClip impactEnemySound2;
    [SerializeField]
    private AudioClip impactGroundSound2;
    [SerializeField]
    private ShopManager shopManager;
    void Start()
    {
        shopManager = GameObject.Find("PlayerUiCanvas").GetComponent<ShopManager>();
    }
    public void ResetBulletImpactSize()
    {
        enemyHitImpactParticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        groundHitImpactParticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPosition = collision.contacts[0].point;
        Quaternion hitRotation = Quaternion.LookRotation(collision.contacts[0].normal);
        GameObject hitImpactParticle = null;
        // 衝突対象に応じたパーティクルエフェクトを生成
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hitImpactParticle = Instantiate(enemyHitImpactParticle, hitPosition, hitRotation);
            PlayHitEnemySound();
            // 弾を消す
            Destroy(hitImpactParticle, particleLifetime); // パーティクルエフェクトを一定時間後に破壊
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            hitImpactParticle = Instantiate(groundHitImpactParticle, hitPosition, hitRotation);
            PlayHitGroundSound();
            // 弾を消す
            Destroy(gameObject, destroyDelay);
            Destroy(hitImpactParticle, particleLifetime); // パーティクルエフェクトを一定時間後に破壊
        }
    }
    public void ChangeBulletImpactSize(Vector3 newSize)
    {
        enemyHitImpactParticle.transform.localScale = newSize;
        groundHitImpactParticle.transform.localScale = newSize;
    }
    public void PlayHitEnemySound()
    {
        if (shopManager.upBulletSizeLevel > 7)
        {
            SoundManager.Instance.PlaySound(impactEnemySound2);
        }
        else
        {
            SoundManager.Instance.PlaySound(impactEnemySound1);
        }
    }
    public void PlayHitGroundSound()
    {
        if (shopManager.upBulletSizeLevel > 7)
        {
            SoundManager.Instance.PlaySound(impactGroundSound2);
        }
        else
        {
            SoundManager.Instance.PlaySound(impactGroundSound1);
        }
    }
}

