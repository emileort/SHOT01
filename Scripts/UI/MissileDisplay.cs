using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text amountText;

    static Image cooldownImage;

    [SerializeField] GameObject Fill;

    [SerializeField] GameObject Fillpointer;

    Animator animator;

    public AnimatorHandler animatorHandler;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        amountText = transform.Find("Amount Text"). GetComponent<Text>();
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
    }

    private void Update()
    {
        Filled();
    }

    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();

    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;

    public void Filled()
    {
        Animator animator = Fillpointer.GetComponent<Animator>();

        if (cooldownImage.fillAmount > 0.0004)
        {
            Fill.SetActive(false);
            animator.Play("大招指針續氣");
        }
        else
        {
            Fill.SetActive(true);
            animator.Play("大招指針待機");
        }
    }
}
