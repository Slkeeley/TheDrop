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
    public float walkPointRange, aggroRange, attackRange;//
    public bool enemyInAggro, enemyInAttackRange;//tell the monster to chase after a seen enemy and attack one if in range  
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

            // if (fading) fadeAway();

       /*     if (dead && !fading)//destroy object once it has faded away
            {
                Destroy(this.gameObject);
            }
            */
            enemyInAggro = Physics.CheckSphere(transform.position, aggroRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!enemyInAggro && !enemyInAttackRange) patrol();
            if (enemyInAggro && !enemyInAttackRange) chasePlayer();
           // if (enemyInSight && enemyInAttackRange) attackPlayer();
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
    /*
    void attackPlayer()
    {
        Debug.Log("attacking player");
        transform.LookAt(player);
        agent.SetDestination(transform.position);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            //punch();
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
    */
   public IEnumerator attackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.5f);
        alreadyAttacked = false;

    }

    void Die()//falls to the ground and instantiates money
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
/*
    void fadeAway()
    {
        Color objColor = GetComponent<Renderer>().material.color;
        float fadeAmount = objColor.a - (0.5f * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
        GetComponent<Renderer>().material.color = objColor;

        if(objColor.a<=0)
        {
            fading = false;
        }
    }
      */ 
      
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
