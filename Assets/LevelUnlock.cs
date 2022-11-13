using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{
    public Button levelButton;
    public static void unlockLevel()
    {
        this.turnOnButton();
    }

    public void turnOnButton()
    {
        if (!levelButton.interactable)
            levelButton.interactable = true;
    }

}
