using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Win : MonoBehaviour
{
    public GameObject player;
    public GameObject timelineObject;
    public PlayableDirector timeline;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //timelineObject.SetActive(true);
            //player.SetActive(false);
            //timeline.Play();
        }
    }
}
