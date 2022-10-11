using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    public GameObject moneyModel;

    private void Start()
    {
        StartCoroutine(waitToDestroy());
    }
    // Update is called once per frame
    void Update()
    {
        moneyModel.transform.Rotate(0f, 3f, 0f);
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
