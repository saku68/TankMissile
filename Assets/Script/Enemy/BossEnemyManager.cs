using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossEnemyManager : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Animator animator;
    private EnemyUiManager enemyUiManager;
    private EnemySpawn enemySpawn;
    public Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject goldCoinPrefab;
    [SerializeField]
    private List<AudioClip> EnemyDamageVoice;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
        enemyUiManager = GameObject.Find("EnemyUiCanvas").GetComponent<EnemyUiManager>();
        enemyUiManager.UpdateHp(enemyStats.enemyHp);
        enemyUiManager.UpdateMaxHp(enemyStats.enemyHp);

        // プレイヤーを検索してターゲットとして設定
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            UnityEngine.Debug.LogWarning("Playerオブジェクトが見つかりませんでした。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーへの移動
        agent.destination = target.position;
        //Hpバーの更新
        enemyUiManager.UpdateHp(enemyStats.enemyHp);
    }
    // ダメージの処理
    void Damage(int damage)
    {
        SoundManager.Instance.PlaySound(EnemyDamageVoice[0]);
        enemyStats.enemyHp -= damage;
        animator.SetTrigger("Damage");
        animator.SetInteger("DamageAmount", damage);
        if (enemyStats.enemyHp <= 0)
        {
            SoundManager.Instance.PlaySound(EnemyDamageVoice[1]);
            enemyStats.enemyHp = 0;
            // enemySpawn.AddEnemyMoney(enemyMoney);
            enemySpawn.AddEnemyScore(enemyStats.enemyScore);
            animator.SetTrigger("Death");
        }
    }
    public void OnEnemyDeath()
    {
        SpawnGoldCoins(enemyStats.enemyMoney);
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
                Vector3 force = new Vector3(Random.Range(-2f, 2f), Random.Range(8f, 14f), Random.Range(-2, 2f));
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
