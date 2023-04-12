using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateEverything : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("totalCoins"))
        {
            PlayerPrefs.SetInt("totalCoins", 0);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
