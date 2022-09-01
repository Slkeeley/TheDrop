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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(); 
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
}
