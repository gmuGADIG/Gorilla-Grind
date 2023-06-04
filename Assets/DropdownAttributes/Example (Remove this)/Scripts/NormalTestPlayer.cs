using SerializedObjectTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTestPlayer : MonoBehaviour
{
    public SerializedObjectDatabaseTest database;
    [Dropdown("database.PlayerNameList")]
    public string PlayerName;

    [Dropdown("database.HelmetDataList", "")]
    public HelmetData Helmet;

    [Dropdown("database.ChestplateDataList")]
    public ChestplateData Chestplate;

    [Dropdown("database.LeggingsDataList")]
    public LeggingsData Leggings;

    [Dropdown("database.BootsDataList")]
    public BootsData Boots;
}
