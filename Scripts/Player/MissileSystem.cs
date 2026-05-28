using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;

    [SerializeField] float cooldownTime = 20f;

    [SerializeField] GameObject missilePrefab = null;

    [SerializeField] AudioData lanuchSFX = null;

    bool isReady = true;

    int amount;

    void Awake()
    {
        amount = defaultAmount;
    }

    void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }

    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }

    public void Launch(Transform muzzleTransfrom)
    {
        if (amount == 0 || !isReady) return; //可以加音效提示

        amount++;

        isReady = false;
        
        // 池裡取出導彈
        PoolManager.Release(missilePrefab, muzzleTransfrom.position);
        // 發射導彈的音效
        AudioManager.Instance.PlayRandomSFX(lanuchSFX);

        amount--;

        MissileDisplay.UpdateAmountText(amount);

        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            //進冷卻
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {
        // yield return new WaitForSeconds(cooldownTime);

        var cooldownValue = cooldownTime;

        while (cooldownValue > 0f)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);

            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }

        isReady = true;
    }



}
