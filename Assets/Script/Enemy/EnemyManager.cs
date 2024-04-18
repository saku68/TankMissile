using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private int Hp = 1;
    public Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
    // ダメージの処理
    void damage(int Damage)
    {
        Hp -= Damage;
        if(Hp <= 0)
        {
            Hp = 0;
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
            damage(damager.Damage1);
        }
        }
    }
}
