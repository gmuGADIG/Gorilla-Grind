
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public delegate string GetItemNameCallback(object baseMaster, object master);
/// <summary>
/// [Dropdown(path, displayProperty)]
/// 
/// - path:            the path of the List
/// - displayProperty: the property you want to display in the dropdown selection
/// 
/// </summary>
public class DropdownAttribute : PropertyAttribute
{
    public Type Type = null;
    public string ListPath = "";
    public string ItemNameProperty = "";
    public List<Object> List = new List<Object>();
    public int SelectedID = -1;
    public GetItemNameCallback GetItemName = null;
    public DropdownAttribute(string listPath, string ItemNameProperty)
    {//With property name to get name
        //[Dropdown("SkillDatabase.Instance.SkillList", "skillID")]
        ListPath = listPath;
        GetItemName = ( (baseMaster, obj) =>
         {
             return ReflectionSystem.GetValue(baseMaster, obj, ItemNameProperty)?.ToString();
         });
        this.ItemNameProperty = ItemNameProperty;
    }
    public DropdownAttribute(string listPath)
    {
        ListPath = listPath;
        GetItemName = ((baseMaster, master) =>
        {
            if(master is Object)
            {//is Unity Object
                return master.GetType().GetProperty("name").GetValue(master).ToString();
            }
            else if(master is string)
            {//string
                return master.ToString();
            }
            else if(master.GetType().IsPrimitive)
            {//short, long, int, bool, char, double, float
                return master.ToString();
            }
            else
            {//object
                return JsonUtility.ToJson(master);
            }
        });
    }
}
