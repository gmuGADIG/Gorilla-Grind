using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory GameInventory;
    private static int BananasInInventory; //Number of bananas in player's inventory; should never go below 0
    // Start is called before the first frame update
    private static HashSet<string> PurchasedItems;
    
    void Awake() {
            if (GameInventory) {
            Destroy(this);
            return;
        }
        GameInventory = this;
        DontDestroyOnLoad(this);

        BananasInInventory = 0;
        PurchasedItems = new HashSet<string>(); 
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///<summary>
    ///Returns the number of bananas currently stored in the player's inventory.
    ///</summary>
    public static int getBananasInInventory() {
        return BananasInInventory;
    }

    ///<summary>
    ///Adds the passed in number to the count of bananas in the player's inventory.
    ///Returns true on success, false on failure (count wasn't updated)
    ///</summary>
    public static bool AddBananas(int moreBananas) {
        BananasInInventory += moreBananas;
        return true;
    }

    ///<summary>
    ///Removes the passed in number from the count of bananas in the player's inventory.
    ///Returns true on success, false on failure (count wasn't updated)
    ///</summary>
    public static bool RemoveBananas(int lessBananas) {
        if (BananasInInventory >= lessBananas) {
            BananasInInventory -= lessBananas;
            return true;
        }
        else {
            //Cannot take away that many bananas
            return false;
        }
    }

    public static bool hasItem(string itemName) {
        return PurchasedItems.Contains(itemName);
    }

    public static bool addItem(string itemName) {
        if (hasItem(itemName)) {
            return false;
        }
        PurchasedItems.Add(itemName);
        return true;
    }
}
