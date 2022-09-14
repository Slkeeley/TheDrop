using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{

    public int cashValue; 
    // Start is called before the first frame update
    void Start()
    {
        //could put randomization of the cash value here
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Collided with Player");
            other.GetComponent<Player>().money = other.GetComponent<Player>().money + cashValue;//increase the players money pool by however much this pickup is worth
            Destroy(this.gameObject);
        }
    }

}
