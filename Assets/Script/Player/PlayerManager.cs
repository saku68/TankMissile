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
    private PlayerUiPresenter playerUiPresenter;

    private EnemySpawn enemySpawn;

    // [SerializeField]
    // private int maxHp = 10;
    // [SerializeField]
    // private int hp = 10;

    public IReadOnlyReactiveProperty<int> Hp => hp;
    [SerializeField]
    private IntReactiveProperty hp = new IntReactiveProperty(10);

    public IReadOnlyReactiveProperty<int> MaxHp => maxHp;
    [SerializeField]
    private IntReactiveProperty maxHp = new IntReactiveProperty(10);

    void Start()
    {
        hp.Value = maxHp.Value;
        playerPresenter = GetComponent<PlayerPresenter>();
        playerUiPresenter = GameObject.Find("PlayerUiCanvas").GetComponent<PlayerUiPresenter>();

        //参照先を減らすのとUniRxの練習のため
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
    }
    void Update()
    {
        //確認用の加速
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Time.timeScale = 8;
        }
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            Time.timeScale = 1;
        }
        
        // エスケープキーの操作
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerUiPresenter.OnEsckey();
        }
    }

    // ダメージの処理
    void Damage(int damage)
    {
        hp.Value -= damage;
        if (hp.Value <= 0)
        {
            hp.Value = 0;
            Destroy(this.gameObject);
            hp.Dispose();
            maxHp.Dispose();
            Debug.Log("死んだ！");
            playerUiPresenter.LetsSetDeadText();
            enemySpawn.PlayerDie();
        }
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
