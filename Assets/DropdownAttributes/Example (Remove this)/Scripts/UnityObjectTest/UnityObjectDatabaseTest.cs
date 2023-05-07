using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObjectTest;

public class UnityObjectDatabaseTest : MonoBehaviour
{
    public string[] PlayerNameList;
    public HelmetData[] HelmetDataList;
    public List<ChestplateData> ChestplateDataList;
    public List<LeggingsData> LeggingsDataList;
    public List<BootsData> BootsDataList;
    #region Singleton
    private static UnityObjectDatabaseTest instance = null;
    private static readonly Object syncRoot = new Object();
    public static UnityObjectDatabaseTest Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = GameObject.FindObjectOfType(typeof(UnityObjectDatabaseTest)) as UnityObjectDatabaseTest;
                        if (instance == null)
                            Debug.LogError("SingletoneBase<T>: Could not found GameObject of type " + typeof(UnityObjectDatabaseTest).Name);
                    }
                }
            }
            return instance;
        }
        set { }
    }
    public static UnityObjectDatabaseTest GetInstance()
    {
        return Instance;
    }
    #endregion
}
