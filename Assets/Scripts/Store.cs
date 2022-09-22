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
    public Slider storeOpen;
    public Slider storeClosed;

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

    [Header("Store Times")]
    public float openTime; 
    public float closeTime;

    private void Awake()
    {
        playerEntered = false;
        items.SetActive(false);
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        storeOpen.maxValue = openTime;
        storeClosed.maxValue = closeTime;
        StartCoroutine(waitToOpen());
    }

    // Update is called once per frame
    void Update()
    {
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
            storeOpen.value -= Time.deltaTime / openTime * openTime;
        }
        else
        {
            items.SetActive(false);
            storeClosed.value -= Time.deltaTime / closeTime * closeTime;
        }


        if(itemsLeft <=0)
        {
            open = false; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Collided with player");
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

     void Randomizer()//way to randomize prices of the items between drops
    {
        sweaterPriceCurr = Random.Range(sweaterPriceMin, sweaterPriceMax + 1);
        shoePriceCurr = Random.Range(shoePriceMin, shoePriceMax + 1);
        hatPriceCurr = Random.Range(hatPriceMin, hatPriceMax + 1);
        roundToTens();
        updatePrices();
        open = true;
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

    IEnumerator waitToBuy()
    {
        canBuy = false;
        itemsLeft--;
        yield return new WaitForSeconds(0.5f);
        canBuy = true;
    }

    IEnumerator waitToOpen()//store is closed 
    {
        open = false;
        storeClosed.value = closeTime;
        storeOpen.value = 0;
        yield return new WaitForSeconds(closeTime);
        open = true; 
        Randomizer();
        StartCoroutine(waitToClose());
    }

    IEnumerator waitToClose()
    {
  //    open = true;
        storeClosed.value = 0;
        storeOpen.value = openTime;
        yield return new WaitForSeconds(openTime);
        open = false; 
        StartCoroutine(waitToOpen());
    }
}
