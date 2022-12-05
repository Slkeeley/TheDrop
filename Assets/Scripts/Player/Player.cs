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
    public float money = 0;
    public float clout = 100;
    public float MaxHealth = 100;
    public bool canBeDamaged = true;
    public bool dead = false;

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
    public GameObject playerBody;
    public float crowBarCooldown;
    public GameObject brick;//brick attack
    public Transform brickThrowLocation;//position that the brick should spawn in
    public bool hasBrick = false;
    bool canBlock = true;

    [Header("Attack Statuses")]
    public bool isPunching;
    public bool isKicking;
    public bool isBlocking = false;
    bool spinning = false;
    //COOLDOWN CHECks
    float timeLeftKickCD=0.0f; 
    float timeLeftCrowbarCD=0.0f; 
    float timeBlockCD=0.0f; 

    [Header("UI Elements")]//data for player UI s
    public GameObject moneyEffect;
    public GameObject sweaterEffect;
    public GameObject shoeEffect;
    public GameObject hatEffect;
    public GameObject effectLocation;

    [Header("Cooldown Bars")]
    public GameObject KickCoolDownImage;
    public Image KickCooldownBar;
    public GameObject CBCoolDownImage;
    public Image CBCooldownBar; 
    public GameObject BlockCoolDownImage;
    public Image BlockCooldownBar;

    [Header("Audio")]
    public AudioClip[] soundEffects;
    AudioSource source; 
    private void Awake()
    {
        source = GetComponent<AudioSource>(); 
        cam = GameObject.Find("MainCamera").transform;
        am = GetComponent<Animator>();
        playerBody.SetActive(true);
        Crowbar.SetActive(false);
        KickCoolDownImage.SetActive(false);
        CBCoolDownImage.SetActive(false);
        BlockCoolDownImage.SetActive(false);
    }

    void Start()//put the players weapons away and make sure that the UI reflects default values
    {
        defaultSpeed = movementSpeed;
        punchHitbox.SetActive(false);
        Leg.SetActive(false);
    }


    void Update()
    {
        if (clout <= 0)
        {
            dead = true;
            StartCoroutine(Die());
        }
        if (!dead)
        {
            Move();//always check if the player is moving
            attackInputs();//always check if the player is trying to attack; 
            if (spinning) transform.Rotate(0f, 8f, 0f);//rotate the player if they are doing the spinning crowbar attack
        }
    }

    private void FixedUpdate()
    {
        readAttacks();
        if (transform.position.y != 1.0f)
        {
            Vector3 groundCheck = new Vector3(transform.position.x, -.5f, transform.position.z);
            transform.position = groundCheck;
        }
        //KICK COOLDOWN CHECKS
        if(timeLeftKickCD >=2.0f)
        {
            canKick = true; 
        }
        else
        {
            timeLeftKickCD+= Time.deltaTime;
            timeLeftKickCD = Mathf.Clamp(timeLeftKickCD, 0.0f, 2.0f);
        }
        if(KickCoolDownImage.activeInHierarchy) KickCooldownBar.fillAmount = Mathf.Clamp(timeLeftKickCD / 2.0f, 0, 1f);
        //Crowbar COOLDOWN CHECKS
        if (timeLeftCrowbarCD>=crowBarCooldown)
        {
            crowbarOnCooldown = false; 
        }
        else
        {
            timeLeftCrowbarCD += Time.deltaTime;
            timeLeftCrowbarCD = Mathf.Clamp(timeLeftCrowbarCD, 0.0f, crowBarCooldown);
        }
        if (CBCoolDownImage.activeInHierarchy) CBCooldownBar.fillAmount = Mathf.Clamp(timeLeftCrowbarCD / crowBarCooldown, 0, 1f);
        //Block COOLDOWN CHECKS
        if (timeBlockCD >= 3.0f)
        {
            canBlock = true;
        }
        else
        {
            timeBlockCD+= Time.deltaTime;
            timeBlockCD= Mathf.Clamp(timeBlockCD, 0.0f, 3.0f);
        }
        if (BlockCoolDownImage.activeInHierarchy) BlockCooldownBar.fillAmount = Mathf.Clamp(timeBlockCD / 3.0f, 0, 1f);
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
            //move the player with speed independent of frame rate
            controller.Move(moveDir * movementSpeed * Time.deltaTime);
            animationInput(2);
        }
        else
        {
            animationInput(0);
        }

    }
    //USE THIS TO SWITCH ANIMATIONS 
    void animationInput(int state)
    {
        switch (state)
        {

            case 1:
                am.SetInteger("States", 1); //death
                break;
            case 2:
                am.SetInteger("States", 2); //running
                break;
            case 3:
                am.SetInteger("States", 3); //throwing
                break;
            case 4:
                am.SetInteger("States", 4); //blocking
                break;
            default:
                am.SetInteger("States", 0); //idle
                break;
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

        if (Input.GetKey(KeyCode.E))
        {
            if (hasBrick)
            {
                animationInput(0);
                StartCoroutine(throwBrick());
            }
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Debug.Log("mouse 2");
            if (canBlock)
            {
                canBlock = false;
                block();
            }
        }
    }

    void readAttacks()//script for "animation" Canceling player attacks 
    {
        if (isPunching)//punch attack
        {
            isKicking = false;
            isBlocking = false;
            Leg.SetActive(false);
        }

        if (isKicking)
        {
            isPunching = false;
            isBlocking = false;
            punchHitbox.SetActive(false);
        }

        if (isBlocking)
        {
            isPunching = false;
            isKicking = false;
            punchHitbox.SetActive(false);
            Leg.SetActive(false);
        }

        if (spinning)
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
        if (other.tag == "Car")
        {
            source.PlayOneShot(soundEffects[1], 2);
            clout = clout - 20;
        }
        if (other.tag == "EnemyCrowbar")
        {
            source.PlayOneShot(soundEffects[1], 2);
            clout = clout - 10;
        }
    }


    //ATTACK METHODS
    void Punch()//punch attack alternates between left and right arms
    {
        if (rightArmNext)//punch with right arm 
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
        source.PlayOneShot(soundEffects[0], 1);
        punchHitbox.SetActive(true);
        movementSpeed = 0;
        yield return new WaitForSeconds(.2f);
        punchHitbox.SetActive(false);
        am.SetBool("Left", false);
        am.SetBool("Right", false);
        movementSpeed = defaultSpeed;
        yield return new WaitForSeconds(.1f);
        canPunch = true;
        isPunching = false;

    }

    void Kick()//kick attack uses right leg only, but brings in a hitbox in the same way
    {

        source.PlayOneShot(soundEffects[0], 1);
        am.SetBool("Kicking", true);
        StartCoroutine(kickCoolDown());
    }


    IEnumerator kickCoolDown()//slightly longer cooldown for kick attack since it is strongers
    {

        isKicking = true;
        movementSpeed = 0;
        Leg.SetActive(true);
        timeLeftKickCD = 0.0f; 
        KickCoolDownImage.SetActive(true);
        yield return new WaitForSeconds(.5f);
        Leg.SetActive(false);
        am.SetBool("Kicking", false);
        movementSpeed = defaultSpeed;
        yield return new WaitForSeconds(1.5f);
        canKick = true;
        isKicking = false;
        KickCoolDownImage.SetActive(false);

    }

    void crowBarAttack()//brings out the crowbar model and has the character spin around hitting everythign around it for half a second
    {
        playerBody.SetActive(false);
        Crowbar.SetActive(true);
        spinning = true;
        movementSpeed = 0;
        crowbarOnCooldown = true;
        StartCoroutine(crowbarAttackCooldown());
    }

    IEnumerator crowbarAttackCooldown()//cooldown for crowbar attack
    {
        source.loop = true;
        source.PlayOneShot(soundEffects[1]);
        yield return new WaitForSeconds(0.75f);//the crowbar attack ast for half a second, after that passes put everything away
        Crowbar.SetActive(false);
        playerBody.SetActive(true);
        spinning = false;
        movementSpeed = 10;
        timeLeftCrowbarCD = 0.0f;
        CBCoolDownImage.SetActive(true);
        source.loop = false; 
        yield return new WaitForSeconds(crowBarCooldown);//after the attack ends the cooldown begins
        crowbarOnCooldown = false;
        CBCoolDownImage.SetActive(false); 
    }

    IEnumerator throwBrick()//brick attack instantiates a rigidbody projectile that falls to the ground and needs to be picked up again. 
    {

        animationInput(3);
        hasBrick = false;
        yield return new WaitForSeconds(0.25f);
        GameObject.Instantiate(brick, brickThrowLocation.position, this.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        animationInput(0);
    }


    void block()
    {
        isBlocking = true;
        canBlock = false;
        animationInput(4);
        movementSpeed = 0;
        StartCoroutine(blockCooldown());
    }

    IEnumerator blockCooldown()
    {
        timeBlockCD = 0.0f;
        BlockCoolDownImage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        movementSpeed = defaultSpeed;
        animationInput(0);
        isBlocking = false;
        yield return new WaitForSeconds(1.0f);
        canBlock = true;
        BlockCoolDownImage.SetActive(false);
    }

    //GAME INTERACTION 

    public void takePunch()//method for player to take damage from an enemy punching them 
    {
        source.PlayOneShot(soundEffects[1], 2);
        clout = clout - 2;
        canBeDamaged = false;
        StartCoroutine(invincibility());
    }

    public void takeProjDamage()//method for the player to take damage from an enemy projectile
    {
        source.PlayOneShot(soundEffects[1], 2);
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
        GameObject.Instantiate(moneyEffect, effectLocation.transform.position, Quaternion.identity);
    }

    public void hatAcquired()//instantiates the money effect 
    {
        GameObject.Instantiate(hatEffect, effectLocation.transform.position, Quaternion.identity);
    }

    public void sweaterAcquired()//instantiates the money effect 
    {
        GameObject.Instantiate(sweaterEffect, effectLocation.transform.position, Quaternion.identity);
    }


    public void shoeAcquired()//instantiates the money effect 
    {
        GameObject.Instantiate(shoeEffect, effectLocation.transform.position, Quaternion.identity);
    }



    IEnumerator Die()
    {
        movementSpeed = 0;
        animationInput(1);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("DeathScreenTemp");

    }
}
