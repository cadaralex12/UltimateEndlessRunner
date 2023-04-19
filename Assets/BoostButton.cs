using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostButton : MonoBehaviour
{
public Movement player;
public Image timerBar;
public float duration = 3.0f;
private bool boostActive = false;
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
    if (boostActive)
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0.0f)
        {
            boostActive = false;
            player.boostPressed = false;
            timerBar.gameObject.SetActive(false);
        }
        else
        {
            float t = timeRemaining / duration;
            float progress = 1.0f - t;
            float barFill = progress * barWidth;
            timerBar.rectTransform.sizeDelta = new Vector2(barWidth - barFill, timerBar.rectTransform.sizeDelta.y);

            float r = 120;
            float g = barColor.g;
            float b = 0;
            timerBar.color = new Color(r, g, b);
        }
    }
}

public void ActivateBoost()
{
    if (!boostActive)
    {
        boostActive = true;
        player.boostPressed = true;
        player.Boost();

        timeRemaining = duration;
        timerBar.gameObject.SetActive(true);
        timerBar.rectTransform.sizeDelta = new Vector2(barWidth, timerBar.rectTransform.sizeDelta.y);
        timerBar.color = barColor;
    }
}
}