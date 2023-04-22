using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    public TMP_Text textComponent; // Reference to the TMPro Text element
    public float letterDelay = 0.1f; // Delay between each letter
    public string[] linesToWrite; // The array of lines to be written
    public char underscoreCharacter = '_'; // The character to be added behind the last letter
    public float blinkInterval = 0.5f; // Interval for blinking the underscore character

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(WriteText());
    }

    IEnumerator WriteText()
    {
        textComponent.text = ""; // Clear the text initially
        for (int i = 0; i < linesToWrite.Length; i++)
        {
            string line = linesToWrite[i]; // Get the current line
            for (int j = 0; j < line.Length; j++)
            {
                textComponent.text += line[j]; // Add one letter at a time
                yield return new WaitForSeconds(letterDelay); // Wait for the specified delay
            }

            if (i < linesToWrite.Length - 1)
            {
                textComponent.text += "\n"; // Add new line after typing each line except the last line
            }
        }

        textComponent.text += underscoreCharacter; // Add underscore character after typing all lines

        // Blink the underscore character
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval); // Wait for the blink interval
            textComponent.text = textComponent.text.Remove(textComponent.text.Length - 1); // Remove underscore character
            yield return new WaitForSeconds(blinkInterval); // Wait for the blink interval
            textComponent.text += underscoreCharacter; // Add underscore character
        }
    }
}
