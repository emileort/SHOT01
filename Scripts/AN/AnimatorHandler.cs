using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    
    public Animator anim;
    Player player;


    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        // this.UpdateAnumStatus();
        // this.Atk();
    }

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    // private void UpdateAnumStatus()
    // {
    //     anim.SetFloat("animTime", Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
    // 
    //     anim.ResetTrigger("atk");
    // }

    // private void Atk()
    // {
    //     if (inputHander.rb_Input)
    //     {
    //         anim.SetTrigger("atk");
    //     }
    // }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }

        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }

        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }

        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }

        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal

        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }

        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }

        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }

        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }

        else
        {
            h = 0;
        }
        #endregion

        if (isSprinting && verticalMovement > 0)
        {
            v = 2;
            h = horizontalMovement;
        }

    }

    public void PlayTargetAnimation(string targetAnim, bool idel)
    {
        anim.applyRootMotion = idel;
        anim.SetBool("idel", idel);
        anim.CrossFade(targetAnim, 0.2f);
    }

}
