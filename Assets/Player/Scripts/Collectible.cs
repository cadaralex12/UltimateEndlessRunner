using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement player = other.gameObject.GetComponent<Movement>();
            player.starsCounter++;
            player.stylePoints++;
            this.gameObject.SetActive(false);
        }
    }
}
