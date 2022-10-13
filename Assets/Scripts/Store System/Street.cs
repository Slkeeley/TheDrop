using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    GameObject gameUI;

    private void Start()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>().gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Collided");
            gameUI.GetComponent<GameUI>().currLocationText.text = this.name;
        }
    }
}
