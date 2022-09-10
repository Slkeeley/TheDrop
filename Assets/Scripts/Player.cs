using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    public float movementSpeed=10;
    public int money=0;
    public int clout = 20;

    bool leftArmNext = false;
    bool rightArmNext = true;
    bool canPunch = true; 
    public GameObject punchBox; 
    public GameObject leftArm; 
    public GameObject rightArm; 

    public TMP_Text moneyText; 
    public TMP_Text healthText; 
    // Start is called before the first frame update
    void Start()
    {
        leftArm.SetActive(false);
        rightArm.SetActive(false);
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
            if (Input.GetButton("Fire1"))
            {
                if (canPunch)
             {
               
                canPunch = false;
                StartCoroutine(punchCooldown());
                Punch();
               }
            }
        
        
        if(Input.GetButton("Fire2"))
        {
            //kick
        }

        updateUI(); 
    }

    private void LateUpdate()
    {
        updateUI();
    }

    void Move()
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
    void Punch()
    {
        if(rightArmNext)
        {
            rightArm.SetActive(true);
            Instantiate(punchBox, transform.position, Quaternion.identity);
            rightArmNext = false;
            leftArmNext = true;
            StartCoroutine(despawnFists());
        }
        else
        {
            leftArm.SetActive(true);
            Instantiate(punchBox, transform.position, Quaternion.identity);
            leftArmNext = false;
            rightArmNext = true;
            StartCoroutine(despawnFists());
        }
    }
    IEnumerator despawnFists()
    {
        yield return new WaitForSeconds(.5f);
        rightArm.SetActive(false);
        leftArm.SetActive(false);
    }

    IEnumerator punchCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canPunch = true; 
    }
    void Kick()
    {

    }

    void updateUI()
    {
        moneyText.text = "Bread: " + money.ToString(); 
        healthText.text = "Clout: " + clout.ToString(); 
    }

    
}
