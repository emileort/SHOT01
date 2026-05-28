using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;

    

    float defaultfixedDeltaTime;

    float timeScaleBeforePause;

    float t;

    protected override void Awake()
    {
        base.Awake();
        defaultfixedDeltaTime = Time.fixedDeltaTime;
    }


    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
        
    }

    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
        
    }

    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration, float outDuration)
    {

        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {

        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));
    }

    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }


    IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        yield return new WaitForSecondsRealtime(keepingDuration);

        StartCoroutine(SlowInCoroutine(outDuration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowInCoroutine(outDuration));
    }


    IEnumerator SlowInCoroutine(float duration)
    {
        float startTime = Time.unscaledTime;

        t = 0f;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultfixedDeltaTime * Time.timeScale;
            }
           

            yield return null;
        }

        Debug.Log("Slow out Duration:" + (Time.unscaledTime - startTime));
    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        float startTime = Time.unscaledTime;

        t = 0f;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultfixedDeltaTime * Time.timeScale;
            }
            

            yield return null;
        }

        Debug.Log("Slow out Duration:" + (Time.unscaledTime - startTime));
    }
}