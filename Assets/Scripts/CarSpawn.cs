using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
    public GameObject[] cars;
    bool onCooldown = false;
    public float spawnCooldown;


    void Update()
    {
        if (!onCooldown)//if more enemies can still spawn, bring in a new enemy
        {
            spawnEnemy();
        }
    }

    void spawnEnemy()//add a new enemy to the map from whatever is slotted into this object
    {
        int carToSpawn = Random.Range(0, cars.Length);
        GameObject.Instantiate(cars[carToSpawn], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        onCooldown = true;
        StartCoroutine(cooldown());
    }

    IEnumerator cooldown()//make sure that enemies are not constantly spawning 
    {
        yield return new WaitForSeconds(spawnCooldown);
        onCooldown = false;
    }
}
