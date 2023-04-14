using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    public TMP_Text textComponent; // Reference to the TMPro Text element
    public float letterDelay = 0.1f; // Delay between each letter
    public string textToWrite; // The text to be written letter by letter
    public char underscoreCharacter = '_'; // The character to be added behind the last letter
    public float blinkInterval = 0.5f; // Interval for blinking the underscore character

    private void Start()
    {
        StartCoroutine(WriteText());
    }

    IEnumerator WriteText()
    {
        textComponent.text = ""; // Clear the text initially
        foreach (char c in textToWrite)
        {
            textComponent.text += c; // Add one letter at a time
            yield return new WaitForSeconds(letterDelay); // Wait for the specified delay
        }

        textComponent.text += underscoreCharacter; // Add underscore character after typing all letters

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
