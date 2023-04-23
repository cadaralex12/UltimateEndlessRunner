using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Movement player;
    public bool isAttracted = false;
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
        if (player.inBoost == true || isAttracted == true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 100f)
            {
                isAttracted = true;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 250f * Time.deltaTime);
            }
        }
    }
}
