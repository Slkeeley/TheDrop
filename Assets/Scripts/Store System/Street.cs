using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    GameObject Phone;

    private void Start()
    {
        Phone = GameObject.FindObjectOfType<Phone>().gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Collided");
            Phone.GetComponent<Phone>().streetName.text = this.name;
        }
    }
}
