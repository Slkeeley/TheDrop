using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    public GameObject projectile;
    public bool playerInLOS = false; 
    public RangedEnemy()
    {
        
    }

    private void FixedUpdate()
    {
        checkLOS();
        if (playerInLOS)
        {
            if (enemyInAggro && enemyInAttackRange) attackPlayer();
        }
    }

    void attackPlayer()
    {
        Debug.Log("attacking player");
        transform.LookAt(player);
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            throwItem(); 
            StartCoroutine(attackCoolDown());
        }
    }

    void throwItem()
    {
        GameObject.Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z+0.75f), Quaternion.identity);
        Debug.Log("Object Instantiated");
    }

    void checkLOS()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.GetComponent<Player>())
            {
                playerInLOS = true; 
            }
            else
            {
                playerInLOS = false;
            }
        }
    }
}
