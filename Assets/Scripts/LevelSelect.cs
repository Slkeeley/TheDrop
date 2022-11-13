using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
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

    public void tutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void lvl1()
    {
        SceneManager.LoadScene("Level1");
    }
}
