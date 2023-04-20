using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public Movement player;

    public GameObject PauseMenuUI;


    void Start()
    {
        PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public float lastTimeScale = 1f;

    public void onPauseButtonPress()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = lastTimeScale;
        GameIsPaused = false;
        player.inPause = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        GameIsPaused = true;
        player.inPause = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("LevelSelection");
        Time.timeScale = 1f;
    }
}
