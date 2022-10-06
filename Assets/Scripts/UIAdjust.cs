using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdjust : MonoBehaviour
{
    public Transform south;
    public GameObject itemText;
    public GameObject priceText;
    // Start is called before the first frame update
    void Start()
    {
        south = GameObject.Find("South").transform;
        itemText.transform.LookAt(south.position);
        priceText.transform.LookAt(south.position);
    }
    
}
