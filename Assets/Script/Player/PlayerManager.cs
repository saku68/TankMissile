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
    public Camera mainCamera; // メインカメラの参照
    private Rigidbody rb;

    public int antiDamage = 0;
    public float moveSpeed = 9f;
    public IReadOnlyReactiveProperty<int> Hp => hp;
    [SerializeField]
    private IntReactiveProperty hp = new IntReactiveProperty(100);

    public IReadOnlyReactiveProperty<int> MaxHp => maxHp;
    [SerializeField]
    private IntReactiveProperty maxHp = new IntReactiveProperty(100);

    void Start()
    {
        hp.Value = maxHp.Value;
        playerPresenter = GetComponent<PlayerPresenter>();
        playerUiPresenter = GameObject.Find("PlayerUiCanvas").GetComponent<PlayerUiPresenter>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        //参照先を減らすのとUniRxの練習のため
        //発射の処理
        _ = this.UpdateAsObservable()
        .Where(_ => Input.GetKey(KeyCode.Return))
        .Subscribe(_ => playerPresenter.LetsShoot());
        //砲台とプレイヤーの操作
        _ = this.UpdateAsObservable()
        .Subscribe(_ =>
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            playerPresenter.LetsUpdateAngles(horizontal, vertical);

            // 左右回転の操作
            float rotationInput = 0f;
            //このifとても美しくない
            if (Input.GetKey(KeyCode.E)) rotationInput += 1f;
            if (Input.GetKey(KeyCode.Q)) rotationInput -= 1f;
            if (Input.GetKey(KeyCode.A)) rotationInput += 1f;
            if (Input.GetKey(KeyCode.D)) rotationInput -= 1f;
            playerPresenter.LetsRotate(rotationInput);

            // 戦車のように前進/後退
            if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q))
            {
                // 前進
                rb.MovePosition(transform.position + transform.forward * Time.deltaTime * moveSpeed);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            {
                // 後退
                rb.MovePosition(transform.position - transform.forward * Time.deltaTime * moveSpeed);
            }
        });

        enemySpawn = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawn>();

        // hpがmaxHpを超えないようにする
        _ = hp.Subscribe(newHp =>
        {
            if (newHp > maxHp.Value)
            {
                hp.Value = maxHp.Value;
            }
        });
    }

    void Update()
    {
        // 確認用の加速
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Time.timeScale = Time.timeScale == 1 ? 8 : 1; // Backspace キーで加速/元に戻す
        }

        // エスケープキーの操作
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerUiPresenter.OnEsckey();
        }
    }

    // ダメージの処理
    private void Damage(int damage)
    {
        if (antiDamage > 0)
        {
            damage = damage - antiDamage;
            if (damage < antiDamage)
            {
                damage = 1;
            }
        }
        hp.Value -= damage;
        if (hp.Value <= 0)
        {
            hp.Value = 0;
            OnPlayerDeath();
        }
        Debug.Log("残りHP:" + hp);
    }
    private void OnPlayerDeath()
    {
        // カメラを破壊しないようにする
        mainCamera.transform.SetParent(null);

        Destroy(this.gameObject);
        hp.Dispose();
        maxHp.Dispose();
        Debug.Log("死んだ！");
        playerUiPresenter.LetsSetDeadText();
        enemySpawn.PlayerDie();
        playerPresenter.LetsOffDrawArc();
    }

    //MaxHpの増加
    public void UpMaxHp(int UpMaxHp)
    {
        maxHp.Value += UpMaxHp;
    }

    //Hpの回復
    public void UpHp(int UpHp)
    {
        hp.Value += UpHp;
    }

    public void UpHpMax()
    {
        hp.Value = maxHp.Value;
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