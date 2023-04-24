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
    public AudioSource ding1, ding2, ding3;
    public float starSoundTimer = 0f;
    public int starSoundCounter = 0;
    private Animator m_Animator;
    public Movement player;
    public int respawn = 0;
    public GameObject camera1;
    public GameObject camera2;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public GameObject cart;
    public StarUIMovement UI;


    // Start is called before the first frame update

    void PlayMultipleSounds()
    {
        if(starSoundCounter < 2 && starSoundTimer < 0f)
        {
            ding3.Stop();
            ding2.Stop();
            ding1.Stop();
        }

    }
    
    void Update()
    {
        starSoundTimer -= Time.deltaTime;
        if(starSoundTimer < 0f){
            starSoundCounter = 0;
        }
        PlayMultipleSounds();
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        UI = FindObjectOfType<StarUIMovement>();
    }

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RampPeak"))
        {
            player.onRamp = true;
        }
        else if (other.CompareTag("JumpPad"))
        {
            player.y = 0.5f * player.jumpPower;
            player.inJump = true;
            m_Animator.Play("Jump");
        }
        else if (other.CompareTag("LowObstacle"))
        {
            player.fwdSpeed = 0;
            //player.y = player.jumpPower / 4;
            player.hasControl = false;
            //Vector3 moveVector = new Vector3(player.x - player.transform.position.x, player.y * Time.deltaTime, player.m_char.transform.position.z - 50);
            //player.m_char.Move(moveVector);
            m_Animator.Play("Big Blow");
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
        else if (other.GetComponent<Collectible>())
        {
            if (UI)
            {
                UI.SpawnMoveStar();
            }

            if(starSoundTimer > 0f){
                if (starSoundCounter == 1){
                    ding2.Play();
                    starSoundTimer = 0.5f;
                    starSoundCounter++;
                }
                else if (starSoundCounter >= 2){
                    starSoundTimer = 0.5f;
                    starSoundCounter++;
                    ding3.Play();
                }
                
            }  
            else {
                ding1.Play();
                starSoundTimer = 0.5f;
                starSoundCounter++;
            }

        }
        else if (other.CompareTag("ObstacleRight") || other.CompareTag("ObstacleLeft"))
        {
            player.m_SIDE = player.lastSide;
        }
        else if (other.CompareTag("FrontPlatform"))
        {
            player.y = player.jumpPower;
            m_Animator.Play("Floating");
            player.doubleJump = false;
        }
        else if (other.CompareTag("FrontWallFuckYou"))
        {
            player.m_SIDE = SIDE.MID;
            player.lastSide = SIDE.MID;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SplineCart"))
        {
            m_Animator.applyRootMotion = true;
            player.transform.SetParent(null);
        }
    }
}
