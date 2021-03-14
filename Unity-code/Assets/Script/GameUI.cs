using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static bool IsPause = false;
    public static bool LevelOver= false;
    private static bool CallR = false;
    public static bool IsGameOver = false;
    public static bool IsGameComplete = false;
    public GameObject PauseUI;
    public GameObject NextLevelUI;
    public GameObject GameOverUI;
    public GameObject GameCompleteUI;

    void Update()
    {
        if (IsGameComplete)
        {
            GameComplete();
        }
        if (IsGameOver)
        {
            GameOver();
        }
        else
        {
            GameNotOver();
        }
        if (LevelOver)
        {
            print("hmmmmm");
            NextLevelUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            NextLevelUI.SetActive(false);
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPause = !IsPause;
            if (IsPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        if (CallR)
        {
            CallR = false;
            IsPause = !IsPause;
            Resume();
        }
    }

    static public void CallResume()
    {
        CallR = true;
    }

    private void Resume()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {

        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void GameNotOver()
    {

        GameOverUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameComplete()
    {
        GameCompleteUI.SetActive(true);
    }


}
