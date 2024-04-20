using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    bool AllEnemiesDestroyed()
    {
        // 敵が存在するかどうかを確認し、すべての敵が消滅したら true を返す
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }   
    private PlayerUiManager playerUiManager;
    [SerializeField]
    private bool playerDie = false;
    public bool spawnWaveFlag = true;
    public bool shopFlag = false;
    public GameObject enemyPrefab1; // 敵のプレハブ
    public GameObject enemyPrefab2; // 敵のプレハブ
    public float spawnDistance1 = 25f; // 敵のスポーンする距離
    public float spawnDistance2 = 80f; // 敵のスポーンする距離
    public float spawnAngle1 = 110f; // 扇形の角度
    public float spawnAngle2 = 90f; // 扇形の角度
    void Start()
    {
        playerDie = false;
        playerUiManager = GameObject.Find("PlayerUiCanvas").GetComponent<PlayerUiManager>();
        StartCoroutine(SpawnEnemiesPeriodically1());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDie == true && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            playerUiManager.SetScorePanel();
        }
        if (spawnWaveFlag == false)
        {
            if (AllEnemiesDestroyed())
            {
                if (shopFlag == false)
                {
                    Debug.Log("Waveクリア");
                    playerUiManager.SetShopPanel();
                    shopFlag = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopFlag == true)
            {
                playerUiManager.OutShopPanel();
                shopFlag = false;
                spawnWaveFlag = true;
            }
        }
    }
    public void OutShopButton()
    {
        playerUiManager.OutShopPanel();
        shopFlag = false;
        spawnWaveFlag = true;
    }

    //一定時間敵を湧かせる
    IEnumerator SpawnEnemiesPeriodically1()
    {
        Debug.Log("Wave1開始");
        spawnWaveFlag = true;

        // 60秒間の時間計測
        float elapsedTime = 0f;

        // 60秒経過するかプレイヤーが死ぬまで無限ループ
        while (!playerDie && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitTime);

            // 敵を生成する
            SpawnEnemy1();
            SpawnEnemy2();

            // 経過時間を加算
            elapsedTime += waitTime;
        }
        Debug.Log("Wave1終了");
        spawnWaveFlag = false;
    }

    public void PlayerDie()
    {
        playerDie = true;
        if (spawnWaveFlag == true)
        {
            StopCoroutine(SpawnEnemiesPeriodically1());
            // spawnWaveFlag = false;
        }
    }

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
        // 敵プレハブ1の湧き範囲を赤色で描画
        Gizmos.color = Color.red;
        Vector3 playerPosition = transform.position;
        for (int i = 0; i < 36; i++)
        {
        float angle = (float)i / 35f * spawnAngle1 - spawnAngle1 / 2f;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance1);
        }

        // 敵プレハブ2の湧き範囲を黄色で描画
        Gizmos.color = Color.yellow;
        for (int i = 0; i < 36; i++)
        {
        float angle = (float)i / 35f * spawnAngle2 - spawnAngle2 / 2f;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance2);
        }
    }
}
