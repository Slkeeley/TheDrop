using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseRandomizer : MonoBehaviour
{
    public GameObject[] houses;
    public bool buyersActive;
    public int housesBuying;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitToOpen());
    }

    // Update is called once per frame
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


    void chooseHouses()
    {
        for (int i = 0; i < housesBuying; i++)
        {
            int houseSelected = Random.Range(0, houses.Length);

            if (houses[houseSelected].GetComponent<BuyerHouse>().open == false)//if the house was not already selected open it up 
            {
                houses[houseSelected].GetComponent<BuyerHouse>().openHouse();
            }
            else//if the house was already selected keep iterating
            {
                i--;
            }
         }
    }


    IEnumerator waitToOpen()//store is closed 
    {
        buyersActive= false;
        yield return new WaitForSeconds(10);
        buyersActive = true;
        chooseHouses(); 
        StartCoroutine(waitToClose());
    }

    IEnumerator waitToClose()
    {
        yield return new WaitForSeconds(20);
        buyersActive = false;
        StartCoroutine(waitToOpen());
    }
}
