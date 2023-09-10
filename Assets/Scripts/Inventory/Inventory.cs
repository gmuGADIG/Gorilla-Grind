using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class Inventory : MonoBehaviour
{
    private static Inventory Instance;
    private static int BananasInInventory; //Number of bananas in player's inventory; should never go below 0
    private List<string> PurchasedItems;
    private static string equippedBoard;
    
    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            //BananasInInventory = 50000000;
            PurchasedItems = new List<string>();
            //addItem("All Natural Board"); //Starting board is already in inventory at start of game
            //equipBoard("All Natural Board");
        }
        else
        {
            Destroy(this);
        }
        if (transform.parent == null)
        {
            DontDestroyOnLoad(this);
        }

    }

    private void Start()
    {
        UnlockItem("Holy Board", false);
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

    public static void UnlockItem(string itemName, bool displayPopUp = true)
    {
        print(PopupManager.Instance);
        print(itemName);
        if (displayPopUp) PopupManager.Instance.SendPopupMessage("Board Unlocked: " + itemName);
        print("Item Unlocked");
        addItem(itemName);
        equipBoard(itemName, displayPopUp);
    }

    public static void LockItem(string itemName, bool displayPopUp = true)
    {
        if (displayPopUp) PopupManager.Instance.SendPopupMessage("Board Locked: " + itemName);
        print("Board locked: " + itemName);
        RemoveItem(itemName);
        if (equippedBoard == itemName)
        {
            equipBoard("Basic Board");
        }
    }

    ///<summary>
    ///Returns true if the passed in item name is stored in the player's inventory
    ///Returns false otherwise
    ///</summary>
    public static bool hasItem(string itemName) {
        return Instance.PurchasedItems.Contains(itemName);
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
        Instance.PurchasedItems.Add(itemName);
        return true;
    }

    public static void RemoveItem(string itemName)
    {
        Instance.PurchasedItems.Remove(itemName);
    }

    ///<summary>
    ///Set the equipped board to the passed in board name
    ///</summary>
    public static void equipBoard(string boardName, bool displayPopUp = true) {
        if (displayPopUp) PopupManager.Instance.SendPopupMessage("Board Equipped: " + boardName);
        equippedBoard = boardName;
    }

    ///<summary>
    ///Returns the name of the board that the player has equipped
    ///May return null if no board is equipped
    ///</summary>
    public static string getEquippedBoard() {
        return equippedBoard;
    }

    public static List<string> getOwnedBoards() {
        return Instance.PurchasedItems;
    }

    public static void setBananas(int bananas) {
        BananasInInventory = bananas;
    }
}
