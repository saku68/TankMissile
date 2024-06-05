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
    private float destroyDelay = 0.1f;

    [SerializeField, Tooltip("パーティクルエフェクトの寿命（秒）")]
    private float particleLifetime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // 衝突地点と回転を取得
            Vector3 hitPosition = collision.contacts[0].point;
            Quaternion hitRotation = Quaternion.LookRotation(collision.contacts[0].normal);

            GameObject hitImpactParticle = null;

            // 衝突対象に応じたパーティクルエフェクトを生成
            if (collision.gameObject.CompareTag("Enemy"))
            {
                hitImpactParticle = Instantiate(enemyHitImpactParticle, hitPosition, hitRotation);
            }
            else
            {
                hitImpactParticle = Instantiate(groundHitImpactParticle, hitPosition, hitRotation);
            }

            if (hitImpactParticle != null)
            {
                Destroy(hitImpactParticle, particleLifetime); // パーティクルエフェクトを一定時間後に破壊
            }

            // 弾を消す
            Destroy(gameObject, destroyDelay);
        }
    }
}

