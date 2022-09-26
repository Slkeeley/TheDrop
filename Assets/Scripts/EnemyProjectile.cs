using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed=15;
    public float activeTime=4;
    Transform playerPos;
    Vector3 target;
    Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void Start()
    {
        playerPos = GameObject.Find("Player").transform;//find the object named player
        target = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);
        StartCoroutine(destroyProj());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == target)
        {
            rb.useGravity = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<Player>().takeProjDamage();
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }

    IEnumerator destroyProj()
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(this.gameObject);
    }
}
