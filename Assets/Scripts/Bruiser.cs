using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : BasicEnemy
{
    public GameObject fists;//show visually that the enemy is trying to attack


    void Start()
    {
        fists.SetActive(false);//make sure that the fists are put away on instantiation
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (enemyInAggro && enemyInAttackRange) attackPlayer();//attack is in child script as a unique behavior
    }

    void attackPlayer()//look at the player and punch them if it isnt on cooldown
    {
        transform.LookAt(player);
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            punch();
            StartCoroutine(showFists());
            StartCoroutine(attackCoolDown());
        }
    }

    void punch()//fire a raycast to determine if the player was hit by an enemies punch
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.transform.name);
            Player player = hit.transform.GetComponent<Player>();
            if (player != null)
            {
                if (player.canBeDamaged)//check if the player can be damaged so they do not take too much damage at once 
                {
                    player.takePunch();
                }
            }
        }
    }

    IEnumerator showFists()//show the fists for a short time to demonstrate that the enemy is trying to attack
    {
        fists.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fists.SetActive(false); 
    }
}
