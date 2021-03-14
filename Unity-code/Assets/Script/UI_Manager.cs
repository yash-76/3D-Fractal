using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public void Start_Button()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit_Button()
    {
        Application.Quit();
    }

    public void Resume_Button()
    {
        GameUI.CallResume();
    }

    public void Restart_Button()
    {
        Main.Restart_Level();
        GameUI.CallResume();
    }

    public void MainMenu_Button()
    {
        if (GameUI.LevelOver)
        {
            GameUI.LevelOver = false;
        }
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        GameUI.LevelOver = false;
        Main.Next_Level();
    }

    public void RetryLevel()
    {
        Main.Restart_Level();
        GameUI.IsGameOver = false;
    }

    public void MainMenuDone()
    {
        Main.LevelNo = 0;
        if (GameUI.IsGameComplete)
        {
            GameUI.IsGameComplete = false;
        }
        SceneManager.LoadScene(0);
    }
}
