using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Controller Variables")]//variables for player movement
    public CharacterController controller;
    public Transform cam;
    public Animator am; 
    public float turnSmoothTime = 0.5f;
    float turnSmoothVelocity;
    public float movementSpeed = 15;
    float defaultSpeed = 15;

    [Header("Gameplay Variables")]//variables for player-game interaction
    public float money=0;
    public float clout = 100;
    public float MaxHealth = 100;
    public bool canBeDamaged = true;

    [Header("Inventory")]//how much of each item does the player own
    public int sweatersHeld;
    public int shoesHeld;
    public int hatsHeld;

    [Header("Attacks")]//data for player attacks, cooldown, models, 
    bool rightArmNext = true;//punch attacks
    bool canPunch = true; 
    public GameObject punchHitbox; 
    //kick attack
    bool canKick = true;//kick attack
    public GameObject Leg;
    public bool hasCrowbar = false;//crowbar attack
    bool crowbarOnCooldown = false; 
    public GameObject Crowbar;
    public float crowBarCooldown;
    public GameObject brick;//brick attack
    public Transform brickThrowLocation;//position that the brick should spawn in
    public bool hasBrick = false;
    bool canBlock = true; 

    [Header("Attack Statuses")]
    public bool isPunching;
    public bool isKicking;
    public bool isBlocking=false;
    bool spinning = false;


    [Header("UI Elements")]//data for player UI s
    public GameObject moneyEffect;

    private void Awake()
    {

        cam = GameObject.Find("MainCamera").transform;
        am = GetComponent<Animator>();
    }

    void Start()//put the players weapons away and make sure that the UI reflects default values
    {
        defaultSpeed = movementSpeed;
        punchHitbox.SetActive(false);
        Leg.SetActive(false);
    }


    void Update()
    {
        if (clout <= 0) SceneManager.LoadScene("DeathScreenTemp");
        Move();//always check if the player is moving
        attackInputs();//always check if the player is trying to attack; 
        if (spinning) transform.Rotate(0f, 2.8f, 0f);//rotate the player if they are doing the spinning crowbar attack
    }

    private void FixedUpdate()
    {
        readAttacks();
        if (transform.position.y != 1.0f)
        {
            Vector3 groundCheck = new Vector3(transform.position.x, 0f, transform.position.z);
            transform.position = groundCheck;
        }
        if (movementSpeed == 0) am.SetBool("Moving", false);

    }

    void Move()//movement method for a 3D space
    {
        //Get Input Axes 
        float vertAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

       //detect what direction the player is trying to move
        Vector3 direction = new Vector3(horizontalAxis, 0f, vertAxis).normalized;

        if (direction.magnitude >= 0.01f)
        {
            //rotate the player in the direction they are trying to move 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
           // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            am.SetBool("Moving", true);
            //move the player with speed independent of frame rate
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir * (movementSpeed * 3) * Time.deltaTime);
                am.SetBool("Running", true);
                am.SetBool("Walking", false);
            }
            else
            {
                controller.Move(moveDir * movementSpeed * Time.deltaTime);
                am.SetBool("Walking", true);
                am.SetBool("Running", false);
            }
 
            
        }
        else
        {
            am.SetBool("Moving", false);
            am.SetBool("Running", false);
            am.SetBool("Walking", false);
        }
    }

    void attackInputs()//attack methods 
    {
        if (Input.GetButton("Fire1"))//left click is for punch
        {
            if (canPunch)//check if the punch attack is off cooldown
            {
                canPunch = false;
                Punch();
            }
        }


        if (Input.GetButton("Fire2"))//right click is for kick
        {
            if (canKick)//check if the punch attack is off cooldown
            {

                canKick = false;
                Kick();
            }
        }

        if (Input.GetKey(KeyCode.Q))//Q is to use the crowbar attack if the player has one
        {
            if (hasCrowbar && !crowbarOnCooldown)
            {
                crowBarAttack();
            }
        }

        if(Input.GetKey(KeyCode.E))
        {
            if (hasBrick) throwBrick(); 
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Debug.Log("mouse 2");
            if(canBlock)
            {
                canBlock = false;
                block();
            }
        }
    }
 
    void readAttacks()//script for "animation" Canceling player attacks 
    {
        if(isPunching)//punch attack
        {
            isKicking = false;
            isBlocking = false;
            Leg.SetActive(false);
        }

        if(isKicking)
        {
            isPunching = false;
            isBlocking = false;
            punchHitbox.SetActive(false);
        }

        if(isBlocking)
        {
            isPunching = false;
            isKicking = false;
            punchHitbox.SetActive(false);
            Leg.SetActive(false);
        }

        if(spinning)
        {
            isPunching = false;
            isKicking = false;
            isBlocking = false;
            punchHitbox.SetActive(false);
            Leg.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Car")
        {
            clout = clout - 20;
        }
    }


    //ATTACK METHODS
    void Punch()//punch attack alternates between left and right arms
    {
        if(rightArmNext)//punch with right arm 
        {

            rightArmNext = false;
            am.SetBool("Right", true);
            StartCoroutine(punchCoolDown());
        }
        else//punch with left arm
        {

            rightArmNext = true;
            am.SetBool("Left", true);
            StartCoroutine(punchCoolDown());
        }
    }
    IEnumerator punchCoolDown()//put the players fist away after the attack is over and allow the player to be able to punch again
    {

        yield return new WaitForSeconds(.25f);
        punchHitbox.SetActive(true);
        yield return new WaitForSeconds(.25f);
        punchHitbox.SetActive(false);
        yield return new WaitForSeconds(.25f);
        am.SetBool("Left", false);
        am.SetBool("Right", false);
        canPunch = true;
        isPunching = false;
       
    }

    void Kick()//kick attack uses right leg only, but brings in a hitbox in the same way
    {
      
        am.SetBool("Kicking",true);
        StartCoroutine(movementPause());
        StartCoroutine(kickCoolDown());
    }


    IEnumerator kickCoolDown()//slightly longer cooldown for kick attack since it is strongers
    {
        yield return new WaitForSeconds(.25f);
        isKicking = true;
        Leg.SetActive(true);
        yield return new WaitForSeconds(.25f);
        Leg.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        am.SetBool("Kicking", false);
        canKick = true;
        isKicking = false; 
      
    }

    void crowBarAttack()//brings out the crowbar model and has the character spin around hitting everythign around it for half a second
    {
        Crowbar.SetActive(true);
        spinning = true;
        movementSpeed = 0; 
        crowbarOnCooldown = true;
        StartCoroutine(crowbarAttackCooldown());
    }

    IEnumerator crowbarAttackCooldown()//cooldown for crowbar attack
    {
        yield return new WaitForSeconds(0.5f);//the crowbar attack ast for half a second, after that passes put everything away
        Crowbar.SetActive(false);
        spinning = false;
        movementSpeed = 10;
        yield return new WaitForSeconds(crowBarCooldown);//after the attack ends the cooldown begins
        crowbarOnCooldown = false; 
    }

    void throwBrick()//brick attack instantiates a rigidbody projectile that falls to the ground and needs to be picked up again. 
    {
        GameObject.Instantiate(brick, brickThrowLocation.position, this.transform.rotation);
        hasBrick = false; 
    }

    void block()
    {
        isBlocking = true;
        am.SetBool("Blocking", true);
        StartCoroutine(movementPause());
        StartCoroutine(blockCooldown());
    }

    IEnumerator blockCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        am.SetBool("Blocking", false);
        isBlocking = false;
        yield return new WaitForSeconds(1.0f);
        canBlock = true; 
    }

    //GAME INTERACTION 

    public void takePunch()//method for player to take damage from an enemy punching them 
    {
        clout = clout - 2;
        canBeDamaged = false;
        StartCoroutine(invincibility());
    }

    public void takeProjDamage()//method for the player to take damage from an enemy projectile
    {
        clout = clout - 5;
        canBeDamaged = false;
        StartCoroutine(invincibility());
    }

    IEnumerator invincibility()//short cooldown to make sure that the player doesn't take a ton of damage at once
    {
        yield return new WaitForSeconds(0.5f);
        canBeDamaged = true;
    }

    public void moneyUp()//instantiates the money effect 
    {
        GameObject.Instantiate(moneyEffect, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
    }


    IEnumerator movementPause()
    {
        movementSpeed = 0;
        am.SetBool("Moving", false);
        yield return new WaitForSeconds(.25f);
        movementSpeed = defaultSpeed;

    }

}
