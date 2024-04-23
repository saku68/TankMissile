using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private EnemySpawn enemySpawn;

    [SerializeField]
    private int enemyMoney;
    [SerializeField]
    private int hp = 1;
    public Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemySpawn= GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
    // ダメージの処理
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            enemySpawn.AddMoney(enemyMoney);
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
                Damage(damager.damage1);
            }
        }
    }
}
