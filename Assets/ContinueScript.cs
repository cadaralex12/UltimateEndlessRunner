using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueScript : MonoBehaviour
{
    public Movement player;
    public GameObject yesBtn;
    void Start(){
        if(player.starsCounter < player.respawnCost){
            yesBtn.SetActive(false);
        }
    }
    public void PressYes()
    {
        player.isDead = false;
        player.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        player.starsCounter -= player.respawnCost;
        player.respawnCost *= 2;
        player.y = player.jumpPower*2;
        player.m_Animator.Play("Floating");
        player.hurtCounter = 0.8f;
        Time.timeScale = 1f;
        player.lives = 1;
        player.continueCanvas.SetActive(false);
    }

    
    public void PressNo()
    {
      PlayerPrefs.SetInt("StarsCounter", player.starsCounter);
      PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins") + player.starsCounter);
      player.isDead = true;
      SceneManager.LoadScene("VictoryScene");   
      player.continueCanvas.SetActive(false);
      Time.timeScale = 1f;
    }
}
