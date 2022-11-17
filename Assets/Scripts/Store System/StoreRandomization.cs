using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRandomization : MonoBehaviour
{
    public GameObject[] stores;//array of all store locations in the level
    public GameObject GameUI;
    public bool dropsActive=false;//are the stores opening?
    public int totalDrops;//how many stores are open at a time
    public float timeOpen; //how long do the stores stay open
    public float timeClosed; //how long do the stores stay closed
    public static bool crowBarSold = false; 
    public static bool brickSold = false;


    void Start()
    {
        crowBarSold = false;
        brickSold = false;
        StartCoroutine(waitToOpen());//begin opening stores at the start of the level
    }

    // Update is called once per frame
    void Update()
    {
        if (!dropsActive)//turn off all stores if drops are not active
        {
            foreach (GameObject i in stores)
            {
                i.GetComponent<Store>().open = false;
            }
        }
    }


    void chooseStore()//function to determine what stores are open
    {
        for (int i = 0; i < totalDrops; i++)
        {
            int storeSelected = Random.Range(0, stores.Length);

            if (stores[storeSelected].GetComponent<Store>().open == false)//if the house was not already selected open it up 
            {
                stores[storeSelected].GetComponent<Store>().beginOpening();
                GameUI.GetComponent<GameUI>().socialNotifications++;
            }
            else//if the house was already selected keep iterating
            {
                i--;
            }
        }
    }


    IEnumerator waitToOpen()//stores is closed, but waiting to open 
    {
        dropsActive = false;
        BuyerEnemy[] scalpers = GameObject.FindObjectsOfType<BuyerEnemy>();
        for (int i = 0; i < scalpers.Length; i++)
        {
            scalpers[i].doneShopping= true;
            scalpers[i].itemsBought = 0; 
        }
        yield return new WaitForSeconds(timeClosed);
        dropsActive = true;
        chooseStore();
        StartCoroutine(waitToClose());
    }

    IEnumerator waitToClose()//stores are open, but waitind to close 
    {
        BuyerEnemy[] scalpers = GameObject.FindObjectsOfType<BuyerEnemy>();
        for (int i = 0; i < scalpers.Length; i++)
        {
            scalpers[i].doneShopping = false;
            scalpers[i].itemsBought = 0;
        }
        yield return new WaitForSeconds(timeOpen);
        dropsActive = false;
        GameUI.GetComponent<GameUI>().socialNotifications = 0;
        StartCoroutine(waitToOpen());
    }
}
