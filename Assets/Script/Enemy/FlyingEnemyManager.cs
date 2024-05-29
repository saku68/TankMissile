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
        if (hp <= 0)
        {
            hp = 0;
            enemySpawn.AddEnemyMoney(enemyMoney);
            enemySpawn.AddEnemyScore(enemyScore);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 衝突相手が "Player" タグを持っているかチェックする
        if (other.CompareTag("Player"))
        {
            // 衝突相手が "Player" タグを持っている場合のみダメージを与える
            Dameger damager = other.GetComponent<Dameger>();
            if (damager != null)
            {
                Damage(damager.damage2);
            }
        }
    }
}
