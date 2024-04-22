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
    public bool pauseFlag = false;

    public List<GameObject> enemyPrefabs; // 敵のプレハブのリスト
    public float spawnDistance1 = 25f; // 敵のスポーンする距離
    public float spawnDistance2 = 80f; // 敵のスポーンする距離
    public float spawnAngle1 = 110f; // 扇形の角度
    public float spawnAngle2 = 90f; // 扇形の角度
    private bool isWaveClearShopOpen = false;//一度だけ実行するためのフラグ
    public float waveNumber = 1;
    void Start()
    {
        playerDie = false;
        playerUiManager = GameObject.Find("PlayerUiCanvas").GetComponent<PlayerUiManager>();
        StartCoroutine(SpawnEnemiesPeriodically1());
    }


    void Update()
    {
        //デス後のテキストからスコア画面への移行
        if (playerDie == true && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            playerUiManager.SetScorePanel();
        }

        //ウェーブクリアの判定
        if (AllEnemiesDestroyed() && playerDie == false && shopFlag == false && spawnWaveFlag == false)
        {
            Debug.Log("Waveクリア");
            StartCoroutine(WaveClearOpenShop());
            shopFlag = true;
        }
        
        //エスケープキーの操作
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //*エスケープでの*ショップ退出の処理
            if (shopFlag == true)
            {
                OutShopButton();
                }else{
                playerUiManager.PauseGame();
                pauseFlag = true;
            }

            if (pauseFlag == true)
            {
                playerUiManager.ResumeGame();
                pauseFlag = false;
            }
        }
    }

    //ウェーブクリアメッセージとショップ画面への移行
    IEnumerator WaveClearOpenShop()
    {
        if (!isWaveClearShopOpen)
        {
            isWaveClearShopOpen = true;
            playerUiManager.SetWaveClearText();
            yield return new WaitForSeconds(5f);
            playerUiManager.OutWaveClearText();
            playerUiManager.SetShopPanel();
            isWaveClearShopOpen = false;
        }
    }
    //*ボタンでの*ショップ画面退出の処理
    public void OutShopButton()
    {
        playerUiManager.OutShopPanel();
        shopFlag = false;
        spawnWaveFlag =true;
        waveNumber = waveNumber + 1;//ショップの退出でwave進行
        OnWaveStart();
        
    }
    public void OnWaveStart()
    {
        spawnWaveFlag = true;
        switch(waveNumber)
        {
            case 2:
            StartCoroutine(SpawnEnemiesPeriodically2());
            break;
            case 3:
            StartCoroutine(SpawnEnemiesPeriodically3());
            break;
            case 4:
            StartCoroutine(BossSpawnEnemiesPeriodically1());
            break;
        }
    }

    //ウェーブ１
    IEnumerator SpawnEnemiesPeriodically1()
    {
        Debug.Log("Wave" + waveNumber + "開始");
        spawnWaveFlag = true;

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDie && elapsedTime < 5f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime1);

            SpawnEnemy1(0);//手前の湧き
            // SpawnEnemy2(0);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" +waveNumber +"終了");
        spawnWaveFlag = false;
    }

    //ウェーブ２
    IEnumerator SpawnEnemiesPeriodically2()
    {
        Debug.Log("Wave" + waveNumber + "開始");
        spawnWaveFlag = true;

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDie && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = Random.Range(2f, 4f);
            yield return new WaitForSeconds(waitTime1);

            SpawnEnemy1(0);//手前の湧き
            SpawnEnemy2(1);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" +waveNumber +"終了");
        spawnWaveFlag = false;
    }

    //ウェーブ３
    IEnumerator SpawnEnemiesPeriodically3()
    {
        Debug.Log("Wave" + waveNumber + "開始");
        spawnWaveFlag = true;

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDie && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = Random.Range(3f, 5f);
            yield return new WaitForSeconds(waitTime1);

            SpawnEnemy1(2);//手前の湧き
            SpawnEnemy2(3);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" +waveNumber +"終了");
        spawnWaveFlag = false;
    }

    //デス検知で湧き停止
    public void PlayerDie()
    {
        playerDie = true;
        if (spawnWaveFlag == true)
        {
            StopCoroutine(SpawnEnemiesPeriodically1()); //ウェーブ１停止
            StopCoroutine(SpawnEnemiesPeriodically2()); //ウェーブ２停止
            StopCoroutine(SpawnEnemiesPeriodically3()); //ウェーブ３停止
            // spawnWaveFlag = false;
        }
    }

    //手前の範囲での敵のスポーン
    public void SpawnEnemy1(int enemyNumber)
    {
        if (enemyNumber >= 0 && enemyNumber < enemyPrefabs.Count)
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

            // 指定された敵のプレハブを生成
            GameObject enemyPrefab = enemyPrefabs[enemyNumber];
            if (enemyPrefab != null)
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("指定された敵のプレハブが存在しません。");
            }
        }
    }
        // 奥の範囲での敵のスポーン
        public void SpawnEnemy2(int enemyNumber)
    {
        if (enemyNumber >= 0 && enemyNumber < enemyPrefabs.Count)
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

            // 指定された敵のプレハブを生成
            GameObject enemyPrefab = enemyPrefabs[enemyNumber];
            if (enemyPrefab != null)
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {   
                Debug.LogWarning("指定された敵のプレハブが存在しません。");
            }
        }
    }   
    //ボスのスポーン
    public void SpawnBossEnemy(int enemyNumber)
    {
        if (enemyNumber >= 0 && enemyNumber < enemyPrefabs.Count)
        {
        // 指定された座標に敵を生成
        GameObject enemyPrefab = enemyPrefabs[enemyNumber];
        Vector3 bossSpawnPosition = new Vector3(0, 3, 30);
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, bossSpawnPosition, Quaternion.identity);
        }
        else
        {   
            Debug.LogWarning("指定された敵のプレハブが存在しません。");
        }
        }
    }
    //ボスウェーブ１
    IEnumerator BossSpawnEnemiesPeriodically1()
    {
        Debug.Log("Wave" + waveNumber + "開始");
        spawnWaveFlag = true;
        yield return new WaitForSeconds(5f);
        SpawnBossEnemy(4);
        Debug.Log("Wave" + waveNumber + "終了");
        spawnWaveFlag = false;
    }

    // 湧き範囲の可視化
    void OnDrawGizmosSelected()
    {
        // 手前の湧き範囲を赤色で描画
        Gizmos.color = Color.red;
        Vector3 playerPosition = transform.position;
        for (int i = 0; i < 36; i++)
        {
        float angle = (float)i / 35f * spawnAngle1 - spawnAngle1 / 2f;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance1);
        }
    

        // 奥の湧き範囲を黄色で描画
        Gizmos.color = Color.yellow;
        for (int i = 0; i < 36; i++)
        {
        float angle = (float)i / 35f * spawnAngle2 - spawnAngle2 / 2f;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Gizmos.DrawLine(playerPosition, playerPosition + direction * spawnDistance2);
        }
    }
}
