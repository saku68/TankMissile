using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossEnemyManager : MonoBehaviour
{
    private Animator animator;
    private EnemyUiManager enemyUiManager;
    private EnemySpawn enemySpawn;
    [SerializeField]
    private int enemyScore;

    [SerializeField]
    private int enemyMoney;
    [SerializeField]
    private int hp = 20;
    public Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject goldCoinPrefab;
    [SerializeField]
    private List<AudioClip> EnemyDamageVoice; 
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
        enemyUiManager = GameObject.Find("EnemyUiCanvas").GetComponent<EnemyUiManager>();
        enemyUiManager.UpdateHp(hp);
        enemyUiManager.UpdateMaxHp(hp);

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
        //プレイヤーへの移動
        agent.destination = target.position;
        //Hpバーの更新
        enemyUiManager.UpdateHp(hp);
    }
    // ダメージの処理
    void Damage(int damage)
    {
        SoundManager.Instance.PlaySound(EnemyDamageVoice[0]);
        hp -= damage;
        animator.SetTrigger("Damage");
        animator.SetInteger("DamageAmount", damage);
        if (hp <= 0)
        {
            SoundManager.Instance.PlaySound(EnemyDamageVoice[1]);
            hp = 0;
            // enemySpawn.AddEnemyMoney(enemyMoney);
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
