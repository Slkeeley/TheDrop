using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameControl : MonoBehaviour
{
    [Header("Win Conditions")]
    public int moneyThreshold;
   /* public int sweaterThreshold; 
    public int shoesThreshold; 
    public int hatsThreshold;
    public int enemiesNeeded;
    public int cloutThreshold;
    */
    [Header("Data")]
    public GameObject player;
    public int playerMoney;
    public GameObject winText; 
 /*   public int sweatersSold;
    public int shoesSold;
    public int hatsSold;
    public int enemiesDefeated;
    public int cloutTotal;*/
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
        winText.SetActive(false);
        linkData(); 
    }

    // Update is called once per frame
    void Update()
    {
        linkData();
        if(playerMoney>=moneyThreshold)
        {
            winText.SetActive(true);
        }
    }

    void linkData()
    {
        playerMoney = player.GetComponent<Player>().money;
    }


}
