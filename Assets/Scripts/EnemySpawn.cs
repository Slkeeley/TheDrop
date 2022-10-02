using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject controller;
    bool onCooldown = false;
    public float spawnCooldown; 
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameWatcher").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!onCooldown && controller.GetComponent<GameControl>().enemiesInPlay < controller.GetComponent<GameControl>().maxEnenmies)
        {
            spawnEnemy();
        }
    }

    void spawnEnemy()
    {
        GameObject.Instantiate(enemy);
        controller.GetComponent<GameControl>().enemiesInPlay++;
        onCooldown = true;
        StartCoroutine(cooldown());
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        onCooldown = false; 
    }
}
