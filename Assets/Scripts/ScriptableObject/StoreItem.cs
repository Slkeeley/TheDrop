using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StoreItem", menuName = "Scriptable Objects/items")]
public class StoreItem : ScriptableObject
{
    [SerializeField]
    public int priceMin;
    public int priceMax;
    public int priceCurr;
    public string itemName;
    public Sprite itemImage;
    public Sprite resellListingImage; 

    
    public void PriceRandomizer()//way to randomize prices of the items between drops
    {
        priceCurr = Random.Range(priceMin, priceMax + 1);
        priceCurr = (priceCurr / 10) * 10;
    }

}
