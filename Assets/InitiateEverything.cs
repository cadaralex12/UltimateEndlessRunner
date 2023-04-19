using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitiateEverything : MonoBehaviour
{
    public TMP_Text postProcessingSettingText;
    public static int postProcessingGlobal = 1;

    void Start()
    {
        if (!PlayerPrefs.HasKey("totalCoins"))
        {
            PlayerPrefs.SetInt("totalCoins", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("PostProcessingSetting"))
        {
            PlayerPrefs.SetInt("PostProcessingSetting", 1);
            PlayerPrefs.Save();
        }
        else
        {
            postProcessingGlobal = PlayerPrefs.GetInt("PostProcessingSetting");

            if (postProcessingGlobal == 1)
            {
                postProcessingSettingText.text = "Post Processing: ON";
            }
            else
            {
                postProcessingSettingText.text = "Post Processing: OFF";
            }
        }
    }

    public void TogglePostProcessing()
    {
        if (postProcessingGlobal == 1)
        {
            postProcessingGlobal = 0;
            postProcessingSettingText.text = "Post Processing: OFF";
        }
        else
        {
            postProcessingGlobal = 1;
            postProcessingSettingText.text = "Post Processing: ON";
        }
        PlayerPrefs.SetInt("PostProcessingSetting", postProcessingGlobal);
    }
    
}
