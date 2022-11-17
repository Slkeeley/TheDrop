using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class StreetLamp : MonoBehaviour
{
    public TMP_Text parrallelStreetSign;
    public TMP_Text perpendicularStreetSign;
    public GameObject parrallelStreet;
    public GameObject perpendicularStreet;

    void Start()
    {
        parrallelStreetSign.text = parrallelStreet.name;
        perpendicularStreetSign.text = perpendicularStreet.name;
    }

}
