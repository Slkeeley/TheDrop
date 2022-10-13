using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseRandomizer : MonoBehaviour
{
    public GameObject[] houses;//how many houses are in the level
    public GameObject phone;
    public GameObject GameUI;
    public bool buyersActive;//are the houses currently buying itemss
    public int housesBuying;//how many houses are offering to buy items at a time
    public float timeClosed; 
    public float timeOpen; 

    void Start()
    {
        StartCoroutine(waitToOpen());//begin by waiting to open the houses
    }

    void Update()
    {
        if(!buyersActive)//turn off all houses if buyers are not active
        {
            foreach (GameObject i in houses)
            {
                i.GetComponent<BuyerHouse>().open = false; 
            }
        }
    }


    void chooseHouses()//choose what houses out of the many are trying to buy items at a time
    {
        for (int i = 0; i < housesBuying; i++)
        {
            int houseSelected = Random.Range(0, houses.Length);

            if (houses[houseSelected].GetComponent<BuyerHouse>().open == false)//if the house was not already selected open it up 
            {
                houses[houseSelected].GetComponent<BuyerHouse>().openHouse();
                phone.GetComponent<Phone>().messages[i].text = "Looking to buy a " + houses[houseSelected].GetComponent<BuyerHouse>().itemSold + " for $" + houses[houseSelected].GetComponent<BuyerHouse>().itemPriceCurr + " on " + houses[houseSelected].GetComponent<BuyerHouse>().streetLocation.name;
                GameUI.GetComponent<GameUI>().msgNotifications++;
            }
            else//if the house was already selected keep iterating
            {
                i--;
            }
         }
    }


    IEnumerator waitToOpen()//house is closed waiting to open
    {
        buyersActive= false;
        yield return new WaitForSeconds(timeClosed);
        buyersActive = true;
        chooseHouses(); 
        StartCoroutine(waitToClose());
    }

    IEnumerator waitToClose()//houses are active and waiting to close back down
    {
        yield return new WaitForSeconds(timeOpen);
        buyersActive = false;
        StartCoroutine(waitToOpen());
    }
}
