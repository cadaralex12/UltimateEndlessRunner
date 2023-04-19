using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameObject player;
    private Movement playerScript;

    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text timerText;
    public TMP_Text starsText;
    public TMP_Text livesText;
    public string minutesText;
    public string secondsText;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    
    int starsCounter = 0;
    int lives = 0;
    int score = 0;
    int highScore = 0;
    float minutes = 0;
    float seconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Movement>();
        score = 0;
        highScore = PlayerPrefs.GetInt("endlessHighScore");
        minutes = 0;
        seconds = 0;
        //Debug.Log("Saved Highscore: " + PlayerPrefs.GetInt("endlessHighScore"));
        scoreText.text = "Score: " + score.ToString() + " m";
        highScoreText.text = "High Score: " + highScore.ToString() + " m";
        starsText.text = starsCounter.ToString();
        //livesText.text = "Lives: " + lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        score = playerScript.score;
        minutes = playerScript.minutes;
        seconds = playerScript.seconds;
        starsCounter = playerScript.starsCounter;
        lives = playerScript.lives;

        scoreText.text = "Score: " + score.ToString() + " m";
        if (minutes < 10)
        {
            minutesText = "0" + minutes.ToString();
        }
        else
        {
            minutesText = minutes.ToString();
        }
        secondsText = seconds.ToString("00.000");
        
        timerText.text = minutesText + ":" + secondsText;
        highScoreText.text = "High Score: " + highScore.ToString() + " m";

        starsText.text = starsCounter.ToString();
        //livesText.text = "Lives: " + lives.ToString();

        if (PlayerPrefs.HasKey("endlessHighScore"))
        {
            //Debug.Log("HasKey");
            if (score >= PlayerPrefs.GetInt("endlessHighScore"))
            {
                Debug.Log("Score is bigger");
                highScore = score;
                PlayerPrefs.SetInt("endlessHighScore", highScore);
                PlayerPrefs.Save();
                //Debug.Log("Dick");
            }
        }
        else
        {
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("endlessHighScore", highScore);
                PlayerPrefs.Save();
            }
        }
        if(playerScript.lives == 2){
            life3.gameObject.SetActive(false);
        }
         if(playerScript.lives == 1){
            life3.gameObject.SetActive(false);
            life2.gameObject.SetActive(false);
        }
    }
    
}
