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
    public GameObject fists;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;//find the object named player ,
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        fists.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) Die();

        enemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!enemyInSight && !enemyInAttackRange) patrol();   
        if (enemyInSight && !enemyInAttackRange) chasePlayer();   
        if (enemyInSight && enemyInAttackRange) attackPlayer();   
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag=="PlayerPunch"&& !isBlocking)
        {
            health = health - 2;
        }
        if (other.tag == "PlayerKick")
        {
            health = health - 5;
        }
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
        Debug.Log("Chasing Player");
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    void attackPlayer()
    {
        Debug.Log("attacking player");
        transform.LookAt(player);
        agent.SetDestination(transform.position);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            fists.SetActive(true);
            punch();
            StartCoroutine(attackCoolDown());
        }
    }

    void punch()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            Debug.Log(hit.transform.name);
            Player player = hit.transform.GetComponent<Player>();
            if(player!=null)
            {
                player.takePunch();
            }
        }
    }

    IEnumerator attackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        fists.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        alreadyAttacked = false;

    }

    void Die()
    {
        //death function
    }
}
