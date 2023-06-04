using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityObjectTest
{
    public class MonobehaviourEquipmentData : MonoBehaviour
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

}