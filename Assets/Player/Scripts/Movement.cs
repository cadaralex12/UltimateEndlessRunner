using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]

public enum SIDE { LEFTLEFT = -30, LEFT = -15, MID = 0, RIGHT = 15, RIGHTRIGHT = 30 };

public enum HitX { Left, Mid, Right, None };
public enum HitY { Up, Mid, Down, Low, None };
public enum HitZ { Forward, Mid, Backward, None };

public class Movement : MonoBehaviour
{
    public int starsCounter = 0;
    public GameObject shield;
    public float desiredFOV = 60f;
    public Camera myCamera;
    public GameObject ObtsacleDeleter;
    public int lives;
    public int direction = 0;
    public float zPosition = 0;
    public bool hasControl = true;
    public float timer = 0;
    public float lastUpdate = 0;
    public int score = 0;
    public float seconds = 0;
    public float minutes = 0;
    public float miliseconds = 0;
    public bool isAlive = false;
    public SIDE m_SIDE = SIDE.MID;
    public SIDE lastSide;
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    public CharacterController m_char;
    public Collider col;
    public float x;
    public float speedDodge;
    public float jumpPower = 25f;
    public float y;
    public bool inJump;
    public bool inSlide;
    public bool inAirSlide;
    public Animator m_Animator;
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

    public const float MAX_SWIPE_TIME = 0.2f;

    public const float MIN_SWIPE_DISTANCE = 0.01f;

    public bool swipedRight = false;
    public bool swipedLeft = false;
    public bool swipedUp = false;
    public bool swipedDown = false;
    public bool boostPressed = false;
    public bool shieldPressed = false;
    public bool slowMoPressed = false;

    public bool rotateRight = false;

    public bool debugWithArrowKeys = true;

    Vector2 startPos;
    float startTime;


    void Start()
    {
        lives = 3;
        timer = 0;
        lastUpdate = 0;
        isAlive = true;
        col = GetComponent<Collider>();
        m_char = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_SIDE = SIDE.MID;
        colHeight = m_char.height;
        colCenterY = m_char.center.y;
        transform.position = new Vector3(0,0.5f,0);
        ObtsacleDeleter.SetActive(false);
        shield.SetActive(false);
    }

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

        boostPressed = false;
        shieldPressed = false;
        slowMoPressed = false;

        rotateRight = false;

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
                /*if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
                    return;
                Why would you want this?
                */

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                /*if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                    return;*/

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                { 
                    // Horizontal swipe
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
                { 
                    // Vertical swipe
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
            rotateRight = Input.GetKeyDown(KeyCode.R);
            boostPressed = Input.GetKeyDown(KeyCode.C);
            shieldPressed = Input.GetKeyDown(KeyCode.X);
            slowMoPressed = Input.GetKeyDown(KeyCode.Z);
        }

        if (rotateRight) 
        {
            Vector3 rotation = new Vector3(0, 90, 0);
            this.transform.Rotate(rotation);
        }

        if (hasControl == true)
        {
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

            Slide();
            BulletTime();
            Hurt();
            Boost();
            Shield();
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, desiredFOV, Time.deltaTime * speedDodge);
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
                inAirSlide = true;
                y -= 100f;
            }
            if (!inSlide && m_char.isGrounded)
            {
                slideCounter = 0.7f;
                inSlide = true;
                m_Animator.Play("Roll");
                m_char.center = new Vector3(0, colCenterY / 2f, 0);
                m_char.height = colHeight / 10f;
            }
        }
        else if (inAirSlide && m_char.isGrounded)
        {
            
            slideCounter = 0.7f;
            inSlide = true;
            
            
            m_char.center = new Vector3(0, colCenterY / 2f, 0);
            m_char.height = colHeight / 10f;
            inAirSlide = false;
        }
    }
    /*public void OnCharacterColliderHit(Collider col)
    {
        hitX = GetHitX(col);
        hitY = GetHitY(col);
        hitZ = GetHitZ(col);

        if (hitZ == HitZ.Forward && hitX == HitX.Mid)
        {
            if (hitY == HitY.Low) // Stumble, -1 life
            {
                Debug.Log("Damn");
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
    }*/

    internal float slowMoCounter = 0f;
    public bool inSlowMo = false;

    public void BulletTime()
    {
        slowMoCounter -= Time.deltaTime;
        if (slowMoCounter <= 0f && inSlowMo == true)
        {
            Resume();
        }
        if (slowMoPressed && Time.timeScale != 0f && starsCounter >= 10)
        {
            starsCounter -= 10;
            slowMoPressed = false;
            slowMoCounter = 3f;
            SlowDown();
        }
    }

    void SlowDown()
    {
        inSlowMo = true;
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        desiredFOV = 45f;
        slow = true;
    }

    void Resume()
    {
        desiredFOV = 60f;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        slow = false;
    }

    internal float boostCounter = 0f;
    public bool inBoost = false;

    public void Boost()
    {
        boostCounter -= Time.deltaTime;
        if (inBoost == true)
        {
            if (boostCounter <= 0f)
            {
                myCamera.transform.position -= new Vector3(0, 100f, 0);
                inBoost = false;
                boostCounter = 0f;
                desiredFOV = 60f;
                fwdSpeed = 150;
            }
        }
        
        if (boostPressed && inBoost == false && starsCounter >= 5 && hurtCounter <= 0f)
        {
            starsCounter -= 5;
            boostPressed = false;
            desiredFOV = 80f;
            myCamera.transform.position += new Vector3(0, 100f, 0);
            inBoost = true;
            boostCounter = 5f;
            fwdSpeed = 220;
            m_Animator.Play("Boost");
        }
    }

    public bool inShield = false;

    public void Shield()
    {
        if (shieldPressed && starsCounter >= 10)
        {
            starsCounter -= 10;
            shieldPressed = false;
            inShield = true;
            shield.SetActive(true);
        }
    }

    void Blink()
    {
        if (GetComponent<Renderer>().enabled == true)
            GetComponent<Renderer>().enabled = false;
        else
            GetComponent<Renderer>().enabled = true;
    }

    public float hurtCounter = 0;

    void Hurt()
    {
        hurtCounter -= Time.deltaTime;
        if (hurtCounter > 0f)
        {
            ObtsacleDeleter.SetActive(true);
            desiredFOV = 50f;
            //InvokeRepeating("Blink", 0, 0.2f);
            Time.timeScale = 0.4f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
        }
        else if (Time.timeScale!=0f && Time.timeScale !=0.5f && inBoost == false)
        {
            /*CancelInvoke("Blink");
            if (GetComponent<Renderer>().enabled == false)
                GetComponent<Renderer>().enabled = true;*/
            ObtsacleDeleter.SetActive(false);
            desiredFOV = 60f;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
        }

        
    }
}
