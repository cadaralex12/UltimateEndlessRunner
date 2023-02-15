using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public bool isGrounded;
    public bool justJumped;
    public Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    private Animator animator;
    public float jumpMultiplier = 2.5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isGrounded = true;
    }

    void Update()
    {
        /*isGrounded = controller.isGrounded;
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.Play("Jump");
                moveDirection.y = jumpSpeed;
                animator.SetFloat("yVelocity", moveDirection.y);
                isGrounded = false;
                animator.SetBool("isGrounded", false);
            }
        }
        moveDirection.y -= gravity * speed * Time.deltaTime;
        moveDirection.z = 0;
        moveDirection.x = 0;
        controller.Move(moveDirection * Time.deltaTime);
        animator.SetFloat("yVelocity", moveDirection.y);*/
    }
}