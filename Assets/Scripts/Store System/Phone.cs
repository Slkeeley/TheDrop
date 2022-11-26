using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Phone : MonoBehaviour
{
    public GameObject phoneScreen;
    public TMP_Text[] messages;
    public GameObject player;
    Player playerScript;

    [Header("Inventory")]
    public TMP_Text hatsText;
    public TMP_Text sweaterText;
    public TMP_Text shoesText;
    public TMP_Text streetName;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
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
        sweaterText.text = playerScript.sweatersHeld.ToString();
        shoesText.text = playerScript.shoesHeld.ToString();
        hatsText.text = playerScript.hatsHeld.ToString();
    }

    public void clearMessages()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            messages[i].text = "";
        }
    }


}
