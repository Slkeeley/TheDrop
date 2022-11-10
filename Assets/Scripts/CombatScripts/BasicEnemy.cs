using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{

    [Header("AI Variables")]//variables for the enemy to be able to use the NavMesh
    public NavMeshAgent agent; //allows enemy to move
    public Transform player;//What enemy is the monster targetting
    public LayerMask whatIsGround;//What is legal for the enemy to walk on
    public LayerMask whatIsPlayer;
    public Vector3 walkPoint;//where will the enemy go to 
    bool walkPointSet;
    public float walkPointRange, aggroRange, attackRange; 
    public bool enemyInAggro, enemyInAttackRange;
   public bool dead = false;
    public GameObject knockbackLocation; 

    [Header("Object Variables")]//variables that allow the enemy to interact with the player
    public bool alreadyAttacked = false;
    public float attackDelay;
    public float health;
    public float maxHealth;
    public GameObject money;
    public int billsDropped;
    public bool isBlocking;
    bool canBeDamaged = true;

    [Header("Visuals")]//allows the enemy to fade out of the scene
    private Color alpha;
   public bool fading = false;
    MeshRenderer mr; 
    public Material skin; 
    public Material damagedMat;
    public GameObject bar;
    public Image healthBar;
    public Animator am;

    private void Awake()//find the player the enemy should be chasing and make sure that it is visible
    {
        player = GameObject.Find("Player").transform;//find the object named player ,
        am = GetComponent<Animator>();
        //   mr = GetComponent<MeshRenderer>();
       // mr.material = skin;
        alpha.a = 0;
        maxHealth = health;
        bar.SetActive(false);
    }

   
    void Update()//check if the enemy is dead and allow it to move around the map
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


            enemyInAggro = Physics.CheckSphere(transform.position, aggroRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!enemyInAggro && !enemyInAttackRange) patrol();
            if (enemyInAggro && !enemyInAttackRange) chasePlayer();
        }

        if(dead)
        {
            enemyInAggro = false;
            enemyInAttackRange = false; 
        }
    }


    private void OnTriggerEnter(Collider other)//check if the player is hitting this enemy
    {
        if (canBeDamaged)
        {
            if (other.tag == "PlayerPunch")
            {
                if (!isBlocking)
                {
                    health = health - 2;
                    transform.position = knockbackLocation.transform.position;
                    StartCoroutine(damaged());
                    
                }
                else return;
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
            if(other.tag=="brick"||other.tag=="Car")
            {
                health = 0;
            }
        }
    }


   public void patrol()//move to a predetermined point on the map
    {
        am.SetBool("Moving", true);
        am.SetBool("Running", false);
        am.SetBool("Walking", true);
        if (!walkPointSet) setWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        transform.LookAt(walkPoint);
        Vector3 distToPoint = transform.position - walkPoint;
        if(distToPoint.magnitude <1.0f)
        {
            walkPointSet = false;
        }

    }

    void setWalkPoint()//if there is no point to wak to select a random point
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    public void chasePlayer()//move towards the player if they are close enough
    {
        transform.LookAt(player);
        agent.SetDestination(player.position);
        am.SetBool("Moving", true);
        am.SetBool("Walking", false);
        am.SetBool("Running", true);
    }

   public IEnumerator attackCoolDown()//cooldown to make sure that the enemy isn't constantly attacking
    {
        yield return new WaitForSeconds(attackDelay);
        alreadyAttacked = false;

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

  public void Die()//falls to the ground and instantiates money
    {

        transform.rotation = Quaternion.Euler(90, 0, 0);
        am.SetInteger("States", 2);
        while (billsDropped > 0)
        {
            GameObject.Instantiate(money, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            billsDropped--;
        }
        GameControl.enemiesInPlay--;
        StartCoroutine(fadeOut()); 
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
