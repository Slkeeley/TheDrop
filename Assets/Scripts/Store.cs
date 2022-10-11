using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Store : MonoBehaviour
{
    public bool playerEntered = false;//is the player in front of the store?
    bool canBuy= true;//boolean to make sure that the player can only buy one item at a time
    public bool open; //is the store open and selling items
    public GameObject player;
    public Transform south;


    [Header("Store UI")]//display what items aare being sold and for hwow much
    public GameObject items;
    public TMP_Text item1Text;
    public TMP_Text item2Text;
    public GameObject waitingCube; 
    public GameObject openCube; 

    [Header("Store Prices")]//prices for each item should be within different ranges
    public int itemsLeft;
    public int itemsMax;
    private string item1Txt; 
    private string item2txt;
    public StoreItem[] itemsSold;
    public StoreItem item1; 
    public StoreItem item2; 

    [Header("Store States")]//how long is the store open or waiting to open
    public float openTime; 
    //public float closeTime;
    public float waitingTime;
    public bool crowBarSold = false;
    public int crowBarChance; 
    public bool brickSold = false;
    public int brickChance;

    private void Awake()//turn off everything before opening
    {
        playerEntered = false;
        items.SetActive(false);
        openCube.SetActive(false);
        waitingCube.SetActive(false);
        player = GameObject.FindObjectOfType<Player>().gameObject;
        south = GameObject.Find("South").transform;
        //itemRandomizer(); 
    }
    private void Start()
    {
        items.transform.LookAt(south.position);
    }

    void Update()
    {
        if (open)//if open show items and their prices
        {
            if (playerEntered)
            {
                items.SetActive(true);
                buyItems();
            }
            else
            {
                items.SetActive(false);
            }
            openCube.SetActive(true);
            waitingCube.SetActive(false);
        }
        else//if the store is not open do not show items and show that the store is closed
        {
            items.SetActive(false);
            openCube.SetActive(false);
        }


        if (itemsLeft <= 0) open = false; //if all of the stores inventory has been bought shut it down
    }

    private void OnTriggerStay(Collider other)//if the player remains in front of the store they have entered
    {
        if (other.tag == "Player") playerEntered = true; 
    }

    private void OnTriggerExit(Collider other)//if a player moves away from the store they hav not entered
    {
        if (other.tag == "Player")
        {
            playerEntered = false;
        }
    }
    /*
    public void PriceRandomizer()//way to randomize prices of the items between drops
    {


        sweaterPriceCurr = Random.Range(sweaterPriceMin, sweaterPriceMax + 1);
        shoePriceCurr = Random.Range(shoePriceMin, shoePriceMax + 1);
        hatPriceCurr = Random.Range(hatPriceMin, hatPriceMax + 1);
        roundToTens();
        updatePrices();
    }

    void roundToTens()//round all prices to the nearest 10 to simplify calculation
    {
        sweaterPriceCurr = (sweaterPriceCurr / 10) * 10;
        shoePriceCurr = (shoePriceCurr / 10) * 10;
        hatPriceCurr = (hatPriceCurr / 10) * 10;
    }
    */
    void updatePrices()//make sure that the display text shows the correct price of the item. 
    {
        item1Text.text = "Press 1 to buy a " + item1.itemName + " for $" + item1.priceCurr.ToString(); 
        item2Text.text = "Press 2 to buy a " + item2.itemName + " for $" +item2.priceCurr.ToString();
    }

    void buyItems()//if player is able to let them buy items
    {
        if (canBuy)
        {
            if (Input.GetKey(KeyCode.Alpha1))//player buys item1 1
            {
                if (player.GetComponent<Player>().money >= item1.priceCurr)
                {
                    player.GetComponent<Player>().money = player.GetComponent<Player>().money - item1.priceCurr;
                    player.GetComponent<Player>().sweatersHeld++;
                    StartCoroutine(waitToBuy());
                }
            }

            if (Input.GetKey(KeyCode.Alpha2))//player buys item 2
            {
                if (player.GetComponent<Player>().money >= item2.priceCurr)
                {
                    player.GetComponent<Player>().money = player.GetComponent<Player>().money - item2.priceCurr;
                    player.GetComponent<Player>().shoesHeld++;
                    StartCoroutine(waitToBuy());
                }
            }

            
        }
    }

    public void beginOpening()//function to tell the player that the drop is about to happen
    {
        Debug.Log("beginning to open");
        itemsLeft = itemsMax;
        itemRandomizer();
        StartCoroutine(waitToOpen());
    }

    void itemRandomizer()
    {
        int leftItem = Random.Range(0, itemsSold.Length);
        int rightItem = Random.Range(0, itemsSold.Length);
        item1 = itemsSold[leftItem];
        item2 = itemsSold[rightItem];
        item1.PriceRandomizer(); 
        item2.PriceRandomizer();
        updatePrices(); 
    }

    IEnumerator waitToOpen()//store is closed but waiting to begin opening up
    {
        waitingCube.SetActive(true);
        yield return new WaitForSeconds(waitingTime);
        open = true; 
    }

    IEnumerator waitToBuy()//short cooldown to prevent players from buying more than one item at a time
    {
        canBuy = false;
        itemsLeft--;
        yield return new WaitForSeconds(0.5f);
        canBuy = true;
    }

}
