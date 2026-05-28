using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;

    [SerializeField] Image fillImageFront;

    [SerializeField] bool delayFill = true;

    [SerializeField] float fillDelay = 0.5f;

    [SerializeField] float fillSpeed = 0.1f;

    float currentFillAmount;

    protected float targetFillAmount;

    float t;


    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingcoroutine;

    void Awake()
    {

        if(TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        

        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue,float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue,float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (bufferedFillingcoroutine != null)
        {
            StopCoroutine(bufferedFillingcoroutine);
        }

        //當狀態值減少時
        if (currentFillAmount > targetFillAmount)
        {
            //前面圖片的填充值=目標填充值
            fillImageFront.fillAmount = targetFillAmount;
            //慢慢減少後面圖片填充值
            bufferedFillingcoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            // return;
        }

        //當狀態增加時
        else if (currentFillAmount < targetFillAmount)
        {
            //後面圖片的填充值=目標填充值
            fillImageBack.fillAmount = targetFillAmount;
            //慢慢增加前面圖片填充值
            bufferedFillingcoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }
        
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;

            yield return null;

        }

        
    }
}
