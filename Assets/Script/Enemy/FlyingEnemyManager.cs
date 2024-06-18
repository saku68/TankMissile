using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyManager : MonoBehaviour
{
    public float speed = 3f; // 敵の移動速度
    public float rotationSpeed = 8f; // 敵の回転速度
    [SerializeField]
    private Transform playerTransform;
    private EnemySpawn enemySpawn;
    [SerializeField]
    private int enemyScore;

    [SerializeField]
    private int enemyMoney;
    [SerializeField]
    private int hp = 2;
    [SerializeField]
    private GameObject goldCoinPrefab;
    [SerializeField]
    private List<AudioClip> EnemyDamageVoice; 

    void Start()
    {
        // プレイヤーのTransformを取得
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
    }

    void Update()
    {
        // プレイヤーの方向を計算
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // 敵の現在の方向を取得
        Vector3 currentDirection = transform.forward;

        // 回転をスムーズにする
        Vector3 newDirection = Vector3.RotateTowards(currentDirection, direction, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        // プレイヤーの方向に移動
        transform.position += newDirection * speed * Time.deltaTime;
    }
    // ダメージの処理
    void Damage(int damage)
    {
        hp -= damage;
        SoundManager.Instance.PlaySound(EnemyDamageVoice[0]);
        if (hp <= 0)
        {
            SoundManager.Instance.PlaySound(EnemyDamageVoice[1]);
            hp = 0;
            // enemySpawn.AddEnemyMoney(enemyMoney);
            enemySpawn.AddEnemyScore(enemyScore);
            OnEnemyDeath();
        }
    }
    public void OnEnemyDeath()
    {
        SpawnGoldCoins(enemyMoney);
        Destroy(this.gameObject);
    }
    private void SpawnGoldCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * 2f;
            randomPosition.y = transform.position.y; // 高さを合わせる

            GameObject coin = Instantiate(goldCoinPrefab, randomPosition, Quaternion.identity);

            // Rigidbodyを取得して力を加える
            Rigidbody rb = coin.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 force = new Vector3(Random.Range(-2f, 2f), Random.Range(4f, 8f), Random.Range(-2, 2f));
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 衝突相手が "Bullet" タグを持っているかチェックする
        if (other.CompareTag("Bullet"))
        {
            // 衝突相手が "Bullet" タグを持っている場合のみダメージを与える
            Dameger damager = other.GetComponent<Dameger>();
            if (damager != null)
            {
                Damage(damager.damage2);
                Destroy(other.gameObject);
            }
        }
    }
}
