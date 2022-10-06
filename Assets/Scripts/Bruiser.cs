using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : BasicEnemy
{
    public GameObject fists;//show visually that the enemy is trying to attack
    public GameObject blockPos;//show visually that the enemy is trying to attack
    public bool defensive = false;
    public bool blockOnCooldown = false;
    bool fight;

    void Start()
    {
        fists.SetActive(false);//make sure that the fists are put away on instantiation
        blockPos.SetActive(false);//make sure that the enemy does not show they are blocking at the start of the game. 
        fightOrFlight();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (health <= maxHealth / 2) defensive = true;//if the enemy has half of its health or less begin to act defensiely


        if (!defensive)//if the enemy is not low on health than behave normally
        {
            if (enemyInAggro && enemyInAttackRange) attackPlayer();//attack is in child script as a unique behavior
        }
        else
        {
            if (!enemyInAggro) defensive = false;//if the player chooses to run away then enemies no longer should continue to block
            if (!blockOnCooldown)//if they are not blocking then block
            {
                blockOnCooldown = true;
                StartCoroutine(blockCooldown());
            }
            else
            {
                walkPointRange = 10;
                chosenAction();//do other action if the enemies block is on cooldown
            }
        }


    }

    void fightOrFlight()//behavior chosen at the beggining of the scene whether enemies flee or keep fighting when in defense mode
    {
        int behavior = Random.Range(0, 2);
        switch (behavior)
        {
            case 0:
                Debug.Log("has chosen to fight");
                fight = true; 
                break;
            case 1:
                fight = false;
                Debug.Log("has chosen to flee");
                break;
            default:
                break;
        }
    }

    void chosenAction()
    {
        if (fight) attackPlayer();
        else patrol();
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


    void block()//show visually that this enemy is blocking
    {
        blockPos.SetActive(true);
        transform.LookAt(player);
        agent.SetDestination(transform.position);
        isBlocking = true;
    }

    void blockDown()
    {
        blockPos.SetActive(false);
        isBlocking = false;
    }


    IEnumerator blockCooldown()//cooldown for when enemy is/isnt blocking
    {
        block();
        yield return new WaitForSeconds(3);
        blockDown();
        yield return new WaitForSeconds(3);
        blockOnCooldown = false;
    }
}
