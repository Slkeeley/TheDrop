using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Store : MonoBehaviour
{
    public bool playerEntered = false;
    bool canBuy= true;//boolean to make sure that the player can only buy one item at a time
    public bool open; 
    public GameObject player;
    [Header("Store UI")]
    public GameObject items;
    public TMP_Text sweaterText;
    public TMP_Text shoesText;
    public TMP_Text hatText;
    public GameObject waitingCube; 
    public GameObject openCube; 

    [Header("Store Prices")]
    public int sweaterPriceMin = 20; 
    public int sweaterPriceMax = 50;
    public int shoePriceMin = 40;
    public int shoePriceMax = 80;
    public int hatPriceMin = 10; 
    public int hatPriceMax = 30;
    private int sweaterPriceCurr;
    private int shoePriceCurr;
    private int hatPriceCurr;
    public int itemsLeft;
    public int itemsMax;

    [Header("Store Times")]
    public float openTime; 
    public float closeTime;
    public float waitingTime; 

    private void Awake()
    {
        playerEntered = false;
        items.SetActive(false);
        openCube.SetActive(false);
        waitingCube.SetActive(false);
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Debug.Log(open);
        if (open)
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
        else
        {
            items.SetActive(false);
            openCube.SetActive(false);
        }


        if(itemsLeft <=0)
        {
            open = false; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") playerEntered = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEntered = false;
        }
    }

    public void Randomizer()//way to randomize prices of the items between drops
    {
        sweaterPriceCurr = Random.Range(sweaterPriceMin, sweaterPriceMax + 1);
        shoePriceCurr = Random.Range(shoePriceMin, shoePriceMax + 1);
        hatPriceCurr = Random.Range(hatPriceMin, hatPriceMax + 1);
        roundToTens();
        updatePrices();
    }

    void roundToTens()
    {
        sweaterPriceCurr = (sweaterPriceCurr / 10) * 10;
        shoePriceCurr = (shoePriceCurr / 10) * 10;
        hatPriceCurr = (hatPriceCurr / 10) * 10;
    }

    void updatePrices()//make sure that the display text shows the correct price of the item. 
    {
        sweaterText.text = "Press 1 to buy sweater for $" + sweaterPriceCurr.ToString();
        shoesText.text = "Press 2 to buy shoes for $" + shoePriceCurr.ToString();
        hatText.text = "Press 3 to buy a hat for $" + hatPriceCurr.ToString();
    }

    void buyItems()
    {
        if (canBuy)
        {
            if (Input.GetKey(KeyCode.Alpha1))//player buys sweater
            {
                if (player.GetComponent<Player>().money >= sweaterPriceCurr)
                {
                    player.GetComponent<Player>().money = player.GetComponent<Player>().money - sweaterPriceCurr;
                    player.GetComponent<Player>().sweatersHeld++;
                    StartCoroutine(waitToBuy());
                }
            }

            if (Input.GetKey(KeyCode.Alpha2))//player buys shoess
            {
                if (player.GetComponent<Player>().money >= shoePriceCurr)
                {
                    player.GetComponent<Player>().money = player.GetComponent<Player>().money - shoePriceCurr;
                    player.GetComponent<Player>().shoesHeld++;
                    StartCoroutine(waitToBuy());
                }
            }

            if (Input.GetKey(KeyCode.Alpha3))//player buys a hat
            {
                if (player.GetComponent<Player>().money >= hatPriceCurr)
                {
                    player.GetComponent<Player>().money = player.GetComponent<Player>().money - hatPriceCurr;
                    player.GetComponent<Player>().hatsHeld++;
                    StartCoroutine(waitToBuy());
                }
            }
        }
    }

    public void beginOpening()
    {
        itemsLeft = itemsMax;
        Randomizer();
        StartCoroutine(waitToOpen());
    }

    IEnumerator waitToOpen()//store is closed 
    {
        Debug.Log("store is waiting to open");
        waitingCube.SetActive(true);
        yield return new WaitForSeconds(waitingTime);
        open = true; 
    }

    IEnumerator waitToBuy()
    {
        canBuy = false;
        itemsLeft--;
        yield return new WaitForSeconds(0.5f);
        canBuy = true;
    }

}
