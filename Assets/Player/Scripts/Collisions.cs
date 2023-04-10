using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class Collisions : MonoBehaviour
{
    private Animator m_Animator;
    public Movement player;
    public int respawn = 0;
    public GameObject camera1;
    public GameObject camera2;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public GameObject cart;


    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    /// Update is called once per frame
    /// 
    
    void Update()
    {
        
    }

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RampPeak"))
        {
            player.y = 1.5f * player.jumpPower;
            player.inJump = true;
            int ran = UnityEngine.Random.Range(0, 10);
            if (ran % 3 == 0)
            {
                m_Animator.CrossFadeInFixedTime("Pose1", 0.3f);
            }
            else if (ran % 3 == 1)
            {
                m_Animator.CrossFadeInFixedTime("Pose2", 0.3f);
            }
            else
            {
                m_Animator.CrossFadeInFixedTime("Pose3", 0.3f);
            }

            Vector3 rot;
            ran = UnityEngine.Random.Range(0, 10);
            if (ran % 2 == 0)
            {
                rot = new Vector3(0, 360, 0);
            }
            else
            {
                rot = new Vector3(0, -360, 0);
            }

            transform.DORotate(rot, 1.5f, RotateMode.LocalAxisAdd).SetLoops(1).SetEase(Ease.Linear);
        }
        else if (other.CompareTag("JumpPad"))
        {
            player.y = 0.5f * player.jumpPower;
            player.inJump = true;
            m_Animator.CrossFadeInFixedTime("Jump", 0.1f);
        }
        else if (other.CompareTag("LowObstacle"))
        {
            player.fwdSpeed = 0;
            //player.y = player.jumpPower / 4;
            player.hasControl = false;
            //Vector3 moveVector = new Vector3(player.x - player.transform.position.x, player.y * Time.deltaTime, player.m_char.transform.position.z - 50);
            //player.m_char.Move(moveVector);
            m_Animator.CrossFadeInFixedTime("Big Blow", 0.1f);
            Invoke("reloadScene", 1.5f);//this will happen after a delay of 1.5 seconds
        }
        else if (other.CompareTag("3DTo2D"))
        {
            targetPosition = camera2.transform.position;
            targetRotation = camera2.transform.rotation;
            //camera1.transform.DORotate(targetRotation, 0.5f, RotateMode.LocalAxisAdd).SetLoops(1).SetEase(Ease.Linear);
            //camera1.trasnform.position = targetPosition;
            //camera1.transform.position = Vector3.Translate(camera1.transform.position, targetPosition, 1f * Time.deltaTime);
            camera2.SetActive(true);
            camera1.SetActive(false);

        }
        else if (other.CompareTag("2DTo3D"))
        {
            targetPosition = camera2.transform.position;
            targetRotation = camera2.transform.rotation;
            //camera1.transform.DORotate(targetRotation, 0.5f, RotateMode.LocalAxisAdd).SetLoops(1).SetEase(Ease.Linear);
            //camera1.trasnform.position = targetPosition;
            
            camera1.SetActive(true);
            camera2.SetActive(false);

        }
        else if (other.CompareTag("SpawnCart"))
        {
            cart.SetActive(true);
        }
        else if (other.CompareTag("SplineCart"))
        {
            m_Animator.applyRootMotion = false;
            player.transform.SetParent(other.gameObject.transform);
            player.hasControl = false;
        }
        else if (other.CompareTag("Delete"))
        {
            //Quaternion NewRotation = player.transform.parent.transform.rotation;
            m_Animator.applyRootMotion = true;
            player.transform.SetParent(null);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            //player.transform.rotation = NewRotation;
            player.hasControl = true;
            player.transform.position = other.transform.parent.transform.position;
        }

    }

    /*void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SplineCart"))
        {
            player.transform.position = other.transform.parent.transform.position;
        }
    }*/

            void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SplineCart"))
        {
            m_Animator.applyRootMotion = true;
            player.transform.SetParent(null);
        }
    }
}
