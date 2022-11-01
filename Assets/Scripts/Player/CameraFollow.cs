using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float offsetDistance = 5.0f;//distance to place the camera in a more aethestecically pleasing positon

    private void FixedUpdate()
    {
        checkBuilding(); 
    }

    void LateUpdate()
    {
           transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z-offsetDistance);//check where the player currently is at the end of the frame and move the camera above them 
    }

    void checkBuilding()
    {

    }
}
