using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{
    public Button levelButton_1;
    public static bool unLockLvl1 = false;

    private void Start()
    {
        if(unLockLvl1)
        {
            levelButton_1.interactable=true;
        }
        else
        {
            levelButton_1.interactable = false;
        }

    }

    public static void unlockLvl(int lvlValue)
    {
        switch(lvlValue)
        {
            case 1:
                unLockLvl1 = true;
                break;
            default:
                break;
        }
    }
}
