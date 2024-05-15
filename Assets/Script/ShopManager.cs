using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    PlayerPresenter playerPresenter;
    PlayerUiPresenter playerUiPresenter;
    PlayerUiManager playerUiManager;
    private float newRate = 1;
    public int fireRateMuch = 5;
    void Start()
    {
        playerPresenter = GameObject.Find("Player").GetComponent<PlayerPresenter>();
        playerUiPresenter = GetComponent<PlayerUiPresenter>();
        playerUiManager = GetComponent<PlayerUiManager>();
    }
    //発射レート購入ボタン
    public void UpFireRateButton()
    {
        if (playerUiManager.playerMoney >= fireRateMuch)
        {
            //能力上昇の処理
            playerPresenter.UpFirerate(newRate);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(fireRateMuch);
            fireRateMuch += 5;
            newRate += 0.5f;
            //価格変更の処理
            playerUiPresenter.LetsChangeFireRateMuch(fireRateMuch);
        }
    }
}
