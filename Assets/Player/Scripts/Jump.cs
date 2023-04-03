using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Movement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JumpAction();
    }
    public void JumpAction()
    {
        if (player.m_char.isGrounded)
        {
            player.inJump = false;
            player.doubleJump = false;
            if (player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Backflip") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("PoseOne") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Pose1"))
            {
                player.m_Animator.Play("Landing");
                //inJump = false;
            }
            if (player.swipedUp)
            {
                player.y = player.jumpPower;
                player.inJump = true;
                player.m_Animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
        }
        else
        {
            if (player.swipedUp && player.doubleJump == false)
            {
                player.doubleJump = true;
                player.y = player.jumpPower;
                player.inJump = true;
                player.m_Animator.CrossFadeInFixedTime("Backflip", 0.1f);
            }
            else
            {
                if (player.momentum == true)
                {
                    player.y = player.jumpPower / 2;
                    player.momentum = false;
                }
                else
                {
                    player.y -= player.jumpPower * 2 * Time.deltaTime;
                }
            }
        }
    }
}
