using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class MovementTutorial : MonoBehaviour
{
    public SIDE m_SIDE = SIDE.MID;
    public SIDE lastSide;
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private CharacterController m_char;
    private Collider col;
    private float x;
    public float speedDodge = 10f;
    public float jumpPower = 100f;
    private float y;
    public bool inJump;
    public bool inSlide;
    private Animator m_Animator;
    public float fwdSpeed = 150f;

    private float colHeight;
    private float colCenterY;

    public HitX hitX = HitX.None;
    public HitY hitY = HitY.None;
    public HitZ hitZ = HitZ.None;

    public int terrain_sides = 3;
    public bool doubleJump = false;

    public bool slow = false;

    public bool momentum = true;

    public bool doubleJumpUnlocked = false;



    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;

    public float swipeRange;
    public float tapRange;







    public const float MAX_SWIPE_TIME = 0.5f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    public const float MIN_SWIPE_DISTANCE = 0.17f;

    public static bool swipedRight = false;
    public static bool swipedLeft = false;
    public static bool swipedUp = false;
    public static bool swipedDown = false;


    public bool debugWithArrowKeys = true;

    Vector2 startPos;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        m_char = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_SIDE = SIDE.MID;
        colHeight = m_char.height;
        colCenterY = m_char.center.y;
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        swipeLeft = false;
        swipeRight = false;
        swipeUp = false;
        swipeDown = false;


        swipedRight = false;
        swipedLeft = false;
        swipedUp = false;
        swipedDown = false;

        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
                    return;

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                    return;

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                { // Horizontal swipe
                    if (swipe.x > 0)
                    {
                        swipedRight = true;
                    }
                    else
                    {
                        swipedLeft = true;
                    }
                }
                else
                { // Vertical swipe
                    if (swipe.y > 0)
                    {
                        swipedUp = true;
                    }
                    else
                    {
                        swipedDown = true;
                    }
                }
            }
        }

        if (debugWithArrowKeys)
        {
            swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
            swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
            swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
            swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
        }

        if (swipedLeft)
        {
            if (m_SIDE == SIDE.MID)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.LEFT;
            }
            else if (m_SIDE == SIDE.RIGHT)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.MID;
            }
            else if (m_SIDE == SIDE.LEFT && terrain_sides == 5)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.LEFTLEFT;
            }
            else if (m_SIDE == SIDE.RIGHTRIGHT)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.RIGHT;
            }
            else
            {
                lastSide = m_SIDE;
            }

            if (m_char.isGrounded && !inSlide)
            {
                m_Animator.Play("Jog Strafe Left");
            }
        }

        if (swipedRight)
        {
            if (m_SIDE == SIDE.MID)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.RIGHT;
            }
            else if (m_SIDE == SIDE.LEFT)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.MID;
            }
            else if (m_SIDE == SIDE.RIGHT && terrain_sides == 5)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.RIGHTRIGHT;
            }
            else if (m_SIDE == SIDE.LEFTLEFT)
            {
                lastSide = m_SIDE;
                m_SIDE = SIDE.LEFT;
            }
            else
            {
                lastSide = m_SIDE;
            }

            if (m_char.isGrounded && !inSlide)
            {
                m_Animator.Play("Jog Strafe Right");
            }
        }


        x = Mathf.Lerp(x, (int)m_SIDE, Time.deltaTime * speedDodge);
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, fwdSpeed * Time.deltaTime);
        m_char.Move(moveVector);
        Jump();
        Slide();
        BulletTime();
    }

    public void Jump()
    {
        if (m_char.isGrounded)
        {
            doubleJump = false;
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Floating") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Backflip") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("PoseOne"))
            {
                m_Animator.Play("Landing");
                //inJump = false;
            }
            if (swipedUp)
            {
                y = jumpPower;
                inJump = true;
                m_Animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
        }
        else
        {
            if (swipedUp && doubleJump == false && doubleJumpUnlocked == true)
            {
                doubleJump = true;
                y = jumpPower;
                inJump = true;
                m_Animator.CrossFadeInFixedTime("Backflip", 0.1f);
            }
            else
            {
                if (momentum == false)
                {
                    y = jumpPower / 2;
                    momentum = true;
                }
                else
                {
                    y -= jumpPower * 2 * Time.deltaTime;
                }
            }
        }
    }

    internal float slideCounter;

    public void Slide()
    {
        slideCounter -= Time.deltaTime;
        if (slideCounter <= 0f)
        {
            inSlide = false;
            slideCounter = 0f;
            m_char.center = new Vector3(0, colCenterY, 0);
            m_char.height = colHeight;
        }
        if (swipedDown)
        {
            if (!m_char.isGrounded)
            {
                if (swipedUp && doubleJump == false && doubleJumpUnlocked == true)
                {
                    doubleJump = true;
                    y += jumpPower;
                    inJump = true;
                }
                else
                {
                    y -= 60f;
                }
            }
            if (!inSlide && m_char.isGrounded)
            {
                slideCounter = 1f;
                inSlide = true;
                m_Animator.Play("Roll");
                m_char.center = new Vector3(0, colCenterY / 2f, 0);
                m_char.height = colHeight / 30f;
            }
        }
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            y = jumpPower;
        }
        else if (collision.gameObject.tag == "LowObstacle")
        {
            y = jumpPower / 4;
            fwdSpeed = 0;
            x -= 50f;
            m_Animator.CrossFadeInFixedTime("LowTrip", 0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RampPeak"))
        {
            y = 1.5f*jumpPower;
            inJump = true;
            m_Animator.CrossFadeInFixedTime("PoseOne", 0.3f);
        }
        else if (other.CompareTag("JumpPad"))
        {
            y = 0.5f*jumpPower;
            inJump = true;
            m_Animator.CrossFadeInFixedTime("Jump", 0.1f);
        }
        else if (other.CompareTag("LowObstacle"))
        {
            y = jumpPower / 4;
            fwdSpeed = 0;
            m_Animator.CrossFadeInFixedTime("StumbleBack", 0.1f);
        }
        else if (other.CompareTag("5WaySection"))
        {
            terrain_sides = 5;
        }
        else if (other.CompareTag("DoubleJump"))
        {
            doubleJumpUnlocked = true;
        }
    }

    void BulletTime()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (slow)
            {
                Resume();
            }
            else
            {
                SlowDown();
            }
        }
    }

    void SlowDown()
    {
        Time.timeScale = 0.5f;
        slow = true;
    }

    void Resume()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        slow = false;
    }
}
