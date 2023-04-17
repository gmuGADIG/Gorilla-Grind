using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObjectTest;

//Testing the unity object (armor class) 
public class UnityObjectDropdownTestPlayer : MonoBehaviour
{
    public UnityObjectDatabaseTest database;

    //Testing Primitive type
    [Dropdown("database.PlayerNameList")]
    public string PlayerName;

    //Testing instance variable
    //[Dropdown("database.HelmetDataList", "Name")]
    [Dropdown("database.HelmetDataList")]
    public HelmetData Helmet;

    //Testing collection item
    [Dropdown("database.ChestplateDataList", "MaterialTypeList[0].MaterialName")]
    public ChestplateData Chestplate;

    //Testing method return
    [Dropdown("database.LeggingsDataList", "GetFirstMaterialName()")]
    public LeggingsData Leggings;

    //Testing [float], [string], [instance variable] and [Enum] with [method] as arguments
    public string publicArrow = "   ===>   ";//private static string privateStaticArrow = "   --->   ";//Test private static arrow
    [Dropdown("database.BootsDataList", "GetMaterialAt(0).GetMaterialNameWithPrefix(\"Material \").Insert(8,  publicArrow  ).Insert(0, \"  \").Insert(0, Rareness.COMMON.ToString())")]
    public BootsData Boots;

    public void DisplayPlayerData()
    {
        Debug.Log($"I am {PlayerName}");
        Debug.Log($"I am wearing {Helmet?.Name}");
        Debug.Log($"I am wearing {Chestplate?.Name}");
        Debug.Log($"I am wearing {Leggings?.Name}");
        Debug.Log($"I am wearing {Boots?.Name}");
    }
    private void Start()
    {
        DisplayPlayerData();
    }
}
