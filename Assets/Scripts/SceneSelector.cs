using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public static int currLevel = 4; 
    public void restart()
    {
        SceneManager.LoadScene(currLevel);
    }

    public void nextLevel()
    {
        currLevel++;
        SceneManager.LoadScene(currLevel);
    }

    public void toLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void backToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void quitGame()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }

    public void toFeedback()
    {
        Application.OpenURL("https://forms.gle/zSoBa5fP9Ncf6eu19");
    }

    public void tutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void lvl1()
    {
        SceneManager.LoadScene("Level1");
    }
}
