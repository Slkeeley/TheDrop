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
    public float turnSmoothTime = 0.5f;
    float turnSmoothVelocity;
    public float movementSpeed = 10;

    [Header("Gameplay Variables")]//variables for player-game interaction
    public int money=0;
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
    public GameObject leftArm; 
    public GameObject rightArm;
    bool canKick = true;//kick attack
    public GameObject Leg;
    public bool hasCrowbar = false;//crowbar attack
    bool crowbarOnCooldown = false; 
    bool spinning = false; 
    public GameObject Crowbar;
    public float crowBarCooldown;
    public GameObject brick;//brick attack
    public Transform brickThrowLocation;//position that the brick should spawn in
    public bool hasBrick = false; 

    [Header("UI Elements")]//data for player UI s
    public TMP_Text moneyText; 
    public TMP_Text healthText; 
    public TMP_Text sweaterText; 
    public TMP_Text shoesText; 
    public TMP_Text hatsText;
    public Image healthBar;

    void Start()//put the players weapons away and make sure that the UI reflects default values
    {
        leftArm.SetActive(false);
        rightArm.SetActive(false);
        Leg.SetActive(false);
        updateUI();
    }


    void Update()
    {
        if (clout <= 0) SceneManager.LoadScene("DeathScreenTemp");
        Move();//always check if the player is moving
        attackInputs();//always check if the player is trying to attack;
        if (spinning) transform.Rotate(0f, 2.8f, 0f);//rotate the player if they are doing the spinning crowbar attack
    }

    private void LateUpdate()
    {
        updateUI(); //Update the players UI every frame to quickly show any changes
        if (transform.position.y != 1.0f)
        {
            Vector3 groundCheck = new Vector3(transform.position.x, 1.0f, transform.position.z);
            transform.position = groundCheck;
        }
  
    }

    void Move()//movement method for a 3D space
    {
        //Get Input Axes 
        float vertAxis = Input.GetAxis("Vertical") * movementSpeed;
        float horizontalAxis = Input.GetAxis("Horizontal") * movementSpeed;

       //detect what direction the player is trying to move
        Vector3 direction = new Vector3(horizontalAxis, 0f, vertAxis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //rotate the player in the direction they are trying to move 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
           // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            //move the player with speed independent of frame rate
            controller.Move(direction * movementSpeed * Time.deltaTime);
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
    }
    void Punch()//punch attack alternates between left and right arms
    {
        if(rightArmNext)//punch with right arm 
        {
            rightArm.SetActive(true);
            rightArmNext = false;
            StartCoroutine(punchCoolDown());
        }
        else//punch with left arm
        {
            leftArm.SetActive(true);
            rightArmNext = true;
            StartCoroutine(punchCoolDown());
        }
    }
    IEnumerator punchCoolDown()//put the players fist away after the attack is over and allow the player to be able to punch again
    {
        yield return new WaitForSeconds(.5f);
        canPunch = true;
        rightArm.SetActive(false);
        leftArm.SetActive(false);
    }

    void Kick()//kick attack uses right leg only, but brings in a hitbox in the same way
    {
        Leg.SetActive(true);
        StartCoroutine(despawnLeg());
        StartCoroutine(kickCoolDown());
    }

    IEnumerator despawnLeg()//put the players leg away after the attack is over, but the attack is over before they can choose to attack again
    {
        yield return new WaitForSeconds(.5f);
        Leg.SetActive(false);
    }

    IEnumerator kickCoolDown()//slightly longer cooldown for kick attack since it is strongers
    {
        yield return new WaitForSeconds(1.0f);
        canKick = true; 
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


    void updateUI()//change all parts UI display depending on the players current situations
    {
        moneyText.text = "Bread: " + money.ToString(); 
        healthText.text = "Clout: " + clout.ToString(); 
        sweaterText.text = "Sweaters: " + sweatersHeld.ToString(); 
        shoesText.text = "Shoes: " + shoesHeld.ToString(); 
        hatsText.text = "Hats: " + hatsHeld.ToString();
        healthBar.fillAmount = Mathf.Clamp(clout / MaxHealth, 0, 1f);
    }

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
}
