using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int respawn;
    public Movement player;

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(player.inSlide == true)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                Invoke("reloadScene", 0);//this will happen after a delay of 1.5 seconds
            }
        }
    }
}
