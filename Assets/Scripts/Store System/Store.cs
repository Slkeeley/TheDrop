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
    public GameObject UI;
    public GameObject streetLocation;
    public GameObject front; 

    [Header("Store UI")]//display what items aare being sold and for hwow much
    public GameObject items;
    public TMP_Text item1Text;
    public TMP_Text item2Text;
    public TMP_Text itemsLeftText;
    public Image item1Image; 
    public Image item2Image; 
    public GameObject waitingCube; 
    public GameObject openItems; 

    [Header("Store Items")]//prices for each item should be within different ranges
    public int itemsLeft;
    public int itemsMax;
    private string item1Txt; 
    private string item2txt;
    public StoreItem[] itemsSold;
    public StoreItem item1; 
    public StoreItem item2; 

    [Header("Store States")]//how long is the store open or waiting to open
    public float waitingTime;
    public int crowBarChance; 
    public int brickChance;
    public bool scalped = false; 

    private void Awake()//turn off everything before opening
    {
        playerEntered = false;
        items.SetActive(false);
        openItems.SetActive(false);
        waitingCube.SetActive(false);
        player = GameObject.FindObjectOfType<Player>().gameObject;
        UI = GameObject.FindObjectOfType<GameUI>().gameObject;
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
            openItems.SetActive(true);
            waitingCube.SetActive(false);
        }
        else//if the store is not open do not show items and show that the store is closed
        {
            items.SetActive(false);
            openItems.SetActive(false);
        }

        checkInv(); 
        if (itemsLeft <= 0) open = false; //if all of the stores inventory has been bought shut it down
    }

    private void OnTriggerStay(Collider other)//if the player remains in front of the store they have entered
    {
        if (other.tag == "Player") playerEntered = true; 
        if(other.tag=="Buyer")
        {
            
            Debug.Log("Scalper is trying to buy from the store");
            if(!scalped)
            {               
                StartCoroutine(sellToEnemies());
                other.GetComponent<BuyerEnemy>().itemsBought++;
            }
        }
    }

    private void OnTriggerExit(Collider other)//if a player moves away from the store they hav not entered
    {
        if (other.tag == "Player")
        {
            playerEntered = false;
        }
    }
   
    void updateItems()//make sure that the display text shows the correct price of the item. 
    {
        item1Image.GetComponent<Image>().sprite = item1.itemImage;
        item2Image.GetComponent<Image>().sprite = item2.itemImage;
        item1Text.text = "$" + item1.priceCurr.ToString(); 
        item2Text.text =  "$" +item2.priceCurr.ToString();
        
    }

    void checkInv()
    {
        itemsLeftText.text = "Items Left: " + itemsLeft.ToString();
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
                    giveItem1();
                    StartCoroutine(waitToBuy());
                }
            }

            if (Input.GetKey(KeyCode.Alpha2))//player buys item 2
            {
                if (player.GetComponent<Player>().money >= item2.priceCurr)
                {
                    player.GetComponent<Player>().money = player.GetComponent<Player>().money - item2.priceCurr;
                    giveItem2(); 
                    StartCoroutine(waitToBuy());
                }
            }

            
        }
    }

    public void beginOpening()//function to tell the player that the drop is about to happen
    {
      //  Debug.Log("beginning to open");
        itemsLeft = itemsMax;
        itemRandomizer();
        StartCoroutine(waitToOpen());
    }

    void itemRandomizer()//way to randomize what two items are present in the store. 
    {
        int leftItem = Random.Range(0, 3);
        int rightItem = Random.Range(0, 3);
        item1 = itemsSold[leftItem];
        item2 = itemsSold[rightItem];
        item1.PriceRandomizer(); 
        item2.PriceRandomizer();
        if (crowbarSpawn())
        {
            item1 = itemsSold[3];
        }
        if (brickSpawn())
        {
            item2 = itemsSold[4];
        }
        updateItems(); 
    }

    bool crowbarSpawn()//does the crowbar appear in the store
    {
        if (StoreRandomization.crowBarSold) return false; 
        int spawn = Random.Range(1, 101);
        if (spawn <= crowBarChance) return true;
        else return false; 
    }
    bool brickSpawn()//does the brick appear in the store
    {
        if (StoreRandomization.brickSold) return false; 
        int spawn = Random.Range(1, 101);
        if (spawn <= brickChance) return true;
        else return false;
    }

    void giveItem1()//give the left item to the player if they have purchased it
    {
        switch (item1.itemName)
        {
            case "Sweater":
                player.GetComponent<Player>().sweatersHeld++;
                player.GetComponent<Player>().sweaterAcquired();
                break;
            case "Shoes":
                player.GetComponent<Player>().shoesHeld++;
                player.GetComponent<Player>().shoeAcquired();
                break;
            case "Hat":
                player.GetComponent<Player>().hatsHeld++;
                player.GetComponent<Player>().hatAcquired();
                break;
            case "Crowbar":
                player.GetComponent<Player>().hasCrowbar = true;
                StoreRandomization.crowBarSold = true; 
                break;
        }
    }
    void giveItem2()//give the right item to the player if they have purchased it
    {
        switch (item2.itemName)
        {
            case "Sweater":
                player.GetComponent<Player>().sweatersHeld++;
                player.GetComponent<Player>().sweaterAcquired();
                break;
            case "Shoes":
                player.GetComponent<Player>().shoesHeld++;
                player.GetComponent<Player>().shoeAcquired();
                break;
            case "Hat":
                player.GetComponent<Player>().hatsHeld++;
                player.GetComponent<Player>().hatAcquired();
                break;
            case "Brick":
                player.GetComponent<Player>().hasBrick = true;
                StoreRandomization.brickSold= true;
                break;
        }
    }

    IEnumerator waitToOpen()//store is closed but waiting to begin opening up
    {
        waitingCube.SetActive(true);
        waitingTime = GetComponentInParent<StoreRandomization>().timeOpen / 3;
        UI.GetComponent<GameUI>().dropText.text = "The next drop is happening on " + streetLocation.name + " in " + waitingTime.ToString() + " seconds";
        UI.GetComponent<GameUI>().dropAnnouncement();
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

    IEnumerator sellToEnemies()
    {
        scalped = true;
        itemsLeft--;
        yield return new WaitForSeconds(3);
        scalped = false; 
    }
}
