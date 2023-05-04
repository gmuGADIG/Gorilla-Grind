using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemScript : MonoBehaviour
{

    [Tooltip("Display name of this item")]
    public string itemName;
    [Tooltip("Number of bananas to deduct from inventory when player purchases item")]
    public int price;

    private bool isPurchased;
    private Button buyButton;
    private TMP_Text buttonText;

    int itemBoughtSoundID;

    // Start is called before the first frame update
    void Start()
    {
        isPurchased = Inventory.hasItem(itemName);
        buyButton = gameObject.GetComponentInChildren<Button>();
        buttonText = buyButton.GetComponentInChildren<TMP_Text>();


        if (isPurchased) {
            setButtonToPurchased();
        }
        buyButton.onClick.AddListener(purchase);

        if (itemName.Length == 0) {
            itemName = gameObject.name;
        }

        SoundManager.Instance.GetSoundID("Shop_Purchase_Item");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void purchase() {
        if (!(Inventory.hasItem(itemName))) {
            Inventory.addItem(itemName);
            setButtonToPurchased();
            SoundManager.Instance.PlaySoundGlobal(itemBoughtSoundID);
        }
    }

    private void setButtonToPurchased() {
        isPurchased = true;
        buyButton.interactable = false;
        buttonText.text = "Bought";
    }
}
