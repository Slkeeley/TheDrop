using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TMP_Text tutorialText;
    public int tip; 
    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            switch (tip)
            {
                case 1:
                    tutorialText.text = "Left Click to Punch, Right Click to Kick";
                    break;
                case 2:
                    tutorialText.text = "Middle Mouse to Block";
                    break;
                case 3:
                    tutorialText.text = "Spend money at stores like this one when drops happen";
                    break;
                case 4:
                    tutorialText.text = "Press M to open your phone";
                    break;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        tutorialText.text = "";
    }
}
