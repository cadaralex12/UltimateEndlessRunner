using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowMotionButton : MonoBehaviour
{
    public Movement player;
    public Image timerBar;
    public float duration = 5.0f;

    private bool slowMotionActive = false;
    private float timeRemaining = 0.0f;
    private float barWidth = 200.0f;
    private Color barColor;

    private void Awake()
    {
        barWidth = timerBar.rectTransform.sizeDelta.x;
        barColor = timerBar.color;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && player.starsCounter>=50){
            ActivateSlowMotion(); 
            timerBar.gameObject.SetActive(true);

        }
        if (slowMotionActive)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0.0f)
            {
                slowMotionActive = false;
                player.slowMoPressed = false;
                timerBar.gameObject.SetActive(false);
            }
            else
            {
                float t = timeRemaining / duration;
                float progress = 1f - t;
                float barFill = progress * barWidth;
                timerBar.rectTransform.sizeDelta = new Vector2(barWidth - barFill, timerBar.rectTransform.sizeDelta.y);

                float r = 120;
                float g = barColor.g * t;
                float b = 0;
                timerBar.color = new Color(r, g, b);
            }
        }
    }

    public void ActivateSlowMotion()
    {
        if (!slowMotionActive)
        {
            slowMotionActive = true;
            player.slowMoPressed = true;
            player.BulletTime();

            timeRemaining = duration;
            timerBar.gameObject.SetActive(true);
            timerBar.rectTransform.sizeDelta = new Vector2(barWidth, timerBar.rectTransform.sizeDelta.y);
            timerBar.color = barColor;
        }
    }
}
