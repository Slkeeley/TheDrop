using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public int speed;
    AudioSource source;

    // Start is called before the first frame update
    private void Awake()
    {
        source = GetComponent<AudioSource>(); 
    }
    void Start()
    {
        source.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Wall")
        {
            GameControl.enemiesInPlay--;
            Destroy(this.gameObject);
        }
    }
}
