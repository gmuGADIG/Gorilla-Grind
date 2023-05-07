#if UNITY_EDITOR
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
[CustomPropertyDrawer(typeof(DropdownAttribute))]
public class DropdownAttributeDrawer : PropertyDrawer
{
    public List<Object> SerializedPropertyToList(SerializedProperty property)
    {
        if (!property.isArray) return new List<Object>() { property.objectReferenceValue };
       // Debug.Log($"property is array, size: {property.arraySize}");
        List<Object> result = new List<Object>();
        for (int i = 0; i < property.arraySize; i++)
        {
            SerializedProperty item = property.GetArrayElementAtIndex(i);
            //Debug.Log($"found item in list: {item.name}");
            result.Add(item.objectReferenceValue);
        }
        return result;
    }
    public List<Object> CastObjectToList(object obj)
    {
        IList collection = (IList)obj;
        List<Object> result = new List<Object>();
        foreach(var o in collection)
        {
            result.Add((Object)o);
        }
        //Debug.Log($"casting object to list: {obj.GetType()}");
        return result;
    }
    public List<Object> FindList(SerializedObject obj, string path)
    {//Find the list in the serializedobject using the path

        SerializedProperty foundProperty = obj.FindProperty(path);
        //Debug.Log($"Found list result: {result.name}");
        if (foundProperty != null)
        {
            return SerializedPropertyToList(foundProperty);
        }
        else
        {//then try find the list from static class
            //Debug.Log($"Cannot find list from path: {path}, trying static classes");
            string[] items = path.Split('.');
            string typeString = items[0];
            Type staticClass = Type.GetType(typeString);//static class
            //Debug.Log($"static class type: {staticClass}");

            #region Try field
            foreach(var field in staticClass.GetFields())
            {
                Debug.Log($"field: {field}");
            }
            FieldInfo fieldInfo = staticClass.GetField(items[1]);//the instance field
            if (fieldInfo != null)
            {
                Debug.Log($"searching info: {items[1]}  ");
                Debug.Log($"found info: {fieldInfo.Name}");
                object result = fieldInfo.GetValue(null);//the instance object
                Debug.Log($"found instance object result: {result.ToString()}");
                for (int i = 2; i < items.Length; i++)
                {
                    Debug.Log($"found result: {result.ToString()}");
                    Debug.Log($"found field: {result.GetType().GetField(items[i])}");
                    result = result.GetType().GetField(items[i]).GetValue(result);//the instance object
                }
                Debug.Log($"found result: {result.ToString()}");
                return CastObjectToList(result);
            }
            #endregion
            #region Try property (with getter and setter)

            foreach (var p in staticClass.GetProperties())
            {
                Debug.Log($"property: {p}");
            }
            PropertyInfo propertyInfo = staticClass.GetProperty(items[1]);//the instance Property
            if (propertyInfo != null)
            {
                Debug.Log($"searching info: {items[1]}  ");
                Debug.Log($"found info: {propertyInfo.Name}");
                object result = propertyInfo.GetValue(null);//the instance object
                Debug.Log($"found instance object result: {result.ToString()}");
                for (int i = 2; i < items.Length; i++)
                {
                    Debug.Log($"found result: {result.ToString()}");
                    Debug.Log($"found Property: {result.GetType().GetProperty(items[i])}");
                    result = result.GetType().GetProperty(items[i]).GetValue(result);//the instance object
                }
                Debug.Log($"found result: {result.ToString()}");
                return CastObjectToList(result);
            }
            #endregion
            return null;
        }
    }
    public string[] ListToStringArray(List<object> list, GetItemNameCallback GetItemName, object baseMaster)
    {
        string[] result = new string[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            object obj = list[i];
            if (GetItemName == null) result[i] = obj.ToString();
            else result[i] = GetItemName(baseMaster, obj);

        }
        return result;
    }
    public string[] ListToStringArray(List<object> list)
    {
        string[] result = new string[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            object obj = list[i];
            result[i] = obj.ToString();

        }
        return result;
    }
    public string[] ListToStringArray(List<Object> list, GetItemNameCallback GetItemName, object baseMaster)
    {
        string[] result = new string[list.Count];
        int errorCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            Object obj = list[i];
            if (GetItemName == null) result[i] = obj.name;
            else
            {
                string name = GetItemName(baseMaster, obj);
                //Debug.Log($"get name : {name}");
                if (name == null)
                {
                    name = "[ERROR]wrong property name";
                    errorCount++;
                }

                result[i] = name;
            }
        }
        if (errorCount >= result.Length) return null;//All wrong that mean the property name is wrong.
        return result;
    }
    public int FindSelectedID<T>(List<T> list, T obj)
    {
        return list.IndexOf(obj);
    }
    public int FindSystemObjectSelectedID(List<object> list, object target, GetItemNameCallback GetItemName, object baseMaster)
    {
        try
        {
            for (int i = 0; i < list.Count; i++)
            {
                object obj = list[i];
                if (GetItemName(baseMaster, obj) == GetItemName(baseMaster, target))
                {
                    return i;
                }
            }
        }catch(Exception e)
        {
        }
        return -1;
    }
    private IList CreateListWithCustomType(Type itemType)
    {
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(itemType);
        var instance = Activator.CreateInstance(constructedListType);
        return (IList)instance;
    }
    private List<T> CastWholeList<T>(object list, Type itemType)
    {//CastWholeList(new List<int>(){1, 2, 3}, string)
        IList oldList = (IList)list;
        IList newList = CreateListWithCustomType(itemType);
        foreach(var item in oldList)
        {
            newList.Add(item);
        }
        return (List<T>)newList;
    }
    private bool IsUnityObject(object obj)
    {
        return obj is Object;
    }
    private GUIStyle GetGUIColor(Color color)
    {
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = color;
        style.active.textColor = color;
        style.focused.textColor = color;
        style.hover.textColor = color;
        return style;
    }
    private GUIStyle GetDropdownStyle(Color c)
    {
        GUIStyle style = EditorStyles.popup;
        return style;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;
            if (dropdownAttribute.ListPath == "")
            {//PATH EMPTY
                EditorGUI.LabelField(position, property.name, $"The List path could not be empty", GetGUIColor(Color.red));
                return;
            }
            Type itemType = dropdownAttribute.Type;//store the type
            GetItemNameCallback GetItemName = dropdownAttribute.GetItemName;//store the callback
            object BaseMaster = property.serializedObject.targetObject;//store the base master
            object listObj = ReflectionSystem.GetValue(BaseMaster, dropdownAttribute.ListPath);//get the list from path
            if (listObj == null)
            {//PATH NOT WORKING
                EditorGUI.LabelField(position, property.name, $"The list \"{dropdownAttribute.ListPath}\" cannot be found", GetGUIColor(Color.red));
                return;
            }
            //Debug.Log($"the object: {JsonUtility.ToJson(listObj)}");
            IList ilist = (IList)listObj;
            if (ilist.Count > 0)
                if (IsUnityObject(ilist[0]))
                {//Can use UnityObject.Name
                    List<Object> UnityObjectList = CastWholeList<Object>(listObj, typeof(Object));
                    Object obj = property.objectReferenceValue;

                    #region Draw the list dropdown

                    int SelectedID = FindSelectedID(UnityObjectList, obj);//Update the selectedID
                    string[] arr = ListToStringArray(UnityObjectList, GetItemName, BaseMaster);//convert the objects into name list using GetItemName delegate method
                    if(arr == null)
                    {//Cannot find property name
                        EditorGUI.LabelField(position, property.name, $"Cannot find property: \"[item].{dropdownAttribute.ItemNameProperty}\"", GetGUIColor(Color.red));
                        return;
                    }
                    if (SelectedID == -1 && arr.Length > 0) SelectedID = 0;//Set it to 0 as default
                    //int newSelectedID = EditorGUILayout.Popup(dropdown, SelectedID, arr);
                    int newSelectedID = EditorGUI.Popup(position, property.name, SelectedID, arr);
                    if (newSelectedID != SelectedID)
                    {//changed
                        SelectedID = newSelectedID;
                        Object selectedObject = UnityObjectList[SelectedID];
                        property.objectReferenceValue = selectedObject;
                        //EditorUtility.SetDirty(property.serializedObject.targetObject);//repaint
                        //Debug.Log($"changed to {property.objectReferenceValue.name}");
                    }

                    #endregion
                }
                else
                {//Use SystemObject.ToString()
                    List<object> SystemObjectList = CastWholeList<object>(listObj, typeof(object));
                    /*foreach(object o in SystemObjectList)
                    {
                        Debug.Log($" object tostring: {(o.ToString())}");
                        Debug.Log($" object name: {dropdownAttribute.GetItemName(o)}");
                    }*/
                    object obj = property.GetValue();
                    #region Draw the list dropdown

                    int SelectedID = FindSystemObjectSelectedID(SystemObjectList, obj, GetItemName, BaseMaster);//Update the selectedID
                    string[] arr = ListToStringArray(SystemObjectList, GetItemName, BaseMaster);
                    if (SelectedID == -1 && arr.Length > 0) SelectedID = 0;//Set it to 0 as default
                        int newSelectedID = EditorGUI.Popup(position, property.name, SelectedID, arr, GetDropdownStyle(Color.blue));
                    if (newSelectedID != SelectedID)
                    {//changed
                        SelectedID = newSelectedID;
                        object selectedObject = SystemObjectList[SelectedID];
                        property.SetValue(selectedObject);//property.setvalue to the serialized object

                        EditorUtility.SetDirty(property.serializedObject.targetObject);//repaint

                    }

                    #endregion

                }
            else
            {
                EditorGUI.LabelField(position, property.name, $"The list \"{dropdownAttribute.ListPath}\" size is 0", GetGUIColor(Color.red));
            }
        }catch(Exception e)
        {
            GUIStyle style = GetGUIColor(Color.red);
            style.wordWrap = true;//word wrap

            GUILayout.BeginHorizontal();

            GUILayout.Label(property.name+" [Dropdown ERROR] ");
            GUILayout.TextArea(e.ToString(), style , GUILayout.ExpandHeight(true));

            GUILayout.EndHorizontal();
            Debug.LogException(e);
        }

    }

}

#endif