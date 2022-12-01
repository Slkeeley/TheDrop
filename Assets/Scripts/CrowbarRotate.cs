using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 2f, 0f);//rotate this object if they are doing the spinning crowbar attack
    }
}
