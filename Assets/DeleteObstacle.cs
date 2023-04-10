using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            UnityEngine.Debug.Log("Dicks Enter");
            other.gameObject.SetActive(false);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            UnityEngine.Debug.Log("Dicks Stay");
            other.gameObject.SetActive(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            UnityEngine.Debug.Log("Dicks Exit");
            other.gameObject.SetActive(false);
        }
    }
}
