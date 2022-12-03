using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyerHouse : MonoBehaviour
{
    public bool playerEntered = false;//is the player in front of the house
    public bool open = true;//is the house offering to buy something?
    bool sold = false;//boolean to make sure that the player can only buy one item at a time
    public GameObject player;
    public GameObject streetLocation; 
   
    [Header("Store UI")]//the canvas that shows what the house is trying to by and for how much
    public GameObject canvas;
    public TMP_Text priceText;
    public Image desiredItem; 

    [Header("Selling")]//data that determines what is being bought and for how much
    public int itemPriceMin = 40;
    public int itemPriceMax = 120;
    public int itemPriceCurr;
    public StoreItem[] potentialItems; 
    public StoreItem itemSold;
    public string item;
    AudioSource source;
    public AudioClip sellSound; 


    private void Awake()
    {
        source = GetComponent<AudioSource>(); 
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       if(open && !sold)
        {
            canvas.SetActive(true);
            if (playerEntered)//only be able to sell to the player if the house is open, not already sold to, and if the player is next to it 
            {
                playerSell();
            }
        }
       if(!open || sold)//if the house is closed or the item was sold to the house get rid of the canvas
        {
            canvas.SetActive(false);
        }

    }

    private void OnTriggerStay(Collider other)//player has entered if they stay by the house
    {
        if (other.tag == "Player")
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)//if the player leaves the front of the house they have no longer entered
    {
        if (other.tag == "Player")
        {
            playerEntered = false;
        }
    }

    public void openHouse()//function to reset what is being bought and when the house is open
    {
        open = true;
        sold = false;
        randomizeItems();
        updatePrices(); 
    }


    void updatePrices()//make sure that the display text shows the correct price of the item. 
    {
        desiredItem.GetComponent<Image>().sprite = itemSold.resellListingImage;
        priceText.text = "Press SPACE to sell for $" + itemPriceCurr.ToString();

    }

   void randomizeItems()//determine what item is being bought and what they will pay for it
    {
        int itemNumber = Random.Range(1, 4);
        switch(itemNumber)
        {
            case 1:
                itemSold = potentialItems[0];
                item = "Sweater";
                break;
            case 2:
                itemSold = potentialItems[1];
                item = "Shoes";
                break;
            case 3:
                itemSold = potentialItems[2];
                item = "Hat";
                break;
        }
        itemPriceCurr = Random.Range(itemPriceMin, itemPriceMax+1);//randomize the price of the item
        itemPriceCurr = (itemPriceCurr / 10) * 10;//round the randomized price to the nearest 10 for simpler calculations
    }

    void playerSell()//if the player is able to, they can sell items to the house
    {
        switch(item)
        {
            case "Sweater":
                if (Input.GetKey(KeyCode.Space))
                {
                    if (player.GetComponent<Player>().sweatersHeld>0)//sell sweaters if the player has any 
                    {
                        source.PlayOneShot(sellSound, 1);
                        player.GetComponent<Player>().sweatersHeld--;
                        player.GetComponent<Player>().money = player.GetComponent<Player>().money + itemPriceCurr;
                        player.GetComponent<Player>().clout = player.GetComponent<Player>().clout+10;
                        sold = true;
                    }
                }
                break;
            case "Shoes":
                if (Input.GetKey(KeyCode.Space))
                {
                    if (player.GetComponent<Player>().shoesHeld > 0)//sell shoes if the player has any 
                    {
                        source.PlayOneShot(sellSound, 1);
                        player.GetComponent<Player>().shoesHeld--;
                        player.GetComponent<Player>().money = player.GetComponent<Player>().money + itemPriceCurr;
                        player.GetComponent<Player>().clout = player.GetComponent<Player>().clout + 10;
                        sold = true;
                    }
                }
                break;
            case "Hat":
                if (Input.GetKey(KeyCode.Space))
                {
                    if (player.GetComponent<Player>().hatsHeld > 0)//sell shoes if the player has any 
                    {
                        source.PlayOneShot(sellSound, 1);
                        player.GetComponent<Player>().hatsHeld--;
                        player.GetComponent<Player>().money = player.GetComponent<Player>().money + itemPriceCurr;
                        player.GetComponent<Player>().clout = player.GetComponent<Player>().clout + 10;
                        sold = true;
                    }
                }
                break;
        }
    }
}
