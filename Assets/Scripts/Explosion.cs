using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(removeFromScene());
    }

    IEnumerator removeFromScene()
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
    }
  
}
