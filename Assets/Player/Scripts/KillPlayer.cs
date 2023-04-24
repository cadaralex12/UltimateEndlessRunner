using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public int respawn;

    //public GameObject continueCanvas;

    public Movement player;

    void Start()
    {
        player = FindObjectsOfType<Movement>()[0];
    }

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }
    
    void Update()
    {
        if(player.hurtCounter > 0f && Vector3.Distance(transform.position, player.transform.position) < 150f)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<Movement>();
        if (other.CompareTag("Player"))
        {
            player.stylePoints -= 500;
            if (this.CompareTag("Death"))
            {
                if (player.inBoost == false && player.inShield == false)
                {
                    PlayerPrefs.SetInt("StarsCounter", player.starsCounter);
                    PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins") + player.starsCounter);
                    player.isDead = true;
                }
                else
                {
                    if (player.inShield == true)
                    {
                        player.inShield = false;
                        player.shield.SetActive(false);
                    }
                    player.m_Animator.Play("Floating");
                    player.y = player.jumpPower;
                    player.doubleJump = false;
                }
            }
            else
            {

                if (player.inBoost == false && player.hurtCounter < 0f)
                {
                    player.y = player.jumpPower * 2;
                    if (player.lives > 1)
                    {
                        if (player.inShield == true)
                        {
                            player.inShield = false;
                            player.shield.SetActive(false);
                            player.m_Animator.Play("Floating");
                            player.hurtCounter = 0.8f;
                        }
                        else
                        {
                            player.lives--;
                        }
                        
                        player.m_Animator.Play("Floating");
                        player.hurtCounter = 0.8f;
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (player.inShield == true)
                        {
                            player.inShield = false;
                            player.shield.SetActive(false);
                            player.m_Animator.Play("Floating");
                            player.hurtCounter = 0.8f;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("StarsCounter", player.starsCounter);
                            PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins") + player.starsCounter);
                            player.isDead = true;
                            //SceneManager.LoadScene("VictoryScene");
                        }
                    }
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            
        }

    }
}
