using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private bool playerDie = false;
    public GameObject enemyPrefab1; // 敵のプレハブ
    public GameObject enemyPrefab2; // 敵のプレハブ
    public float spawnDistance1 = 10f; // 敵のスポーンする距離
    public float spawnDistance2 = 10f; // 敵のスポーンする距離
    public float spawnAngle1 = 90f; // 扇形の角度
    public float spawnAngle2 = 90f; // 扇形の角度
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDie == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // SpawnEnemy();
                SpawnEnemy1();
                SpawnEnemy2();
            }
        }
        
    }
    public void PlayerDie()
    {
        playerDie = true;
    }
    
    //敵のスポーン
    // void SpawnEnemy()
    // {
    //     // プレイヤーの位置を取得
    //     Vector3 playerPosition = transform.position;

    //     // 扇形の端の位置を計算
    //     Vector3 spawnDirection = Quaternion.Euler(0f, -spawnAngle / 2f, 0f) * transform.forward; // 扇形の左端の方向
    //     Vector3 spawnPosition = playerPosition + spawnDirection.normalized * spawnDistance; // スポーン位置

    //     // ランダムな位置を選択
    //     float randomAngle = Random.Range(-spawnAngle / 2f, spawnAngle / 2f); // 扇形の角度内でランダムな角度を選択
    //     Vector3 randomOffset = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnDistance;

    //     // プレイヤー位置からランダムな位置に湧く
    //     spawnPosition = playerPosition + randomOffset;

    //     // 敵を生成
    //     Instantiate(enemyPrefab1, spawnPosition, Quaternion.identity);
    // }
    // //湧き範囲の可視化
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Vector3 playerPosition = transform.position;

    //     // 扇形の円を描画
    //     for (int i = 0; i < 36; i++)
    //     {
    //         float angle = (float)i / 35f * spawnAngle - spawnAngle / 2f;
    //         Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
    //         Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance);
    //     }
    // }

    // 敵のスポーン
    void SpawnEnemy1()
    {
        // プレイヤーの位置を取得
        Vector3 playerPosition = transform.position;

        // 扇形の端の位置を計算
        Vector3 spawnDirection = Quaternion.Euler(0f, -spawnAngle1 / 2f, 0f) * transform.forward; // 扇形の左端の方向
        Vector3 spawnPosition = playerPosition + spawnDirection.normalized * spawnDistance1; // スポーン位置

        // ランダムな位置を選択
        float randomAngle = Random.Range(-spawnAngle1 / 2f, spawnAngle1 / 2f); // 扇形の角度内でランダムな角度を選択
        Vector3 randomOffset = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnDistance1;

        // プレイヤー位置からランダムな位置に湧く
        spawnPosition = playerPosition + randomOffset;

        // 敵を生成
        Instantiate(enemyPrefab1, spawnPosition, Quaternion.identity);
    }

    void SpawnEnemy2()
    {
        // プレイヤーの位置を取得
        Vector3 playerPosition = transform.position;

        // 扇形の端の位置を計算
        Vector3 spawnDirection = Quaternion.Euler(0f, -spawnAngle2 / 2f, 0f) * transform.forward; // 扇形の左端の方向
        Vector3 spawnPosition = playerPosition + spawnDirection.normalized * spawnDistance2; // スポーン位置

        // ランダムな位置を選択
        float randomAngle = Random.Range(-spawnAngle2 / 2f, spawnAngle2 / 2f); // 扇形の角度内でランダムな角度を選択
        Vector3 randomOffset = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnDistance2;

        // プレイヤー位置からランダムな位置に湧く
        spawnPosition = playerPosition + randomOffset;

        // 追加の敵を生成
        Instantiate(enemyPrefab2, spawnPosition, Quaternion.identity);
    }

    // 湧き範囲の可視化
void OnDrawGizmosSelected()
{
    // 敵プレハブ1の湧き範囲を黄色で描画
    Gizmos.color = Color.yellow;
    Vector3 playerPosition = transform.position;
    for (int i = 0; i < 36; i++)
    {
        float angle = (float)i / 35f * spawnAngle1 - spawnAngle1 / 2f;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance1);
    }

    // 敵プレハブ2の湧き範囲を赤色で描画
    Gizmos.color = Color.red;
    for (int i = 0; i < 36; i++)
    {
        float angle = (float)i / 35f * spawnAngle2 - spawnAngle2 / 2f;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance2);
    }
}
}
