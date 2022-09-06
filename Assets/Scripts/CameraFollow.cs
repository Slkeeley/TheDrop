using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float offsetDistance = 5.0f;
    // Update is called once per frame
    void LateUpdate()
    {
           transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z-offsetDistance);
    }
}
