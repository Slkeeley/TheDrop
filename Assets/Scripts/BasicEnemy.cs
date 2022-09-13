using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{

    [Header("AI Variables")]
    public NavMeshAgent agent; //allows emu to move
    public Transform player;//What enemy is the monster targetting
    public LayerMask whatIsGround;//What is legal for the enemy to walk on
    public LayerMask whatIsPlayer;
    public Vector3 walkPoint;//where will the enemy go to 
    bool walkPointSet;
    public float walkPointRange, sightRange, attackRange;//
    public bool enemyInSight, enemyInAttackRange;//tell the monster to chase after a seen enemy and attack one if in range  

    [Header("Object Variables")]
    bool alreadyAttacked = false;
    public float attackDelay;
    public bool canBlock;//Booleans for later when the blocking mechanic is added
    public bool isBlocking;
    public float health;
    public Image healthBar;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;//find the object named player ,
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) Die();

        enemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!enemyInSight && !enemyInAttackRange) patrol();   
        if (!enemyInSight && enemyInAttackRange) chasePlayer();   
        if (enemyInSight && enemyInAttackRange) attackPlayer();   
    }

    void patrol()
    {
        if (!walkPointSet) setWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        Vector3 distToPoint = transform.position - walkPoint;
        if(distToPoint.magnitude <1.0f)
        {
            walkPointSet = false;
        }

    }

    void setWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    void chasePlayer()
    {
        //chase player
    }

    void attackPlayer()
    {
        //attack player
    }

    void Die()
    {
        //death function
    }
}
