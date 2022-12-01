using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuyerEnemy : MonoBehaviour
{
    [Header("AI Variables")]//variables for the enemy to be able to use the NavMesh
    public NavMeshAgent agent; //allows enemy to move
    public Transform player;//What enemy is the monster targetting
    public LayerMask whatIsGround;//What is legal for the enemy to walk on
    public LayerMask whatIsPlayer;
    public Vector3 walkPoint;//where will the enemy go to 
    bool walkPointSet;
    public float walkPointRange, attackRange;
    public bool enemyInAttackRange;
    public bool dead = false;
    public GameObject knockbackLocation;
    public bool spinning = false; 

    [Header("Object Variables")]//variables that allow the enemy to interact with the player
    public bool alreadyAttacked = false;
    public float health;
    public float maxHealth;
    public GameObject money;
    public int billsDropped;
    bool canBeDamaged = true;
    public bool storeFound = false;
    public bool doneShopping = false;
    public GameObject targetStore;
    public int itemsBought;

    [Header("Visuals")]//allows the enemy to fade out of the scene
    private Color alpha;
    public bool fading = false;
    MeshRenderer mr;
    public Material skin;
    public Material damagedMat;
    public GameObject bar;
    public Image healthBar;
    public Animator am;
    public GameObject normalBody;
    public GameObject attackPos;
    public GameObject explosionEffect;

    private void Awake()//find the player the enemy should be chasing and make sure that it is visible
    {
        player = GameObject.Find("Player").transform;//find the object named player ,
        am = GetComponent<Animator>();
        //   mr = GetComponent<MeshRenderer>();
        // mr.material = skin;
        alpha.a = 0;
        maxHealth = health;
        bar.SetActive(false);
        normalBody.SetActive(true);
        attackPos.SetActive(false);
        animationInput(0);
    }


    // Update is called once per frame
    void Update()
    {

        if (!dead)//if the enemy is not dead move around
        {
            if (health <= 0)
            {
                fading = true;
                dead = true;
                agent.speed = 0;
                Die();
            }

            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


            if (!storeFound&& !enemyInAttackRange)//if there is a store open, then go to a store to buy items, if not patrol 
            {
                if(findStore())
                {
                    storeFound = true;
                }
                else
                {
                    storeFound = false; 
                }
            }

            if (storeFound&&!doneShopping) goToStore();
            else patrol(); 

            if(enemyInAttackRange)
            {
                attackPlayer();
            }
        }

        if (dead)
        {
            enemyInAttackRange = false;
        }

        if (spinning) transform.Rotate(0f, 100f, 0f);//rotate the player if they are doing the spinning crowbar attack
        if (itemsBought >= 5) doneShopping = true;

    }


    public void animationInput(int state)//use this to animate the enemy 
    {
        switch (state)
        {
            case 2:
                am.SetInteger("States", 2); //running
                break;
            case 5:
                am.SetInteger("States", 5); //walking
                break;
            case 0:
                am.SetInteger("States", 0); //idle
                break;
        }
    }

        private void OnTriggerEnter(Collider other)//check if the player is hitting this enemy
    {
        if (canBeDamaged)
        {
            if (other.tag == "PlayerPunch")
            {
                health = health - 2;
                transform.position = knockbackLocation.transform.position;
                StartCoroutine(damaged());
            }
            if (other.tag == "PlayerKick")
            {
                health = health - 5;
                transform.position = knockbackLocation.transform.position;
                StartCoroutine(damaged());
            }
            if (other.tag == "Crowbar")
            {
                health = health - 10;
            }
            if (other.tag == "brick" || other.tag == "Car")
            {
                health = 0;
            }
        }
    }
        IEnumerator damaged()
        {
            canBeDamaged = false;
            //    mr.material = damagedMat;
            bar.SetActive(true);
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
            yield return new WaitForSeconds(0.25f);
            //   mr.material = skin;
            yield return new WaitForSeconds(0.25f);
            bar.SetActive(false);
            canBeDamaged = true;
        }
    
    bool findStore()
    {
        Store[] stores = GameObject.FindObjectsOfType<Store>();
        for (int i = 0; i < stores.Length; i++)
        {
            if(stores[i].open)
            {
                targetStore = stores[i].gameObject;
                return true;
            }
        }
        return false; 
    }

    void goToStore()
    {
        agent.SetDestination(targetStore.GetComponent<Store>().front.transform.position);
        Vector3 distToPoint = transform.position - targetStore.GetComponent<Store>().front.transform.position;
        if (distToPoint.magnitude < 1.1f)
        {
            animationInput(0);
        }
        else
        {
            animationInput(2);
        }

    }

    public void patrol()//move to a predetermined point on the map
    {
        Debug.Log("Patrolling");
        if (!walkPointSet) setWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        transform.LookAt(walkPoint);
        animationInput(5);
        Vector3 distToPoint = transform.position - walkPoint;
        if (distToPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
            animationInput(0);
        }

    }
    void setWalkPoint()//if there is no point to wak to select a random point
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    void attackPlayer()
    {
        animationInput(0);
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            normalBody.SetActive(false);
            attackPos.SetActive(true);
            StartCoroutine(crowBarAttack());
        }
    }
    

    void Die()
    {
        int givenItem = Random.Range(1, 4);
        switch(givenItem)
        {
            case 1:
                player.GetComponent<Player>().sweatersHeld++;
                player.GetComponent<Player>().sweaterAcquired();
                break;
            case 2:
                player.GetComponent<Player>().hatsHeld++;
                player.GetComponent<Player>().hatAcquired();
                break;
            case 3:
                player.GetComponent<Player>().shoesHeld++;
                player.GetComponent<Player>().shoeAcquired();
                break;
        }
        GameObject.Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        while (billsDropped > 0)
        {
            GameObject.Instantiate(money, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            billsDropped--;
        }
        GameControl.enemiesInPlay--;
        Destroy(this.gameObject);
    }


    IEnumerator crowBarAttack()
    {
        alreadyAttacked = true;
        spinning = true;
        yield return new WaitForSeconds(1.5f);
        spinning = false;
        attackPos.SetActive(false);
        normalBody.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        alreadyAttacked = false; 

    }
}
