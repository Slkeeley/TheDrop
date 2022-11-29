using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed=15;//how fast does the projectile move
    public float activeTime=4;//how long until the projectile disappears from the scene
    Transform playerPos;//where is the player located at the time of the throw?
    Vector3 target;//where is the projectile being thrown
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //make sure the projectile is attached to its rigidbodies for collision detection. 
    }

    void Start()
    {
        playerPos = GameObject.Find("Player").transform;//find the object named player
        target = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);//the target is the player's current position at time of instantiations
        StartCoroutine(destroyProj());//begin destroying object
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);//go to the target 
        if (transform.position == target)//if projectile reaches target tell it to fall to the ground
        {
            rb.useGravity = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")//if player is hit tell player to be damaged
        {
          //  Debug.Log("Collided With Player");
            other.GetComponent<Player>().takeProjDamage();
            Destroy(this.gameObject);
        }
        Debug.Log("Destroying Enemy Proj");
        Destroy(this.gameObject); //if projectile collided with anything that isn't the player tell it to be destroyed
    }

    IEnumerator destroyProj()
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(this.gameObject);
    }
}
