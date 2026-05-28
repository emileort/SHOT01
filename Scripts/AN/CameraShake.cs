using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool isShake = false;

    public float shakeLv = 3;
    public float setShakeTime = 0.2f;
    public float shakeFps = 45;

    Camera selfCamera;
    Rect changeRect;

    float fps;
    float shakeTime = 0;
    float frameTime = 0f;
    float shakeDelta = 0.005f;

    void Start()
    {
        selfCamera = GetComponent<Camera>();
        changeRect = new Rect(0, 0, 1, 1);
        shakeTime = setShakeTime;
        fps = shakeFps;
        frameTime = 0.03f;
        shakeDelta = 0.005f;
    }

    private void Update()
    {
        if (isShake)
        {
            if (shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;

                if (shakeTime <= 0)
                {
                    changeRect.xMin = 0;
                    changeRect.yMin = 0;
                    selfCamera.rect = changeRect;
                    isShake = false;
                    shakeTime = setShakeTime;
                    fps = shakeFps;
                    frameTime = 0.03f;
                    shakeDelta = 0.005f;
                }
                else
                {
                    frameTime += Time.deltaTime;
                    if (frameTime > 1.0 / shakeFps)
                    {
                        frameTime = 0;
                        changeRect.xMin = shakeDelta * (-1 + shakeLv * Random.value);
                        changeRect.yMin = shakeDelta * (-1 + shakeLv * Random.value);
                        selfCamera.rect = changeRect;
                    }
                }
            }
        }
    }
}
