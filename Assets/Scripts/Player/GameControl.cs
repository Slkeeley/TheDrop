using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [Header("Win Conditions")]//how much of each thing does a player need to beat the level 
    public float moneyThreshold;

    [Header("Data")]//where is the player currently at in terms of progress
    public GameObject player;
    public float playerMoney;
    public GameObject winText; 
    public static int enemiesInPlay;
    public int maxEnenmies;
    public int LvlValue; 

  void Start()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
        enemiesInPlay = 0; 
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
            LevelUnlock.unlockLvl(LvlValue);
         SceneManager.LoadScene("VictoryScreenTemp");
        }
    }

    void linkData()//make sure that this object is tracking the player's data
    {
        playerMoney = player.GetComponent<Player>().money;
    }


}
