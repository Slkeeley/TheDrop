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
    public float walkPointRange, aggroRange, attackRange; 
    public bool enemyInAggro, enemyInAttackRange;
   public bool dead = false;

    [Header("Object Variables")]
    public bool alreadyAttacked = false;
    public float attackDelay;
    public float health;
    public Image healthBar;
    public GameObject money;
    public int billsDropped; 

    [Header("Visuals")]
    private Color alpha;
   public bool fading = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;//find the object named player ,
        agent = GetComponent<NavMeshAgent>();
        alpha = GetComponent<MeshRenderer>().material.color;
        alpha.a = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
      
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
                Die();
            }


            enemyInAggro = Physics.CheckSphere(transform.position, aggroRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!enemyInAggro && !enemyInAttackRange) patrol();
            if (enemyInAggro && !enemyInAttackRange) chasePlayer();
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag=="PlayerPunch")
        {
            Debug.Log("punched");
            health = health - 2;
        }
        if (other.tag == "PlayerKick")
        {
            health = health - 5;
        }
    }


   public void patrol()
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

  public  void chasePlayer()
    {
        Debug.Log("Chasing Player");
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

   public IEnumerator attackCoolDown()
    {
        Debug.Log("Beginning attack delay");
        yield return new WaitForSeconds(attackDelay);
        alreadyAttacked = false;

    }

  public void Die()//falls to the ground and instantiates money
    {
        Vector3 onGround = new Vector3(90, 0, 0);
        transform.eulerAngles = onGround;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        while (billsDropped > 0)
        {
            GameObject.Instantiate(money, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            billsDropped--;
        }
        StartCoroutine(fadeOut()); 
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
