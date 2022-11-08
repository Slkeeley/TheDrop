using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Phone : MonoBehaviour
{
    public GameObject phoneScreen;
    public TMP_Text[] messages;  
    // Start is called before the first frame update
    void Start()
    {
        phoneScreen.SetActive(false);
        clearMessages(); 
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (phoneScreen.activeInHierarchy)
            {
                phoneScreen.SetActive(false);
            }
            else
            {
                phoneScreen.SetActive(true);
            }
        }
    }

    public void clearMessages()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            messages[i].text = "";
        }
    }

}
