using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public bool playerDie = false;
    public GameObject enemyPrefab1; // 敵のプレハブ
    public float spawnDistance = 10f; // スポーンする距離
    public float spawnAngle = 90f; // 扇形の角度
    // Start is called before the first frame update
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
                SpawnEnemy();
            }
        }
        
    }
    public void PlayerDie()
    {
        playerDie = true;
    }
    
    //敵のスポーン
    void SpawnEnemy()
    {
        // プレイヤーの位置を取得
        Vector3 playerPosition = transform.position;

        // 扇形の端の位置を計算
        Vector3 spawnDirection = Quaternion.Euler(0f, -spawnAngle / 2f, 0f) * transform.forward; // 扇形の左端の方向
        Vector3 spawnPosition = playerPosition + spawnDirection.normalized * spawnDistance; // スポーン位置

        // ランダムな位置を選択
        float randomAngle = Random.Range(-spawnAngle / 2f, spawnAngle / 2f); // 扇形の角度内でランダムな角度を選択
        Vector3 randomOffset = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnDistance;

        // プレイヤー位置からランダムな位置に湧く
        spawnPosition = playerPosition + randomOffset;

        // 敵を生成
        Instantiate(enemyPrefab1, spawnPosition, Quaternion.identity);
    }
    //湧き範囲の可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 playerPosition = transform.position;

        // 扇形の円を描画
        for (int i = 0; i < 36; i++)
        {
            float angle = (float)i / 35f * spawnAngle - spawnAngle / 2f;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
            Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance);
        }
    }
}
