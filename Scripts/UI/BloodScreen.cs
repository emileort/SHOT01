using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScreen : MonoBehaviour
{
    
    public static BloodScreen _instance;
    public static BloodScreen Instance
    {
        get { return _instance; }
    }

    private UISprite uiSprite;
    private TweenAlpha tweenAlpha;

    void Awake()
    {
        _instance = this;
        uiSprite = this.GetComponent<UISprite>();
        tweenAlpha = this.GetComponent<TweenAlpha>();
        uiSprite.enabled = false;
    }

    public void Show()
    {
        uiSprite.enabled = true;

        tweenAlpha.ResetToBeginning();
        tweenAlpha.PlayForward();
    }


}
