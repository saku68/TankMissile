using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class EnemySpawn : MonoBehaviour
{
    //シングルトンのインスタンス処理？
    public static EnemySpawn Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    public int deadEnemyMoney;
    [SerializeField]
    public int deadEnemyScore;
    // ReactivePropertyを使用して値の変化を検知
    private ReactiveProperty<int> scoreReactive = new ReactiveProperty<int>();
    private ReactiveProperty<int> moneyReactive = new ReactiveProperty<int>();
    // ReactivePropertyを公開して外部からアクセス可能にする
    public IReadOnlyReactiveProperty<int> ScoreReactive => scoreReactive;
    public IReadOnlyReactiveProperty<int> MoneyReactive => moneyReactive;
    private bool AllEnemiesDestroyed()
    {
        // 敵が存在するかどうかを確認し、すべての敵が消滅したら true を返す
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
    private PlayerUiManager playerUiManager;
    private PlayerUiPresenter playerUiPresenter;
    public bool playerDieFlag = false;
    public bool spawnWaveFlag = true;
    public List<GameObject> enemyPrefabs; // 敵のプレハブのリスト
    public float spawnDistance1 = 25f; // 敵のスポーンする距離
    public float spawnDistance2 = 80f; // 敵のスポーンする距離
    public float spawnAngle1 = 110f; // 扇形の角度
    public float spawnAngle2 = 90f; // 扇形の角度
    private bool isWaveClearShopOpen = false;//一度だけ実行するためのフラグ
    public int waveNumber = 1;
    [SerializeField]
    private List<AudioClip> EnemySpawnVoice;
    void Start()
    {
        playerDieFlag = false;
        playerUiPresenter = GameObject.Find("PlayerUiCanvas").GetComponent<PlayerUiPresenter>();
        playerUiManager = GameObject.Find("PlayerUiCanvas").GetComponent<PlayerUiManager>();
        _ = StartCoroutine(SpawnEnemiesPeriodically1());
    }
    void Update()
    {
        //ウェーブクリアの判定
        if (AllEnemiesDestroyed() && !playerDieFlag && !playerUiManager.shopFlag && !spawnWaveFlag)
        {
            Debug.Log("Waveクリア");
            StartCoroutine(WaveClearOpenShop());
            waveNumber = waveNumber + 1;//wave進行
            playerUiPresenter.LetsOnShopFlag();
        }
    }
    //倒した敵のスコアの和
    public void AddEnemyScore(int score)
    {
        deadEnemyScore += score;
        scoreReactive.Value = deadEnemyScore; // スコアの変化を通知
    }
    //敵を倒して獲得したお金の和
    // public void AddEnemyMoney(int money)
    // {
    //     deadEnemyMoney += money;
    //     moneyReactive.Value = deadEnemyMoney; // お金の変化を通知
    // }
    public void AddEnemyMoney()
    {
        deadEnemyMoney += 1;
        moneyReactive.Value = deadEnemyMoney; // お金の変化を通知
        playerUiManager.playerMoney += 1;//もうこれでもいいやん
    }
    //ウェーブクリアメッセージとショップ画面への移行
    //これは別にuiPresenterでもいいかも
    IEnumerator WaveClearOpenShop()
    {
        if (!isWaveClearShopOpen)
        {
            isWaveClearShopOpen = true;
            playerUiPresenter.LetsSetWaveClearText();
            yield return new WaitForSeconds(4f);
            playerUiPresenter.LetsOutWaveSetShopPanel();
            isWaveClearShopOpen = false;
        }
    }
    public void OnWaveStart()
    {
        Debug.Log("OnWaveStart実行");
        spawnWaveFlag = true;
        switch (waveNumber % 10)
        {
            case 1:
                StartCoroutine(SpawnEnemiesPeriodically1());
                break;
            case 2:
                StartCoroutine(SpawnEnemiesPeriodically2());
                break;
            case 3:
                StartCoroutine(SpawnEnemiesPeriodically3());
                break;
            case 4:
                StartCoroutine(SpawnEnemiesPeriodically4());
                break;
            case 6:
                StartCoroutine(SpawnEnemiesPeriodically1());
                break;
            case 7:
                StartCoroutine(SpawnEnemiesPeriodically2());
                break;
            case 8:
                StartCoroutine(SpawnEnemiesPeriodically3());
                break;
            case 9:
                StartCoroutine(SpawnEnemiesPeriodically4());
                break;
        }
        if (waveNumber % 5 == 0)
        {
            StartCoroutine(BossSpawnEnemiesPeriodically1());
        }
    }

    //ウェーブ１
    IEnumerator SpawnEnemiesPeriodically1()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Wave" + waveNumber + "開始");
        playerUiPresenter.LetsWaveStartText(waveNumber);
        yield return new WaitForSeconds(3f);
        playerUiPresenter.LetsOutWaveStartText();
        spawnWaveFlag = true;

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDieFlag && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = UnityEngine.Random.Range(2, 5f);
            yield return new WaitForSeconds(waitTime1);
            int voiceNumber = UnityEngine.Random.Range(0, 2);
            SoundManager.Instance.PlaySound(EnemySpawnVoice[voiceNumber]);
            SpawnEnemy1(0);//手前の湧き
            // SpawnEnemy2(0);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" + waveNumber + "終了");
        spawnWaveFlag = false;
    }

    //ウェーブ２
    IEnumerator SpawnEnemiesPeriodically2()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Wave" + waveNumber + "開始");
        playerUiPresenter.LetsWaveStartText(waveNumber);
        yield return new WaitForSeconds(3f);
        playerUiPresenter.LetsOutWaveStartText();
        spawnWaveFlag = true;

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDieFlag && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = UnityEngine.Random.Range(2f, 4f);
            yield return new WaitForSeconds(waitTime1);
            int voiceNumber = UnityEngine.Random.Range(0, 5);
            SoundManager.Instance.PlaySound(EnemySpawnVoice[voiceNumber]);
            SpawnEnemy1(0);//手前の湧き
            SpawnEnemy2(1);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" + waveNumber + "終了");
        spawnWaveFlag = false;
    }

    //ウェーブ３
    IEnumerator SpawnEnemiesPeriodically3()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Wave" + waveNumber + "開始");
        playerUiPresenter.LetsWaveStartText(waveNumber);
        yield return new WaitForSeconds(3f);
        playerUiPresenter.LetsOutWaveStartText();

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDieFlag && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = UnityEngine.Random.Range(3f, 5f);
            yield return new WaitForSeconds(waitTime1);
            int voiceNumber = UnityEngine.Random.Range(6, 11);
            SoundManager.Instance.PlaySound(EnemySpawnVoice[voiceNumber]);
            SpawnEnemy3(2);//手前の湧き
            SpawnEnemy2(3);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" + waveNumber + "終了");
        spawnWaveFlag = false;
    }
    //ウェーブ４
    IEnumerator SpawnEnemiesPeriodically4()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Wave" + waveNumber + "開始");
        playerUiPresenter.LetsWaveStartText(waveNumber);
        yield return new WaitForSeconds(3f);
        playerUiPresenter.LetsOutWaveStartText();

        // 時間計測
        float elapsedTime = 0f;

        //ウェーブ時間の設定
        while (!playerDieFlag && elapsedTime < 10f)
        {
            // 2から5秒のランダムな待ち時間を生成
            float waitTime1 = UnityEngine.Random.Range(4f, 5f);
            yield return new WaitForSeconds(waitTime1);
            int voiceNumber = UnityEngine.Random.Range(9, 11);
            SoundManager.Instance.PlaySound(EnemySpawnVoice[voiceNumber]);
            SpawnEnemy1(3);//手前の湧き
            SpawnEnemy2(3);//奥の湧き

            // 経過時間を加算
            elapsedTime += waitTime1;
        }
        Debug.Log("Wave" + waveNumber + "終了");
        spawnWaveFlag = false;
    }

    //デス確認で湧き停止
    public void PlayerDie()
    {
        playerDieFlag = true;
        playerUiPresenter.LetsChangePlayerDieFlag();
        if (spawnWaveFlag)
        {
            StopCoroutine(SpawnEnemiesPeriodically1()); //ウェーブ１停止
            StopCoroutine(SpawnEnemiesPeriodically2()); //ウェーブ２停止
            StopCoroutine(SpawnEnemiesPeriodically3()); //ウェーブ３停止
            StopCoroutine(SpawnEnemiesPeriodically4()); //ウェーブ4停止
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
            float randomAngle = UnityEngine.Random.Range(-spawnAngle1 / 2f, spawnAngle1 / 2f); // 扇形の角度内でランダムな角度を選択
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
            float randomAngle = UnityEngine.Random.Range(-spawnAngle2 / 2f, spawnAngle2 / 2f); // 扇形の角度内でランダムな角度を選択
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
    //手前の空中の敵のスポーン
    public void SpawnEnemy3(int enemyNumber)
    {
        if (enemyNumber >= 0 && enemyNumber < enemyPrefabs.Count)
        {
            // プレイヤーの位置を取得
            Vector3 playerPosition = transform.position;

            // 扇形の端の位置を計算
            Vector3 spawnDirection = Quaternion.Euler(0f, -spawnAngle1 / 2f, 0f) * transform.forward; // 扇形の左端の方向
            Vector3 spawnPosition = playerPosition + spawnDirection.normalized * spawnDistance1; // スポーン位置

            // ランダムな位置を選択
            float randomAngle = UnityEngine.Random.Range(-spawnAngle1 / 2f, spawnAngle1 / 2f); // 扇形の角度内でランダムな角度を選択
            Vector3 randomOffset = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnDistance1;

            // ランダムな高さを選択
            float randomHeight = UnityEngine.Random.Range(6f, 10f); // 高さの範囲を指定

            // プレイヤー位置からランダムな位置に湧く
            spawnPosition = playerPosition + randomOffset;
            spawnPosition.y += randomHeight; // 高さを設定

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
        switch (waveNumber)
        {
            case 5:
                yield return new WaitForSeconds(1f);
                Debug.Log("BossWave" + waveNumber + "開始");
                playerUiPresenter.LetsWaveStartText(waveNumber);
                yield return new WaitForSeconds(3f);
                playerUiPresenter.LetsOutWaveStartText();
                spawnWaveFlag = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("ボス出現");
                SoundManager.Instance.PlaySound(EnemySpawnVoice[12]);
                SpawnBossEnemy(4);
                Debug.Log("Wave" + waveNumber + "終了");
                spawnWaveFlag = false;
                break;
            case 10:
                yield return new WaitForSeconds(1f);
                Debug.Log("BossWave" + waveNumber + "開始");
                playerUiPresenter.LetsWaveStartText(waveNumber);
                yield return new WaitForSeconds(3f);
                playerUiPresenter.LetsOutWaveStartText();
                spawnWaveFlag = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("ボス出現");
                SoundManager.Instance.PlaySound(EnemySpawnVoice[13]);
                SpawnBossEnemy(5);
                Debug.Log("Wave" + waveNumber + "終了");
                spawnWaveFlag = false;
                break;
            case 15:
                yield return new WaitForSeconds(1f);
                Debug.Log("BossWave" + waveNumber + "開始");
                playerUiPresenter.LetsWaveStartText(waveNumber);
                yield return new WaitForSeconds(3f);
                playerUiPresenter.LetsOutWaveStartText();
                spawnWaveFlag = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("ボス出現");
                SoundManager.Instance.PlaySound(EnemySpawnVoice[14]);
                SpawnBossEnemy(6);
                Debug.Log("Wave" + waveNumber + "終了");
                spawnWaveFlag = false;
                break;
            case 20:
                yield return new WaitForSeconds(1f);
                Debug.Log("BossWave" + waveNumber + "開始");
                playerUiPresenter.LetsWaveStartText(waveNumber);
                yield return new WaitForSeconds(3f);
                playerUiPresenter.LetsOutWaveStartText();
                spawnWaveFlag = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("ボス出現");
                SoundManager.Instance.PlaySound(EnemySpawnVoice[15]);
                SpawnBossEnemy(7);
                Debug.Log("Wave" + waveNumber + "終了");
                spawnWaveFlag = false;
                break;
            case 25:
                yield return new WaitForSeconds(1f);
                Debug.Log("BossWave" + waveNumber + "開始");
                playerUiPresenter.LetsWaveStartText(waveNumber);
                yield return new WaitForSeconds(3f);
                playerUiPresenter.LetsOutWaveStartText();
                spawnWaveFlag = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("ボス出現");
                SoundManager.Instance.PlaySound(EnemySpawnVoice[16]);
                SpawnBossEnemy(8);
                Debug.Log("Wave" + waveNumber + "終了");
                spawnWaveFlag = false;
                break;
            case > 25:
                yield return new WaitForSeconds(1f);
                Debug.Log("BossWave" + waveNumber + "開始");
                playerUiPresenter.LetsWaveStartText(waveNumber);
                yield return new WaitForSeconds(3f);
                playerUiPresenter.LetsOutWaveStartText();
                spawnWaveFlag = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("ボス出現");
                SoundManager.Instance.PlaySound(EnemySpawnVoice[17]);
                SpawnBossEnemy(UnityEngine.Random.Range(4, 8));
                Debug.Log("Wave" + waveNumber + "終了");
                spawnWaveFlag = false;
                break;
        }
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
