using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 15;//how fast does the projectile move
    public float activeTime = 4; 
    public int throwForce; 
    bool active = true;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //make sure the projectile is attached to its rigidbodies for collision detection. 
    }

    void Start()
    {
        rb.AddRelativeForce(Vector3.forward * throwForce);
        StartCoroutine(stopProj());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")//if player is hit tell player to be damaged
        {
            other.GetComponent<Player>().hasBrick = true; 
            Destroy(this.gameObject);
        }

    }
    
    IEnumerator stopProj()
    {
        yield return new WaitForSeconds(activeTime);
        GetComponent<BoxCollider>().isTrigger = true; 
    }
}
