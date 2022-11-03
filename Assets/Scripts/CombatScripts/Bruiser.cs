using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : BasicEnemy
{
    public bool defensive = false;
    public bool blockOnCooldown = false;
    bool fight;

    void Start()
    {
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
                fight = true; 
                break;
            case 1:
                fight = false;
                break;
            default:
                break;
        }
    }

    void chosenAction()
    {
        if (fight) attackPlayer();
        else
        {
            agent.speed = agent.speed * 1.5f;
            patrol();
        }
    }

    void attackPlayer()//look at the player and punch them if it isnt on cooldown
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        agent.SetDestination(transform.position);
        am.SetBool("Moving", false);
        am.SetBool("Running", false);
        am.SetBool("Walking", false);
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
        am.SetBool("Right", true);
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
        yield return new WaitForSeconds(0.5f);
        am.SetBool("Right", false);
    }


    void block()//show visually that this enemy is blocking
    {
        am.SetBool("Moving", false);
        am.SetBool("Blocking", true);
        transform.LookAt(player);
        agent.SetDestination(transform.position);
        isBlocking = true;
    }

    void blockDown()
    {
        am.SetBool("Moving", true);
        am.SetBool("Blocking", false);
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
