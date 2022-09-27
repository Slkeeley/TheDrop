using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameControl : MonoBehaviour
{
    [Header("Win Conditions")]//how much of each thing does a player need to beat the level 
    public int moneyThreshold;
   /* public int sweaterThreshold; 
    public int shoesThreshold; 
    public int hatsThreshold;
    public int enemiesNeeded;
    public int cloutThreshold;
    */
    [Header("Data")]//where is the player currently at in terms of progress
    public GameObject player;
    public int playerMoney;
    public GameObject winText; //placeholder, this will appear once a player has won
 /*   public int sweatersSold;
    public int shoesSold;
    public int hatsSold;
    public int enemiesDefeated;
    public int cloutTotal;*/

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
        if(playerMoney>=moneyThreshold)//if the player has enough of something they win
        {
            winText.SetActive(true);
        }
    }

    void linkData()//make sure that this object is tracking the player's data
    {
        playerMoney = player.GetComponent<Player>().money;
    }


}
