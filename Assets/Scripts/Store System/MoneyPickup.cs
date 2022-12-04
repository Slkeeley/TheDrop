using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{

    public int cashValue; //how much is this bill worth
    public Transform player;
    public bool inRadius = false;
    public float grabRadius; 

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); //insantiate in the position that the parent enemy died in        
        player = GameObject.Find("Player").transform;
    }


    private void Update()
    { 
        radiusCheck();
        if (inRadius)
        {
            Debug.Log("Trying to move towards the player ");
           transform.position= Vector3.MoveTowards(transform.position, player.position, Time.deltaTime*10f);
        }
    }


    private void OnTriggerEnter(Collider other)//once the bill is touched by a player it is collected
    {
        if(other.tag=="Player")
        {
            other.GetComponent<Player>().money = other.GetComponent<Player>().money + cashValue;//increase the players money pool by however much this pickup is worth
            other.GetComponent<Player>().moneyUp();
            Destroy(this.gameObject);
        }
    }

    void radiusCheck()
    {
        RaycastHit hit;

        Vector3 p1 = transform.position + player.transform.position;
        float distanceToObstacle = 0;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (Physics.SphereCast(p1, grabRadius, transform.forward, out hit, 10))
        {
            distanceToObstacle = hit.distance;
            inRadius = true;
        }
    }
}
