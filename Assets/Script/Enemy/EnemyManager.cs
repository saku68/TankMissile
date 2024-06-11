using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private Animator animator;
    private EnemySpawn enemySpawn;
    [SerializeField]
    private int enemyScore;

    [SerializeField]
    private int enemyMoney;
    [SerializeField]
    private int hp = 1;
    public Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject goldCoinPrefab;
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();

        // プレイヤーを検索してターゲットとして設定
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Playerオブジェクトが見つかりませんでした。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.destination = target.position;
        }
        else
        {
            // プレイヤーが存在しない場合の処理（例: 追跡を停止する）
            agent.destination = transform.position;
        }
    }
    // ダメージの処理
    void Damage(int damage)
    {
        hp -= damage;
        animator.SetTrigger("Damage");
        animator.SetInteger("DamageAmount", damage);
        if (hp <= 0)
        {
            hp = 0;
            enemySpawn.AddEnemyMoney(enemyMoney);
            enemySpawn.AddEnemyScore(enemyScore);
            animator.SetTrigger("Death");
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
