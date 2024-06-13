using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip BuyAbilitySound1;
    [SerializeField]
    private AudioClip BuyAbilitySound2;
    PlayerPresenter playerPresenter;
    PlayerUiPresenter playerUiPresenter;
    PlayerUiManager playerUiManager;
    public float newFireRate = 1;
    public int fireRateMuch = 6;
    public int fireRateLevel = 1;
    public int upMaxHpMuch = 8;
    public int upMaxHpAmount = 20;
    public int upMaxHpLevel = 1;
    public int healHpAmount = 100;
    public int healHpMuch = 10;
    public int upBulletRangeAmount = 2;
    public int upBulletRangeMuch = 6;
    public int bulletRangeLevel = 1;
    public int upBulletSizeMuch = 5;
    public int upBulletSizeLevel = 1;
    public Vector3 upBulletSize;
    public int upBulletDamage = 1;
    public int upBulletDamageLevel = 1;
    public int upBulletDamageMuch = 9;
    public int antiDamageMuch = 6;
    public int antiDamageLevel = 1;
    public int upAntiDamage;
    public int upMoveSpeed;
    public int moveSpeedLevel = 1;
    public int moveSpeedMuch = 4;
    public int upShootBulletModeLevel = 1;
    public int upShootBulletModeMuch = 100;
    public int shootModeNumber = 2;
    void Start()
    {
        playerPresenter = GameObject.Find("Player").GetComponent<PlayerPresenter>();
        playerUiPresenter = GetComponent<PlayerUiPresenter>();
        playerUiManager = GetComponent<PlayerUiManager>();
        upBulletSize = new Vector3(0.9f, 0.9f, 0.9f);
        upAntiDamage = 3;
        upMoveSpeed = 6;
        shootModeNumber = 2;
    }
    //発射レート購入ボタン
    public void UpFireRateButton()
    {
        if (playerUiManager.playerMoney >= fireRateMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力上昇の処理
            playerPresenter.UpFirerate(newFireRate);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(fireRateMuch);
            //更新
            fireRateMuch += 5;
            newFireRate += 0.5f;
            fireRateLevel += 1;
            //価格変更の処理
            playerUiPresenter.LetsChangeFireRateMuch(fireRateMuch, fireRateLevel);
        }
    }
    //最大Hpの増加ボタン
    public void UpMaxHpButton()
    {
        if (playerUiManager.playerMoney >= upMaxHpMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.LetsUpMaxHp(upMaxHpAmount);
            playerPresenter.LetsUpHpMax();
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(upMaxHpMuch);
            //更新
            upMaxHpMuch += 4;
            upMaxHpAmount += 20;
            upMaxHpLevel += 1;
            //価格の変更
            playerUiPresenter.LetsChangeUpMaxHpMuch(upMaxHpMuch, upMaxHpLevel);
        }
    }
    //回復ボタン
    public void HealHpButton()
    {
        if (playerUiManager.playerMoney >= healHpMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            playerPresenter.LetsUpHp(healHpAmount);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(healHpMuch);
        }
    }
    //射程増加ボタン
    public void UpBulletRangeButton()
    {
        if (playerUiManager.playerMoney >= upBulletRangeMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.UpBulletRange(upBulletRangeAmount);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(upBulletRangeMuch);
            //更新
            upBulletRangeMuch += 4;
            bulletRangeLevel += 1;
            //価格変更の処理
            playerUiPresenter.LetsChangeBulletRangeMuch(upBulletRangeMuch, bulletRangeLevel);
        }
    }
    //弾のサイズ増加ボタン
    public void UpBulletSizeButton()
    {
        if (playerUiManager.playerMoney >= upBulletSizeMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.LetsChangeBulletSize(upBulletSize);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(upBulletSizeMuch);
            //更新
            upBulletSizeMuch += 5;
            upBulletSizeLevel += 1;
            upBulletSize += new Vector3(0.4f, 0.4f, 0.4f);
            //価格変更の処理
            playerUiPresenter.LetsChangeBulletSizeMuch(upBulletSizeMuch, upBulletSizeLevel);
        }
    }
    //威力増加ボタン
    public void UpBulletDamageButton()
    {
        if (playerUiManager.playerMoney >= upBulletDamageMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.LetsChangeBulletDamage(upBulletDamage);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(upBulletDamageMuch);
            //更新
            upBulletDamageMuch += 5;
            upBulletDamageLevel += 1;
            //価格変更の処理
            playerUiPresenter.LetsChangeBulletDamageMuch(upBulletDamageMuch, upBulletDamageLevel);
        }
    }
    //防御力増加ボタン
    public void UpAntiDamageButton()
    {
        if (playerUiManager.playerMoney >= antiDamageMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.UpAntiDamage(upAntiDamage);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(antiDamageMuch);
            //更新
            antiDamageMuch += 8;
            antiDamageLevel += 1;
            //価格変更の処理
            playerUiPresenter.LetsChangeAntiDamageMuch(antiDamageMuch, antiDamageLevel);
        }
    }
    //移動速度増加ボタン
    public void UpMoveSpeedButton()
    {
        if (playerUiManager.playerMoney >= moveSpeedMuch)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.LetsUpMoveSpeed(upMoveSpeed);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(moveSpeedMuch);
            //更新
            moveSpeedMuch += 3;
            moveSpeedLevel += 1;
            //価格変更の処理
            playerUiPresenter.LetsChangeMoveSpeedMuch(moveSpeedMuch, moveSpeedLevel);
        }
    }
    //発射弾数増加ボタン
    public void UpShootBulletModeButton()
    {
        if (playerUiManager.playerMoney >= upShootBulletModeMuch && shootModeNumber <= 3)
        {
            SoundManager.Instance.PlaySound(BuyAbilitySound1);
            SoundManager.Instance.PlaySound(BuyAbilitySound2);
            //能力の上昇
            playerPresenter.LetsChangeShootBulletMode(shootModeNumber);
            //お金の減少の処理
            playerUiPresenter.LetsBuyAnyAbility(upShootBulletModeMuch);
            //更新
            upShootBulletModeMuch += 100;
            shootModeNumber += 1;
            upShootBulletModeLevel += 1;
            //価格変更の処理
            playerUiPresenter.LetsUpShootBulletModeText(upShootBulletModeMuch, upShootBulletModeLevel);
        }
    }
}
