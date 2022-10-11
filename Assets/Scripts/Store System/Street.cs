using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Collided");
            other.GetComponent<Player>().currLocationText.text = this.name;
        }
    }
}
