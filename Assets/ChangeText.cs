using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] texts;
    private float timer = 0f;
    private int index = 0;

    void Start()
    {
        // Set the initial text
        textMeshPro.text = texts[index];
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // If the timer has reached 9 seconds, change the text
        if (timer >= 7.5f)
        {
            // Reset the timer
            timer = 0f;

            // Increment the index
            index++;

            // If the index is greater than or equal to the number of texts, wrap around to the beginning
            if (index >= texts.Length)
            {
                index = 0;
            }

            // Set the text to the next string in the array
            textMeshPro.text = texts[index];
        }
    }
}
