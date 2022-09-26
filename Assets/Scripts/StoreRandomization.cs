using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRandomization : MonoBehaviour
{
    public GameObject[] stores;
    public bool dropsActive=false;
    public int totalDrops;
    float timeOpen; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitToOpen());
    }

    // Update is called once per frame
    void Update()
    {
        if (!dropsActive)//turn off all houses if buyers are not active
        {
            foreach (GameObject i in stores)
            {
                i.GetComponent<Store>().open = false;
            }
        }
    }


    void chooseStore()
    {
        for (int i = 0; i < totalDrops; i++)
        {
            int storeSelected = Random.Range(0, stores.Length);

            if (stores[storeSelected].GetComponent<Store>().open == false)//if the house was not already selected open it up 
            {
                stores[storeSelected].GetComponent<Store>().beginOpening();
            }
            else//if the house was already selected keep iterating
            {
                i--;
            }
        }
    }


    IEnumerator waitToOpen()//store is closed 
    {
        dropsActive = false;
        yield return new WaitForSeconds(10f);
        dropsActive = true;
        chooseStore();
        StartCoroutine(waitToClose());
    }

    IEnumerator waitToClose()
    {
        yield return new WaitForSeconds(20);
        dropsActive = false;
        StartCoroutine(waitToOpen());
    }
}
