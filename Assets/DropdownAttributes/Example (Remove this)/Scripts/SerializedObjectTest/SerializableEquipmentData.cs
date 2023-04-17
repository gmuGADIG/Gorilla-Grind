using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rareness
{
    COMMON,
    UNCOMMON,
    RARE,
    SUPERRAE,
    LEGENDARY
}
[System.Serializable]
public class MaterialType
{
    public string MaterialName;
    public string GetMaterialNameWithPrefix(string p)
    {
        return p + MaterialName;
    }
}
namespace SerializedObjectTest
{

    [System.Serializable]
    public class EquipmentData
    {
        public string Name;
        public float ProtectionLevel;
        public List<MaterialType> MaterialTypeList;
        public string GetFirstMaterialName()
        {
            if (MaterialTypeList.Count > 0)
                return "First material: " + MaterialTypeList[0].MaterialName;
            else
                return "No Material";
        }
        public MaterialType GetMaterialAt(int index)
        {
            return MaterialTypeList[index];
        }

    }



    [System.Serializable]
    public class HelmetData : EquipmentData
    {
    }

    [System.Serializable]
    public class ChestplateData : EquipmentData
    {
    }

    [System.Serializable]
    public class LeggingsData : EquipmentData
    {
    }

    [System.Serializable]
    public class BootsData : EquipmentData
    {
    }
}