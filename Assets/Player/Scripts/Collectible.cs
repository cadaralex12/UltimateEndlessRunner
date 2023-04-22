using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Movement player;
    void Start()
    {
        player = FindObjectsOfType<Movement>()[0];
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.starsCounter++;
            player.stylePoints++;
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 100f)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 250f * Time.deltaTime);
    }
}
