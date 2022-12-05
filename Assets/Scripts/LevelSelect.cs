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
        SceneManager.LoadScene(6);
    }

    public void lvl2()
    {
        SceneManager.LoadScene(7);
    }


public void lvl3()
{
    SceneManager.LoadScene(8);
}


    public void lvl4()
{
    SceneManager.LoadScene(9);
}


    public void lvl5()
{
    SceneManager.LoadScene("Level3Day");
}

public void lvl6()
{
    SceneManager.LoadScene("Level3Night");
}


public void lvl7()
{
    SceneManager.LoadScene("Level4Day");
}

    public void lvl8()
    {
        SceneManager.LoadScene("Level4Da");
    }


}

