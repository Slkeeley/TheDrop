using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    public GameObject projectile;

    void Update()
    {
        if (!dead)//if the enemy is not dead move around
        {
            if (health <= 0)
            {
                fading = true;
                dead = true;
                Die();
            }


            enemyInAggro = Physics.CheckSphere(transform.position, aggroRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!enemyInAggro && !enemyInAttackRange) patrol();            
            if (checkLOS()&&enemyInAggro && !enemyInAttackRange) chasePlayer();
            if (checkLOS()&&enemyInAggro && enemyInAttackRange) attackPlayer();
            if (!checkLOS()) patrol(); 
        }
    }


    void attackPlayer()
    {
        Debug.Log("attempting to attack player");
        transform.LookAt(player);
        agent.SetDestination(transform.position);
         if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                throwItem();
                StartCoroutine(attackCoolDown());
            }
        
    }


    void chasePlayer()
    {
        Debug.Log("Chasing Player");
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    void throwItem()
    {
        Debug.Log("projectile insantiated");
        GameObject.Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);
    }

    bool checkLOS()
    {
        transform.LookAt(player);
        Debug.Log("checking los");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.GetComponent<Player>())
            {
                Debug.Log("sees player");
                return true; 
            }
            else
            {
                return false;
                Debug.Log("does not see player");
            }
        }
        return false; 
    }
}
