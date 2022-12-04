using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : BasicEnemy
{
    

    void Update()//check if the enemy is dead and allow it to move around the map
    {
        if (!dead)//if the enemy is not dead move around
        {
            if (health <= 0)
            {
                dead = true;
                agent.SetDestination(transform.position);
                Die();
            }


            enemyInAggro = Physics.CheckSphere(transform.position, aggroRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            Debug.Log(enemyInAttackRange);
            if (!enemyInAggro && !enemyInAttackRange) patrol();
            if (enemyInAggro && !enemyInAttackRange) chasePlayer();
            if (enemyInAggro && enemyInAttackRange) attackPlayer();
        }

        if (dead)
        {
            enemyInAggro = false;
            enemyInAttackRange = false;
        }
    }

    void attackPlayer()//look at the player and punch them if it isnt on cooldown
    {
        animationInput(0);
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
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
        am.SetBool("Right", true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange+.25f))
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

 
  
}
