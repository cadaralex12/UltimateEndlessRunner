using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum SIDE { LEFTLEFT = -14, LEFT = -7, MID = 0, RIGHT = 7, RIGHTRIGHT = 14 };

public enum HitX { Left, Mid, Right, None };
public enum HitY { Up, Mid, Down, Low, None };
public enum HitZ { Forward, Mid, Backward, None };

public class Movement : MonoBehaviour
{
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
    public float fwdSpeed = 25f;

    private float colHeight;
    private float colCenterY;

    public HitX hitX = HitX.None;
    public HitY hitY = HitY.None;
    public HitZ hitZ = HitZ.None;

    public int terrain_sides = 5;
    public bool doubleJump = false;

    public bool slow = false;

    //private GroundSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
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
        //terrain_sides = spawner.lastWidth;
        swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        if (swipeLeft)
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

        if (swipeRight)
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
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Floating") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Backflip"))
            {
                m_Animator.Play("Landing");
                //inJump = false;
            }
            if (swipeUp)
            {
                y = jumpPower;
                inJump = true;
                m_Animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
        }
        else
        {
            if (swipeUp && doubleJump == false)
            {
                doubleJump = true;
                y = jumpPower;
                inJump = true;
                m_Animator.CrossFadeInFixedTime("Backflip", 0.1f);
            }
            else
            {
                y -= jumpPower * 2 * Time.deltaTime;
                //if (m_char.velocity.y < -0.1f)
                    //m_Animator.Play("");
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
        if (swipeDown)
        {
            if (!m_char.isGrounded)
            {
                // fix to be able to double jump after stomp
                if (swipeUp && doubleJump == false)
                {
                    doubleJump = true;
                    y += jumpPower;
                    inJump = true;
                    //m_Animator.CrossFadeInFixedTime("jump1", 0.1f);
                }
                else
                {
                    y -= 30f;
                }
            }
            if (!inSlide && m_char.isGrounded)
            {
                slideCounter = 1f;
                inSlide = true;
                Debug.Log("Ass");
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
            Debug.Log("Fuck");
            y = jumpPower;
        }
        //if (collision.gameObject.name == "Trambulin")
        //{
        //    GetComponent<Rigidbody>().AddForce(0, 750, 0);
        //}
    }

    void OnPlayerCollisionEnter(Collider other)
    {
        if (other.CompareTag("LowerEnemy"))
        {
            Debug.Log("Pula");
            m_Animator.Play("Fall Flat");
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
