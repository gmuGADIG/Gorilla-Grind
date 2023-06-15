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
    [Tooltip("The file in Resources/Sounds to play when item is equipped")]
    public string equipSoundName;

    private bool isPurchased;
    private Button buyButton;
    private TMP_Text buyButtonText;
    private Button equipButton;
    private TMP_Text equipButtonText;
    private TMP_Text itemNameText;
    private TMP_Text priceText;
    private Image panelImage;
    private Image bananaImage;

    private int purchasingSoundId = -1;
    private int equippingSoundId = -1;

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
        bananaImage = transform.Find("Banana").GetComponent<Image>();

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

        purchasingSoundId = SoundManager.Instance.GetSoundID("Shop_Purchase_Item");
        if (equipSoundName != "")
        {
            equippingSoundId = SoundManager.Instance.GetSoundID(equipSoundName);
        }
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

        SoundManager.Instance.PlaySoundGlobal(purchasingSoundId);
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
        panelImage.color = new Color(0.63f,1f,0.47f,0.7f); //Light green color
        bananaImage.gameObject.SetActive(false);
        priceText.gameObject.SetActive(false);
    }

    public void onEquipButtonPressed() {
        if (!(Inventory.hasItem(itemName))) {
            return;
        }
        // Board is in inventory
        if (Inventory.getEquippedBoard() == itemName) return; // do nothing if this board is already equipped
        
        Inventory.equipBoard(itemName);
        SoundManager.Instance.PlaySoundGlobal(equippingSoundId);
    }
}
