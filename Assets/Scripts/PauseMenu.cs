using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseObjs; 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            if(Time.timeScale==1)
            {
                Time.timeScale = 0;
                pauseObjs.SetActive(true);
            }
        }
    }

    public void resumeGame()
    {
        if(Time.timeScale==0)
        {
            Time.timeScale = 1;
            pauseObjs.SetActive(false);
        }
    }
}
