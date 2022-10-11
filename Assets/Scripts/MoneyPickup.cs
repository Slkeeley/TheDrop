using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{

    public int cashValue; //how much is this bill worth

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); //insantiate in the position that the parent enemy died in        
    }

    private void OnTriggerEnter(Collider other)//once the bill is touched by a player it is collected
    {
        if(other.tag=="Player")
        {
            Debug.Log("Collided with Player");
            other.GetComponent<Player>().money = other.GetComponent<Player>().money + cashValue;//increase the players money pool by however much this pickup is worth
            other.GetComponent<Player>().moneyUp();
            Destroy(this.gameObject);
        }
    }

}
