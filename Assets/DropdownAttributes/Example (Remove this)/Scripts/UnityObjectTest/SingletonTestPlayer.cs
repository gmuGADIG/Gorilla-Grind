using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObjectTest;

//Testing the serialized object (armor class) 
public class SingletonTestPlayer : MonoBehaviour
{
    //Testing Primitive type
    [Dropdown("UnityObjectDatabaseTest.Instance.PlayerNameList")]
    public string PlayerName;

    //Testing instance variable
    //[Dropdown("database.HelmetDataList", "Name")]
    [Dropdown("UnityObjectDatabaseTest.Instance.HelmetDataList")]
    public HelmetData Helmet;

    //Testing collection item
    [Dropdown("UnityObjectDatabaseTest.GetInstance().ChestplateDataList", "MaterialTypeList[0].MaterialName")]
    public ChestplateData Chestplate;

    //Testing method return
    [Dropdown("UnityObjectDatabaseTest.GetInstance().LeggingsDataList", "GetFirstMaterialName()")]
    public LeggingsData Leggings;

    //Testing method and collection together
    [Dropdown("UnityObjectDatabaseTest.GetInstance().BootsDataList", "GetFirstMaterialName()[16]")]
    public BootsData Boots;//Display the first letter of the material

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
