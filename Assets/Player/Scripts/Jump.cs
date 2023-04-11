using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            //player.transform.Rotate(new Vector3(0,0,0));
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            player.inJump = false;
            player.doubleJump = false;
            if (player.inAirSlide || player.inSlide)
            {
                player.m_Animator.Play("Roll");
            }
            else if (player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Backflip") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("PoseOne") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Pose1") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Pose2") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Pose3") ||
                player.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Floating"))
            {
                player.m_Animator.Play("Landing");
                //inJump = false;
            }
            if (player.swipedUp)
            {
                player.slideCounter = 0;
                player.y = player.jumpPower;
                player.inJump = true;
                player.m_Animator.Play("Jump");
            }
            /*else
            {
                player.y = -0.5f;
            }*/
        }
        else
        {
            if (player.swipedUp && player.doubleJump == false)
            {
                player.doubleJump = true;
                player.y = player.jumpPower;
                player.inJump = true;
                player.m_Animator.Play("Backflip");
            }
            else
            {
                player.y -= player.jumpPower * 2 * Time.deltaTime;
            }

        }
    }
}
