using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockableItemUI : MonoBehaviour
{
    [Tooltip("Display name of this item")]
    public string itemName;
    [Tooltip("The file in Resources/Sounds to play when item is equipped")]
    public string equipSoundName;

    [SerializeField] GameObject lockSymbol;

    private bool isUnlocked;
    private Button equipButton;
    private TMP_Text equipButtonText;
    private TMP_Text itemNameText;
    private Image panelImage;

    private int equippingSoundId = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (itemName.Length == 0)
        {
            itemName = gameObject.name;
        }

        isUnlocked = Inventory.hasItem(itemName);
        equipButton = transform.Find("EquipButton").GetComponent<Button>();
        equipButtonText = equipButton.GetComponentInChildren<TMP_Text>();
        itemNameText = transform.Find("ItemName").GetComponent<TMP_Text>();
        panelImage = gameObject.GetComponent<Image>();
        equipButton.onClick.AddListener(OnEquipButtonPressed);

        itemNameText.text = itemName;

        if (isUnlocked)
        {
            OnUnlock();
        }

        equippingSoundId = SoundManager.Instance.GetSoundID(equipSoundName);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if equipped and update equip button if this is the case
        if (equipButton.enabled)
        {
            if (Inventory.getEquippedBoard() == itemName)
            {
                equipButton.interactable = false;
                equipButtonText.text = "Equipped";
            }
            else
            {
                equipButton.interactable = true;
                equipButtonText.text = "Equip";
            }
        }
    }

    public void Lock()
    {

    }

    public void Unlock()
    {
        if (Inventory.hasItem(itemName))
        {
            return;
        }
        Inventory.addItem(itemName);
        OnUnlock();
    }

    private void OnUnlock()
    {
        isUnlocked = true;
        lockSymbol.SetActive(false);
        equipButton.gameObject.SetActive(true);
        panelImage.color = new Color(0.63f, 1f, 0.47f, 0.7f); //Light green color
    }

    public void OnEquipButtonPressed()
    {
        if (!(Inventory.hasItem(itemName)))
        {
            return;
        }
        // Board is in inventory
        if (Inventory.getEquippedBoard() == itemName) return; // do nothing if this board is already equipped

        Inventory.equipBoard(itemName);
        SoundManager.Instance.PlaySoundGlobal(equippingSoundId);
    }
}
