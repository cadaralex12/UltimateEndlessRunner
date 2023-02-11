using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE { left, mid, right};

public class Movement : MonoBehaviour
{
    public SIDE m_side = SIDE.mid;
    float newXPos = 0f;
    float xVal = 10f;
    private float x;
    private float speedDodge = 10f;
    private float jumpPower = 15f;
    private float y;
    public bool inJump;
    public bool inRoll;

    public bool swipeLeft, swipeRight, swipeUp;

    private CharacterController m_char;
    private Animator m_animator;
    
    // Start is called before the first frame update
    void Start()
    {
        m_char = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.A);
        swipeRight = Input.GetKeyDown(KeyCode.D);
        swipeUp = Input.GetKeyDown(KeyCode.W);

        if (swipeLeft)
        {
            if (m_side == SIDE.mid)
            {
                m_side = SIDE.left;
                newXPos = -xVal;
            }
            else if (m_side == SIDE.right)
            {
                m_side = SIDE.mid;
                newXPos = 0f;
            }
            m_animator.Play("Jog Strafe Left");
        }

        else if (swipeRight)
        {
            if (m_side == SIDE.mid)
            {
                m_side = SIDE.right;
                newXPos = xVal;
            }
            else if (m_side == SIDE.left)
            {
                m_side = SIDE.mid;
                newXPos = 0f;
            }
            m_animator.Play("Jog Strafe Left (1)");
        }
        Vector3 moveVector = new Vector3(x - transform.position.x, y*Time.deltaTime, 0);

        x = Mathf.Lerp(x, newXPos, Time.deltaTime * speedDodge);
        m_char.Move(moveVector);
        Jump();
    }

    public void Jump()
    {
        //To Do: Refine Jump Fall Land animations.
        if (m_char.isGrounded)
        {
            if (swipeUp)
            {
                m_animator.SetTrigger("isGrounded");
                y = jumpPower;
                m_animator.Play("Jump (1)");
                inJump = true;
            }
            else
            {
                inJump = false;
            }
        }
        else
        {
            y -= jumpPower * 2f * Time.deltaTime;
            //m_animator.Play("Falling");
            if (m_char.isGrounded)
            {
                m_animator.SetTrigger("isGrounded");
            }
        }
    }
}
