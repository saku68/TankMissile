using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoinManager : MonoBehaviour
{
    public float followDistance = 5f; // プレイヤーを追跡し始める距離

    private Transform playerTransform;
    private Rigidbody rb;
    [SerializeField]
    private float launchForce = 10f; // 打ち出す力
    [SerializeField]
    private float upwardForce = 6f; // 上方向の力

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // プレイヤーの方向を向く
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Raycastでプレイヤーとの障害物の有無を確認
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, followDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // プレイヤーに向かって移動
                // rb.velocity = direction * 5f; // 移動速度は適宜調整してください
                // 上方向の力を加えて打ち出す
                rb.velocity = direction * launchForce + Vector3.up * upwardForce;
            }
        }
    }
}
