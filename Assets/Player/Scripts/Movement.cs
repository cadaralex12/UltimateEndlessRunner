using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum SIDE { LEFTLEFT = -30, LEFT = -15, MID = 0, RIGHT = 15, RIGHTRIGHT = 30 };

public enum HitX { Left, Mid, Right, None };
public enum HitY { Up, Mid, Down, Low, None };
public enum HitZ { Forward, Mid, Backward, None };

public class Movement : MonoBehaviour
{
    private float timer = 0;
    private float lastUpdate = 0;
    public int score = 0;
    public float seconds = 0;
    public float minutes = 0;
    public float miliseconds = 0;
    public bool isAlive = false;
    public SIDE m_SIDE = SIDE.MID;
    public SIDE lastSide;
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private CharacterController m_char;
    private Collider col;
    private float x;
    public float speedDodge;
    public float jumpPower = 25f;
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

    public int terrain_sides = 5;
    public bool doubleJump = false;

    public bool slow = false;

    public bool momentum = true;



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



    //private GroundSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        lastUpdate = 0;
        isAlive = true;
        //spawner = GetComponent<GroundSpawner>();
        //GameObject spawner = GameObject.FindGameObjectWithTag("GroundSpawner");
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
        timer += Time.deltaTime;
        minutes = Mathf.Floor(timer / 60);
        seconds = timer % 60;

        if (isAlive && (timer - lastUpdate > 0.25f))
        {
            score+=5;
            lastUpdate += 0.25f;
        }

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



        /*if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentPosition - startTouchPosition;

            if (!stopTouch)
            {

                if (Distance.x < -swipeRange)
                {
                    Debug.Log("Swipe left");
                    stopTouch = true;
                    swipeLeft = true;
                    
                }
                else if (Distance.x > swipeRange)
                {
                    Debug.Log("Swipe Right");
                    stopTouch = true;
                    swipeRight = true;
                }
                else if (Distance.y > swipeRange)
                {
                    Debug.Log("Swipe Up");
                    stopTouch = true;
                    swipeUp = true;
                }
                else if (Distance.y < -swipeRange)
                {
                    Debug.Log("Swipe Down");
                    stopTouch = true;
                    swipeDown = true;
                }

            }

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;

            endTouchPosition = Input.GetTouch(0).position;

            Vector2 Distance = endTouchPosition - startTouchPosition;

            if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
            {
                Debug.Log("Tap");
            }

        }*/


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
        if (!(col.transform.tag == "Player"))
            OnCharacterColliderHit(col);
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
            if (swipedUp && doubleJump == false)
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
                // fix to be able to double jump after stomp
                if (swipedUp && doubleJump == false)
                {
                    doubleJump = true;
                    y += jumpPower;
                    inJump = true;
                    //m_Animator.CrossFadeInFixedTime("jump1", 0.1f);
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
                //m_Animator.CrossFadeInFixedTime("Roll", 1f);
                m_Animator.Play("Roll");
                m_char.center = new Vector3(0, colCenterY / 2f, 0);
                m_char.height = colHeight / 30f;
            }
        }
    }
    public void OnCharacterColliderHit(Collider col)
    {
        hitX = GetHitX(col);
        hitY = GetHitY(col);
        hitZ = GetHitZ(col);

        if (hitZ == HitZ.Forward && hitX == HitX.Mid)
        {
            if (hitY == HitY.Low) // Stumble, -1 life
            {
                //m_Animator.Play("stumbleLow");
            }
            else if (hitY == HitY.Down) // KnockBack, fall, -1 life
            {
                //m_Animator.Play("knockback");
            }
            else if (hitY == HitY.Mid)
            {
                if (col.tag != "Ramp") //KnockBack, -1 life
                {
                    //m_Animator.Play("Bounce back");
                }
            }
            else if (hitY == HitY.Low && !inSlide)
            {
                //m_Animator.Play("stumbleLow");
            }
        }
        else if (hitZ == HitZ.Mid)
        {
            if (hitX == HitX.Right)
            {
                //m_Animator.Play("stumbleRight");
            }
            else if (hitX == HitX.Left)
            {
                //m_Animator.Play("stumbleLeft");
            }
        }
        else
        {
            if (hitX == HitX.Right)
            {
                //m_Animator.Play("StumblecornerRight");
                m_SIDE = lastSide;
            }
            else if (hitX == HitX.Left)
            {
                //m_Animator.Play("StumbleCornerLeft");
                m_SIDE = lastSide;
            }
        }
    }

    private void ResetCollision()
    {
        hitX = HitX.None;
        hitY = HitY.None;
        hitZ = HitZ.None;
    }

    public HitX GetHitX(Collider col)
    {
        Bounds char_bounds = m_char.bounds;
        Bounds col_bounds = col.bounds;
        float min_x = Mathf.Max(col_bounds.min.x, char_bounds.min.x);
        float max_x = Mathf.Max(col_bounds.max.x, char_bounds.max.x);
        float average = (min_x + max_x) / 2f - -col_bounds.min.x;
        HitX hit;
        if (average > col_bounds.size.x - 0.33f)
            hit = HitX.Right;
        else if (average < 0.33f)
            hit = HitX.Left;
        else
            hit = HitX.Mid;
        return hit;
    }

    public HitY GetHitY(Collider col)
    {
        Bounds char_bounds = m_char.bounds;
        Bounds col_bounds = col.bounds;
        float min_y = Mathf.Max(col_bounds.min.y, char_bounds.min.y);
        float max_y = Mathf.Max(col_bounds.max.y, char_bounds.max.y);
        float average = ((min_y + max_y) / 2f - char_bounds.min.y) / char_bounds.size.y;
        HitY hit;
        if (average < 0.17f)
            hit = HitY.Low;
        else if (average < 0.33f)
            hit = HitY.Down;
        else if (average < 0.66f)
            hit = HitY.Mid;
        else
            hit = HitY.Up;
        return hit;
    }

    public HitZ GetHitZ(Collider col)
    {
        Bounds char_bounds = m_char.bounds;
        Bounds col_bounds = col.bounds;
        float min_z = Mathf.Max(col_bounds.min.z, char_bounds.min.z);
        float max_z = Mathf.Max(col_bounds.max.z, char_bounds.max.z);
        float average = ((min_z + max_z) / 2f - char_bounds.min.z) / char_bounds.size.z;
        HitZ hit;
        if (average < 0.33f)
            hit = HitZ.Backward;
        else if (average < 0.66f)
            hit = HitZ.Mid;
        else
            hit = HitZ.Forward;
        return hit;
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
            y = jumpPower/4;
            fwdSpeed = 0;
            m_Animator.CrossFadeInFixedTime("StumbleBack", 0.1f);
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
