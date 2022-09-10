using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSpawn : MonoBehaviour
{
    public float liveTime=5f; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(despawn());
        Debug.Log("instantiated box");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z +1.0f);
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(5.0f);
        Debug.Log("destorying Object");
        Destroy(this.gameObject);
    }
}
