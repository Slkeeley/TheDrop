using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEffect : MonoBehaviour
{
    public GameObject pickUpModel;

    private void Start()
    {
        StartCoroutine(waitToDestroy());
    }
    // Update is called once per frame
    void Update()
    {
        pickUpModel.transform.Rotate(0f, 4f, 0f);
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
