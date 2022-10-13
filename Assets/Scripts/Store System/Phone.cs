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
        if(Input.GetKey(KeyCode.M))
        {
            if(Time.timeScale==1.0)
            {
                phoneScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                phoneScreen.SetActive(false);
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
