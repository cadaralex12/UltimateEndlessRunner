using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DG.Tweening;
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
            if (player.swipedUp && player.onRamp == false)
            {
                player.slideCounter = 0;
                player.y = player.jumpPower;
                player.inJump = true;
                player.m_Animator.Play("Jump");
            }
            else if(player.swipedUp && player.onRamp == true)
            {
                //m_Animator.enabled = false;
                player.y = 1.5f * player.jumpPower;
                player.inJump = true;

                int ran = UnityEngine.Random.Range(0, 10);
                Vector3 rot;
                if (ran % 2 == 0)
                {
                    rot = new Vector3(0, 360, 0);
                }
                else
                {
                    rot = new Vector3(0, -360, 0);
                }

                ran = UnityEngine.Random.Range(0, 10);
                if (ran % 3 == 0)
                {

                    //m_Animator.enabled = true;
                    UnityEngine.Debug.Log("Pula 1");
                    player.m_Animator.Play("Pose1");
                }
                else if (ran % 3 == 1)
                {
                    //m_Animator.enabled = true;
                    UnityEngine.Debug.Log("Pula 2");
                    player.m_Animator.Play("Pose2");
                }
                else
                {
                    //m_Animator.enabled = true;
                    UnityEngine.Debug.Log("Pula 3");
                    player.m_Animator.Play("Pose3");
                }

                transform.DORotate(rot, 1.5f, RotateMode.LocalAxisAdd).SetLoops(1).SetEase(Ease.Linear);
            }
            else
            {
                player.y = -20f;
            }
        }
        else
        {
            player.onRamp = false;
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
