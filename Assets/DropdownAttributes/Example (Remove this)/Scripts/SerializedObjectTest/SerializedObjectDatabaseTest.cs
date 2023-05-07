using SerializedObjectTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedObjectDatabaseTest : MonoBehaviour
{
    public List<string> PlayerNameList;
    public List<HelmetData> HelmetDataList;
    public List<ChestplateData> ChestplateDataList;
    public List<LeggingsData> LeggingsDataList;
    public List<BootsData> BootsDataList;
    public void Start()
    {
        Debug.Log($"material type list length: {BootsDataList[0].MaterialTypeList.Count}");
    }
}
