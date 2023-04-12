using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public int respawn;

    public Movement player;

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<Movement>();
        if (other.CompareTag("Player"))
        {
            if (this.CompareTag("Death"))
            {
                if (player.inBoost == false)
                {
                    PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins") + player.starsCounter);
                    Invoke("reloadScene", 0);
                }
                else
                {
                    player.y = player.jumpPower;
                }
            }
            else
            {
                if (player.inBoost == false && player.hurtCounter < 0f)
                {
                    if (player.lives > 1)
                    {
                        if (player.inShield == true)
                        {
                            player.inShield = false;
                            player.shield.SetActive(false);
                        }
                        else
                        {
                            player.lives--;
                        }
                        
                        player.y = player.jumpPower * 2 / 3;
                        player.m_Animator.Play("Floating");
                        player.hurtCounter = 0.8f;
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins") + player.starsCounter);
                        Invoke("reloadScene", 0);//this will happen after a delay of 1.5 seconds
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
