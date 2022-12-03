using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
    public GameObject[] cars;
    public GameObject carNotif;
    bool onCooldown = false;
    public float spawnCooldown;
    AudioSource source;
    public AudioClip carRev; 
    private void Awake()
    {
        source = GetComponent<AudioSource>(); 
    }
    private void Start()
    {
        carNotif.SetActive(false);
    }

    void Update()
    {
        if (!onCooldown)//if more enemies can still spawn, bring in a new enemy
        {
            StartCoroutine(spawnCar());
        }
    }


    IEnumerator spawnCar()
    {
        onCooldown = true;
        source.PlayOneShot(carRev, 1); 
        //CAR NOTIFICATION BEFORE CAR SPAWNS
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(true);
        yield return new WaitForSeconds(.1f);
        carNotif.SetActive(false);
        //SPAWN CAR
        //car go sound here
        int carToSpawn = Random.Range(0, cars.Length);
        GameObject.Instantiate(cars[carToSpawn], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        StartCoroutine(cooldown());
    }

    IEnumerator cooldown()//make sure that cars are not constantly spawning 
    {
        yield return new WaitForSeconds(spawnCooldown);
        onCooldown = false;
    }
}
