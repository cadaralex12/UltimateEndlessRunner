using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public int respawn;

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("reloadScene", 0);//this will happen after a delay of 1.5 seconds
        }
    }
}
