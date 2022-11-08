using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInvis : MonoBehaviour
{
    public bool intersecting = false;
    private float invisRadius=20;
    public Renderer rd; 
    Color ogColor; 
    Color transColor;
    Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        ogColor = rd.material.color;
        transColor = new Color(ogColor.r, ogColor.g, ogColor.b, 0.3f);
        camera = GameObject.Find("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        checkDistance();
        if (intersecting)
        {
           
            rd.material.color = transColor;
        }
        else
        {
         
            rd.material.color = ogColor;
        }
    }

    void checkDistance()
    {
        Vector3 distToPoint = transform.position - camera.position;
        if (distToPoint.magnitude < invisRadius)
        {
   //         rd.material.SetFloat("_Mode", 3);
            intersecting = true;

        }
        else
        {
     //       rd.material.SetFloat("_Mode", 0);
            intersecting = false;
        }
    }
}
