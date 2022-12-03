using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Player Attachment")]
    public GameObject player;
    Player playerScript;

    [Header("Status")]//data for player UI s
    public TMP_Text moneyText;
    public TMP_Text healthText;
    public Image healthBar;
    public Image healthImage;
    public Image moneyMeter;
    public GameObject crowBar;
    public GameObject brick;

    [Header("Notification System")]
    public TMP_Text dropText;
    public Image phone;
    public GameObject messages;
    public GameObject DMS;
    public int msgNotifications;
    public int socialNotifications;
    public TMP_Text messagesText; 
    public TMP_Text dmText;
    public AudioClip vibrate;
    AudioSource source;

    // Start is called before the first frame update
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        updateUI(); //Update the players UI every frame to quickly show any changes
        if (msgNotifications > 0) messages.SetActive(true);
        else messages.SetActive(false);

        if (socialNotifications > 0) DMS.SetActive(true);
        else DMS.SetActive(false);

        if (player.GetComponent<Player>().hasCrowbar) crowBar.SetActive(true);//if the player has the crowbar show them that they can use it 
        else crowBar.SetActive(false);

        if (player.GetComponent<Player>().hasBrick) brick.SetActive(true);//if the player has the brick show them they can use it 
        else brick.SetActive(false);
    }


    void updateUI()//change all parts UI display depending on the players current situations
    {
        moneyText.text = "Bread: " + playerScript.money.ToString();
        healthText.text = "Clout: " + playerScript.clout.ToString();

        healthBar.fillAmount = Mathf.Clamp(playerScript.clout / playerScript.MaxHealth, 0, 1f);
        moneyMeter.fillAmount = Mathf.Clamp(playerScript.money / 500, 0, 1f);
        float hatPos = -500 - (Mathf.Clamp(playerScript.clout / playerScript.MaxHealth, 0, 1f) * -500);
        healthImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, hatPos);
        messagesText.text = msgNotifications.ToString();
        dmText.text = socialNotifications.ToString();
    }


    public void dropAnnouncement()//Accessed from another script to notify the player's phone 
    {
        StartCoroutine(phoneVibrate());
        StartCoroutine(hideDropText());
    }

    IEnumerator phoneVibrate()//slightly rotate the phone object when a drop is announced
    {
        source.PlayOneShot(vibrate, 1);
        phone.rectTransform.Rotate(0, 0, -15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, -30);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 15);
        yield return new WaitForSeconds(.2f);
        phone.rectTransform.Rotate(0, 0, 0);
    }
    IEnumerator hideDropText()//change the drop text shortly after the announcement 
    {
        yield return new WaitForSeconds(8f);
        dropText.text = "";
    }
}
