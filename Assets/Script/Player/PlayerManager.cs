using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerManager : MonoBehaviour
{
    private PlayerPresenter playerPresenter;
    private EnemySpawn enemySpawn;
    public PlayerUiManager playerUiManager;

    [SerializeField]
    private int maxHp = 10;
    [SerializeField]
    private int hp = 10;
    // Start is called before the first frame update
    void Start()
    {
        playerPresenter = GetComponent<PlayerPresenter>();
        //発射の処理
        _ = this.UpdateAsObservable()
        .Where(_ => Input.GetKey(KeyCode.Return))
        .Subscribe(_ => playerPresenter.LetsShoot());
        //砲台の操作
        _ = this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                playerPresenter.LetsUpdateAngles(horizontal, vertical);
            });


        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();
        hp = maxHp;
        playerUiManager.UpdateMaxHp(maxHp);
    }

    // ダメージの処理
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Destroy(this.gameObject);
            Debug.Log("死んだ！");
            playerUiManager.SetDeadText();
            enemySpawn.PlayerDie();
        }
        playerUiManager.UpdateHP(hp);
        Debug.Log("残りHP:" + hp);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 衝突相手が "Enemy" タグを持っているかチェックする
        if (other.CompareTag("Enemy"))
        {
            // 衝突相手が "Enemy" タグを持っている場合のみダメージを与える
            Dameger damager = other.GetComponent<Dameger>();
            if (damager != null)
            {
                Damage(damager.damage1);
                // 対象を破壊する
                Destroy(other.gameObject);
            }
        }
    }
}
