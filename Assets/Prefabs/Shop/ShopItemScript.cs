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
    private TMP_Text buyButtonText;
    private Button equipButton;
    private TMP_Text equipButtonText;
    private TMP_Text itemNameText;
    private TMP_Text priceText;
    private Image panelImage;

    // Start is called before the first frame update
    void Start()
    {
        if (itemName.Length == 0) {
            itemName = gameObject.name;
        }

        isPurchased = Inventory.hasItem(itemName);
        buyButton = transform.Find("BuyButton").GetComponent<Button>();
        buyButtonText = buyButton.GetComponentInChildren<TMP_Text>();
        equipButton = transform.Find("EquipButton").GetComponent<Button>();
        equipButtonText = equipButton.GetComponentInChildren<TMP_Text>();
        itemNameText = transform.Find("ItemName").GetComponent<TMP_Text>();
        priceText = transform.Find("Price").GetComponent<TMP_Text>();
        panelImage = gameObject.GetComponent<Image>();

        if (isPurchased) {
            onPurchased();
        }
        else {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
        }
        buyButton.onClick.AddListener(purchase);
        equipButton.onClick.AddListener(onEquipButtonPressed);

        itemNameText.text = itemName;
        priceText.text = price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if equipped and update equip button if this is the case
        if (equipButton.enabled) {
            if (Inventory.getEquippedBoard() == itemName) {
                equipButton.interactable = false;
                equipButtonText.text = "Equipped";
            }
            else {
                equipButton.interactable = true;
                equipButtonText.text = "Equip";
            }
        }
    }

    public void purchase() {
        if (Inventory.hasItem(itemName)) {
            //Item is already purchased, don't purchase again
            return;
        }
        if (Inventory.getBananasInInventory() < price) {
            //User does not have enough bananas to purchase this item
            return;
        }
        Inventory.RemoveBananas(price);
        Inventory.addItem(itemName);
        onPurchased();
    }

    private void onPurchased() {
        isPurchased = true;
        buyButton.interactable = false;
        buyButtonText.text = "Bought";
        buyButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(true);
        panelImage.color = new UnityEngine.Color(0.63f,1f,0.47f,0.7f); //Light green color
    }

    public void onEquipButtonPressed() {
        if (!(Inventory.hasItem(itemName))) {
            return;
        }
        // Board is in inventory
        Inventory.equipBoard(itemName);
    }
}
