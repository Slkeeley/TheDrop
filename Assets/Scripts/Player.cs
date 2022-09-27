using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Player : MonoBehaviour
{
    [Header("Controller Variables")]
    public CharacterController controller;
    public float turnSmoothTime = 0.5f;
    float turnSmoothVelocity;

    [Header("Gameplay Variables")]
    public float movementSpeed=10;
    public int money=0;
    public float clout = 100;
    public float MaxHealth = 100;
    public bool canBeDamaged = true;

    [Header("Inventory")]
    public int sweatersHeld;
    public int shoesHeld;
    public int hatsHeld;

    [Header("Attacks")]
    bool rightArmNext = true;
    bool canPunch = true; 
    bool canKick = true; 
    public GameObject leftArm; 
    public GameObject rightArm; 
    public GameObject Leg; 

    [Header("UI Elements")]
    public TMP_Text moneyText; 
    public TMP_Text healthText; 
    public TMP_Text sweaterText; 
    public TMP_Text shoesText; 
    public TMP_Text hatsText;
    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        leftArm.SetActive(false);
        rightArm.SetActive(false);
        Leg.SetActive(false);
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        Move();//always check if the player is moving
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

    }

    private void LateUpdate()
    {
        updateUI(); //Update the players UI every frame to quickly show any changes
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
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //move the player with speed independent of frame rate
            controller.Move(direction * movementSpeed * Time.deltaTime);
        }
    }
    void Punch()//punch attack alternates between left and right arms
    {
        if(rightArmNext)
        {
            rightArm.SetActive(true);
            rightArmNext = false;
            StartCoroutine(punchCoolDown());
        }
        else
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

    void Kick()
    {
        Leg.SetActive(true);
        StartCoroutine(despawnLeg());
        StartCoroutine(kickCoolDown());
    }

    IEnumerator despawnLeg()//put the players fist away after the attack is over and allow the player to be able to punch again
    {
        yield return new WaitForSeconds(.5f);
        Leg.SetActive(false);
    }

    IEnumerator kickCoolDown()
    {
        yield return new WaitForSeconds(1.0f);
        canKick = true; 
    }
    void updateUI()
    {
        Debug.Log("updating UI");
        moneyText.text = "Bread: " + money.ToString(); 
        healthText.text = "Clout: " + clout.ToString(); 
        sweaterText.text = "Sweaters: " + sweatersHeld.ToString(); 
        shoesText.text = "Shoes: " + shoesHeld.ToString(); 
        hatsText.text = "Hats: " + hatsHeld.ToString();
        healthBar.fillAmount = Mathf.Clamp(clout / MaxHealth, 0, 1f);
    }

    public void takePunch()
    {
        clout = clout - 2;
        canBeDamaged = false;
        StartCoroutine(invincibility());
    }

    public void takeProjDamage()
    {
        clout = clout - 5;
        canBeDamaged = false;
        StartCoroutine(invincibility());
    }

    IEnumerator invincibility()
    {
        yield return new WaitForSeconds(0.5f);
        canBeDamaged = true;
    }
}
