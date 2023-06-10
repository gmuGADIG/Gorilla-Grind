using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class Inventory : MonoBehaviour
{
    private static Inventory GameInventory;
    private static int BananasInInventory; //Number of bananas in player's inventory; should never go below 0
    private static HashSet<string> PurchasedItems;
    private static string equippedBoard;
    
    void Awake() {
            if (GameInventory) {
            Destroy(this);
            return;
        }
        GameInventory = this;
        if (transform.parent = null)
        {
            DontDestroyOnLoad(this);
        }

        BananasInInventory = 500;
        PurchasedItems = new HashSet<string>();
        addItem("All Natural Board"); //Starting board is already in inventory at start of game
        equipBoard("All Natural Board");
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
    ///Returns true on success, false on failure (count wasn't updated). DO NOT USE DURING A RUN! USE RUNDATA INSTEAD!
    ///</summary>
    public static bool AddBananas(int moreBananas) {
        BananasInInventory += (int)moreBananas;
        return true;
    }

    ///<summary>
    ///Removes the passed in number from the count of bananas in the player's inventory.
    ///Returns true on success, false on failure (count wasn't updated)
    ///</summary>
    public static bool RemoveBananas(int lessBananas) {
        if (BananasInInventory >= lessBananas) {
            BananasInInventory -= (int)lessBananas;
            return true;
        }
        else {
            //Cannot take away that many bananas
            return false;
        }
    }

    ///<summary>
    ///Returns true if the passed in item name is stored in the player's inventory
    ///Returns false otherwise
    ///</summary>
    public static bool hasItem(string itemName) {
        return PurchasedItems.Contains(itemName);
    }

    ///<summary>
    ///Add an item's name to the player's inventory.
    ///Returns true on success, meaning the item was not previously in the inventory
    ///Returns false if the item is already in the inventory
    ///</summary>
    public static bool addItem(string itemName) {
        if (hasItem(itemName)) {
            return false;
        }
        PurchasedItems.Add(itemName);
        return true;
    }

    ///<summary>
    ///Set the equipped board to the passed in board name
    ///</summary>
    public static void equipBoard(string boardName) {
        equippedBoard = boardName;
    }

    ///<summary>
    ///Returns the name of the board that the player has equipped
    ///May return null if no board is equipped
    ///</summary>
    public static string getEquippedBoard() {
        return equippedBoard;
    }
}
