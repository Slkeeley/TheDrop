using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject controller;
    bool onCooldown = false;
    public float spawnCooldown; 


    void Start()
    {
        controller = GameObject.Find("GameWatcher").gameObject;//find the object with our game controller script
    }

    void Update()
    {
        if(!onCooldown && GameControl.enemiesInPlay < controller.GetComponent<GameControl>().maxEnenmies)//if more enemies can still spawn, bring in a new enemy
        {
            spawnEnemy();
        }
    }

    void spawnEnemy()//add a new enemy to the map from whatever is slotted into this object
    {
        GameObject.Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        GameControl.enemiesInPlay++;
        onCooldown = true;
        StartCoroutine(cooldown());
    }

    IEnumerator cooldown()//make sure that enemies are not constantly spawning 
    {
        yield return new WaitForSeconds(spawnCooldown);
        onCooldown = false; 
    }
}
