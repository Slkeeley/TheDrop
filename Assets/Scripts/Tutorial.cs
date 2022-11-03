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
                    tutorialText.text = "Left Click to Punch, Middle Mouse to block";
                    break;
                case 2:
                    tutorialText.text = "Right Click to kick, kicking can be used to break enemy blocks";
                    break;
                case 3:
                    tutorialText.text = "After stacking your bread you can spend it at stores like this one when drops happen";
                    break;
                case 4:
                    tutorialText.text = "Press M to open your messages, now find those buyers and sell to them";
                    break;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        tutorialText.text = "";
    }
}
