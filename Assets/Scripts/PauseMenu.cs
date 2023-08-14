using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    public static bool gameIsPaused = false;

    void Awake()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    // public void MainMenu()
    // {
    //     pauseMenuUI.SetActive(false);
    //     Time.timeScale = 1f;
    //     gameIsPaused = false;
    // }
}
