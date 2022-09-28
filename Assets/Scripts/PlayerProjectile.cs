using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 15;//how fast does the projectile move
    public float activeTime = 4;//how long until the projectile disappears from the scene
    bool active = true;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //make sure the projectile is attached to its rigidbodies for collision detection. 
    }

    void Start()
    { 
        StartCoroutine(stopProj());//begin destroying object
        rb.AddForce(transform.forward * 1000);
        checkGround();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")//if player is hit tell player to be damaged
        {
            other.GetComponent<Player>().hasBrick = true; 
            Destroy(this.gameObject);
        }
        Debug.Log("collided");
    }

    IEnumerator stopProj()
    {
        yield return new WaitForSeconds(activeTime);
        active = false; 
    }


    bool checkGround()
    {
        RaycastHit hit;
        Debug.Log("checking ground");
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10f))
        {
            Debug.Log(hit.transform.name);
        }
        return true; 
    }
}
