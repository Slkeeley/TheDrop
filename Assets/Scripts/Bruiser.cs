using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : BasicEnemy
{
    public GameObject fists;
    // Start is called before the first frame update
    void Start()
    {
        fists.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
