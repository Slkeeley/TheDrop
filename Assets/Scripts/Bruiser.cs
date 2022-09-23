using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : BasicEnemy
{
    public GameObject fists;
    // Start is called before the first frame update
    void Start()
    {
        fists.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (enemyInAggro && enemyInAttackRange) attackPlayer();
    }

    void attackPlayer()
    {
        Debug.Log("attacking player");
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

    void punch()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.transform.name);
            Player player = hit.transform.GetComponent<Player>();
            if (player != null)
            {
                player.takePunch();
            }
        }
    }

    IEnumerator showFists()
    {
        fists.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fists.SetActive(false); 
    }
}
