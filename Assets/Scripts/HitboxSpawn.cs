using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSpawn : MonoBehaviour
{
    public float liveTime;
    public float Offset;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(despawn());
        Debug.Log("instantiated box");
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Offset);
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(liveTime);
        Debug.Log("destorying Object");
        Destroy(this.gameObject);
    }
}
