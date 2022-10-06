using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
   public void restart()
    {
        SceneManager.LoadScene(0);
    }

    public void toFeedback()
    {
        Application.OpenURL("https://forms.gle/5R2zrJeEjW9v1ms19");
    }
}
