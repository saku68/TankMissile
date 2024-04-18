using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private int MaxHp = 10;
    [SerializeField]
    private int Hp = 10;
    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("残りHP:"+Hp);
    }
    private void OnTriggerEnter(Collider other)
    {
        Dameger damager = other.GetComponent<Dameger>();
        if (damager != null)
        {
            damage(damager.Damage1);
        }
    }
}
