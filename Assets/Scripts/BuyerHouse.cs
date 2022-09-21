using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyerHouse : MonoBehaviour
{
    public bool playerEntered = false;
  public  bool open = true;
    bool sold = false;//boolean to make sure that the player can only buy one item at a time
    public GameObject player;
    [Header("Store UI")]
    public GameObject canvas;
    public TMP_Text itemText;
    public TMP_Text priceText;

    [Header("Selling")]
    public int itemPriceMin = 50;
    public int itemPriceMax = 150;
    public int itemPriceCurr;
    public string itemSold; 


    [Header("Open Times")]
    public float openTime;
    public float closeTime;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        randomizeItems();
        updatePrices();
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
       if(!open || sold)
        {
            canvas.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEntered = false;
        }
    }

    public void openHouse()
    {
        open = true;
        sold = false;
        randomizeItems();
        updatePrices(); 
    }


    void updatePrices()//make sure that the display text shows the correct price of the item. 
    {
        itemText.text = "Looking to buy a " + itemSold;
        priceText.text = "Press SPACE to sell for $" + itemPriceCurr.ToString();

    }

   void randomizeItems()
    {
        int itemNumber = Random.Range(1, 4);
        switch(itemNumber)
        {
            case 1:
                itemSold = "Sweater";
                break;
            case 2:
                itemSold = "Shoes";
                break;
            case 3:
                itemSold = "Hat";
                break;
            default:
                itemSold = "Sweater";
                break;
        }
        itemPriceCurr = Random.Range(itemPriceMin, itemPriceMax+1);
    }

    void playerSell()
    {
        switch(itemSold)
        {
            case "Sweater":
                if (Input.GetKey(KeyCode.Space))
                {
                    if (player.GetComponent<Player>().sweatersHeld>0)//sell sweaters if the player has any 
                    {
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
