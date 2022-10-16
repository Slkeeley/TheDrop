using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    public GameObject projectile;//ranged enemy gets a projectile prefab to throw at the player

    void Update()//overrules basic enemy update
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
            if (checkLOS()&&enemyInAggro && !enemyInAttackRange) chasePlayer();//check if the player is also in the line of sight before chasing or attacking them
            if (checkLOS()&&enemyInAggro && enemyInAttackRange) attackPlayer();
            if (!checkLOS()) patrol(); 
        }
    }


    void attackPlayer()//face the player and throw the weapon at them 
    {
        transform.LookAt(player);
        agent.SetDestination(transform.position);
         if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                throwItem();
                StartCoroutine(attackCoolDown());
            }
        
    }


    void throwItem()//instantiate the projectile weapon
    {
        GameObject.Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);
    }

    bool checkLOS()//fire a raycast to determine if there is an object between the player and this enemy, returns false if player is not hit
    {
        transform.LookAt(player);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if(hit.transform.GetComponent<Player>())
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
        return false; 
    }
}
