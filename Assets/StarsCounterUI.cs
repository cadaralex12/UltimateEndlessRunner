using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StarsCounterUI : MonoBehaviour
{
    public static Movement player;
    public int maxStars; // Maximum number of stars that can be collected
    public float countingSpeed; // Speed of the counting effect
    private TMP_Text starsText;
    private int currentStars=0; 

   void Start()
    {
        // Retrieve the maxStars value from PlayerPrefs
        maxStars = PlayerPrefs.GetInt("StarsCounter");
        starsText = GetComponent<TMP_Text>();
        currentStars = 0;
        StartCoroutine(CountStars());
    }

    IEnumerator CountStars()
    {
        while (currentStars < maxStars)
        {
            currentStars++;
            starsText.text = currentStars.ToString();
            yield return new WaitForSeconds(1 / countingSpeed);
        }
    }
}
