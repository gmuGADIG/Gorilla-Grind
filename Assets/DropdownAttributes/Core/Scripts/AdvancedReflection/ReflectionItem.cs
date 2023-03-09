using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class ReflectionItem
{
    public ReflectionItemType ReflectionItemType;
    public object BaseMaster;//the first master object.
    public object Master;//the master object
    public Type MasterType;//the master object
    public string Target;//the Target variable name
    private void debug(object obj)
    {
        if (ReflectionSystem.DebugMode)
        {
            Debug.Log(obj);
        }
    }

    public ReflectionItem(object baseMaster, object master, string target)
    {
        BaseMaster = baseMaster;
        Master = master;
        MasterType = master.GetType();
        Target = target;
        ReflectionItemType = DetectReflectionItemType(target);//Detect the type first
        debug($"<b><color=teal>[Reflection Progress]</color></b> Master class: <b>{MasterType.Name}</b><color=olive>(INSTANCE)</color> {ReflectionSystem.arrowSymbol} Target: <b>{Target}</b><color=olive>({ReflectionItemType})</color> \n");
    }
    public ReflectionItem(object baseMaster, Type masterType, string target)
    {

        BaseMaster = baseMaster;
        MasterType = masterType;
        Target = target;
        ReflectionItemType = DetectReflectionItemType(target);//Detect the type first
        debug($"<b><color=teal>[Reflection Progress]</color></b> Master class: <b>{MasterType.Name}</b><color=olive>(STATIC CLASS)</color> {ReflectionSystem.arrowSymbol} Target: <b>{Target}</b><color=olive>({ReflectionItemType})</color> \n");
    }
    private ReflectionItemType DetectReflectionItemType(string Target)
    {
        if (Target.Contains("(") && Target.Contains(")"))
        {//if GetListFromMap()[0], it would be detected as method first
            if (Target.Contains(")["))
            {//GetListFromMap()[0]
                return ReflectionItemType.MethodAndCollection;
            }
            return ReflectionItemType.METHOD;
        }
        if (Target.Contains("[") && Target.Contains("]"))
        {
            return ReflectionItemType.COLLECTION;
        }
        if (DetectPrimaryTypeFromString(Target)!=null)
        {//It is primary type
            return ReflectionItemType.PRIMITIVEINSTANCE;
        }
        //It is instance or class
        object instanceVariable = FindInstanceVariable(Target);
        Type staticClass = FindStaticClass(Target);

        bool couldBeInstance = (instanceVariable != null);
        bool couldBeClass = (staticClass != null);


        if(couldBeInstance && couldBeClass)
        {//WARNING
            Debug.LogError($"Reflection confused. the target {Target} in {MasterType} could be instance or class at the same time!");
            return ReflectionItemType.InstanceOrClass;
        }

        if (couldBeInstance) return ReflectionItemType.INSTANCE;

        if (couldBeClass) return ReflectionItemType.CLASS;

        return ReflectionItemType.NULL;

    }
    public object GetValue()
    {
        try
        {
            switch (ReflectionItemType)
            {
                case ReflectionItemType.INSTANCE:
                    return FindInstanceVariable(Target);
                case ReflectionItemType.CLASS:
                    Debug.LogError("Please use GetStaticClass() instead of GetValue() if the ReflectionItemType is Class");
                    return FindStaticClass(Target);
                case ReflectionItemType.METHOD:
                    return FindMethodResult();
                case ReflectionItemType.COLLECTION:
                    return FindCollectionItem(Target);
                case ReflectionItemType.MethodAndCollection://GetListFromMap(0)[0]
                    return FindMethodAndCollectionItem(Target);
                case ReflectionItemType.PRIMITIVEINSTANCE://e.g. int, long, short, float, double, char, string
                    return StringToPrimaryInstance(Target);

            }
        }
        catch (Exception e) {
            if (ReflectionSystem.DebugMode)
                Debug.LogException(e);
            return null;
        }
        return null;
    }
    public Type GetStaticClass()
    {
        return FindStaticClass(Target);
    }

    #region Finding Utils
    #region Find Instance Variable
    private object FindInstanceVariable(string Target)
    {
        //Try by field
        object resultByField = FindInstanceVariableByField(Target);
        if (resultByField != null) return resultByField;

        //Try by property
        object resultByProperty = FindInstanceVariableByProperty(Target);
        if (resultByProperty != null) return resultByProperty;

        return null;
    }
    private object FindInstanceVariableByField(string Target)
    {
        Type currentType = MasterType;
        FieldInfo info = currentType?.GetFieldBypassProtection(Target);
        while(info == null)
        {
            if (currentType == null) return null;
            currentType = currentType.BaseType;
            info = currentType?.GetFieldBypassProtection(Target);//get the field
        }
        //debug($"info {info.Name}");
        return info.GetValue(Master);
        
    }
    private object FindInstanceVariableByProperty(string Target)
    {
        Type currentType = MasterType;
        PropertyInfo info = currentType?.GetPropertyBypassProtection(Target);
        while (info == null)
        {
            if (currentType == null) return null;
            currentType = currentType.BaseType;
            info = currentType?.GetPropertyBypassProtection(Target);
        }
        return info.GetValue(Master);

    }
    #endregion
    #region Find Static Class
    private Type FindStaticClass(string Target)
    {
        return Type.GetType(Target);
    }
    #endregion
    #region Find Method Result (Fixing)
    private object FindMethodResult()
    {
        string MethodName = GetMethodNameFromTarget();
        object[] parameters = GetParametersFromTarget();
        if (MethodName == "ToString") return Master.ToString();
        MethodInfo info = null;
        foreach(MethodInfo i in MasterType?.GetMethods())
        {
            if (MethodName == i.Name) info = i;
        }
        //Debug.Log($"try: {MethodName}  found method: {info.Name}");
        //MethodInfo info = MasterType?.GetMethod(MethodName);
        if (info == null) return null;
        object result = info.Invoke(Master, parameters);
        debug($"<color=darkblue><b>[Invoke method]</b></color> <b>{MasterType.Name}.{Target}</b>  Result: <b>{result.ToStringWithQuotes()}</b>\n");

        return result;
    }
    public string GetMethodNameFromTarget()
    {
        return Target.Split('(')[0];//"Fuck(`you`, `your mom`)"  ->  "Fuck"
    }
    private object ParseToType(object obj, Type type)
    {
        var converter = TypeDescriptor.GetConverter(type);
        var result = converter.ConvertFrom(obj);
        return result;
    }
    public object[] GetParametersFromTarget()
    {
        string parameterPart = Target.Split(new[] { '(' }, 2)[1];//"GetName(minecraft, getPlayer(world, uuid))"   ->   "minecraft, getPlayer(world, uuid))"
        parameterPart = parameterPart.Substring(0, parameterPart.Length - 1);//"minecraft, getPlayer(world, uuid))"   ->   "minecraft, getPlayer(world, uuid)"
        parameterPart = ReflectionExtensions.RemoveAllCharacterThatNotInBrackets(parameterPart, ' ');
        //debug($"Trimmed parameter part: >{parameterPart}<");
        string[] stringParameterArray = ReflectionExtensions.SplitIgnoringSplitterInBrackets(parameterPart, ',');
        object[] result = new object[stringParameterArray.Length];
        for (int i = 0 ; i < stringParameterArray.Length; i++)
        {//turrning string array into object array
            string str = stringParameterArray[i];
            if (ReflectionExtensions.MathCharacterNotInBracketsExists(str))
            {//The user try to do the math.
                Debug.LogError($"Currently mathematical symbol could not be used, Wrong path:  \"{Target}\" ");
            }
            Type primaryType = DetectPrimaryTypeFromString(str);
            if (primaryType != null)
            {
                result[i] = StringToPrimaryInstance(str, primaryType);
                continue;
            }
            else
            {//It is not primary type, try object value
                object objValue = ReflectionSystem.GetValue(BaseMaster, str);
                if (objValue != null)
                {//it is object
                    result[i] = objValue;
                    continue;
                }
            }
            result[i] = null;


        }
        return result;
    }
    private object StringToPrimaryInstance(string str)
    {
        Type primaryType = DetectPrimaryTypeFromString(str);
        return StringToPrimaryInstance(str, primaryType);
    }
    private object StringToPrimaryInstance(string str, Type primaryType)
    {
        if (primaryType == typeof(string))
        {
            return str.Substring(1, str.Length - 2);//remove the quotes "
        }
        else if (primaryType == typeof(char))
        {
            return str.Substring(1, str.Length - 2).ToCharArray()[0];//remove the quotes '
        }
        else if (primaryType != null)
        {//Parse if it is primary type
            return ParseToType(str, primaryType);//parse it from "3.53f" to 3.53(float)
        }
        return null;
    }
    private Type DetectPrimaryTypeFromString(string str)
    {
        if (str.Length > 0) if (str[0] == '"' && str[str.Length-1] == '"')
            {//it is a string
                return typeof(string);
            }
        if (str.Length > 0) if (str[0] == '\'' && str[str.Length - 1] == '\'')
            {//it is a string
                return typeof(char);
            }
        if (int.TryParse(str, out int i))
        {
            return typeof(int);
        }
        if (short.TryParse(str, out short s))
        {
            return typeof(short);
        }
        if (long.TryParse(str, out long l))
        {
            return typeof(long);
        }
        if (bool.TryParse(str, out bool b))
        {
            return typeof(bool);
        }
        if (double.TryParse(str, out double d))
        {
            return typeof(double);
        }
        if (float.TryParse(str, out float f))
        {
            return typeof(float);
        }
        return null;
    }
    #endregion
    #region Find Collection Item
    private object FindCollectionItem(string Target)
    {
        string collectionName = GetCollectionNameFromTarget();
        object collectionObject = FindInstanceVariable(collectionName);//SkillList
        int[] collectionIndexes = GetCollectionIndexesFromTarget();//[0,2,5]
        /*Debug.Log($"collection index length: {collectionIndexes.Length.ToString()}");
        foreach(int i in collectionIndexes)
        {
            Debug.Log($"collection index: {i}");
        }*/
        return FindCollectionItem(collectionObject, collectionIndexes);


    }
    private object FindCollectionItem(object collectionObject, int[] collectionIndexes)
    {
        object currentCollectionObject = collectionObject;
        //Debug.Log($"collection BBB length: {collectionIndexes.Length}");
        for (int i = 0; i < collectionIndexes.Length; i++)
        {
            int currentIndex = collectionIndexes[i];
            //Debug.Log($"collection BBB current index: {currentIndex}");
            currentCollectionObject = GetValueInCollection(currentCollectionObject, currentIndex);
        } 
        return currentCollectionObject;


    }
    private object GetValueInCollection(object collectionObject, int index)
    {//could be "aa"[0] or List/array [0]
        //debug($"is array: {collectionObject.GetType().IsArray},  type: {collectionObject.GetType()}");
        if (collectionObject is string)
        {
            //Debug.Log($"collection is string: {collectionObject.ToString()}");
            return collectionObject.ToString()[index];
        }
        else
        {
            IList list = (IList)collectionObject;
            //debug($"Getting value in collection, length: {list.Count}");
            return list[index];
        }
    }
    private string GetCollectionNameFromTarget()
    {
        return Target.Split('[')[0];
    }
    private int[] CollectionPartStringToIntArray(string collectionPart)
    {//Assume input: "[0][3][2]", Output: [0, 3, 2]
        //Debug.Log($"collection part input: {collectionPart}");
        collectionPart = collectionPart.Substring(1, collectionPart.Length - 2);//"[0][3][2]"   ->   "0][3][2"
        string[] stringArray = collectionPart.Split(new string[] { "][" }, StringSplitOptions.None);//"0][3][2"   ->   ["0","3","2"]
        int[] result = new int[stringArray.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = int.Parse(stringArray[i]);
        }
        return result;
    }
    private int[] GetCollectionIndexesFromTarget()
    {
        string collectionPart = Target.Split(new [] {'['}, 2)[1];//"list[0][3][2]"   ->   "0][3][2]"
        collectionPart = "[" + collectionPart; // "0][3][2]" -> "[0][3][2]"
        //Debug.Log($"collection part: {collectionPart}");
        return CollectionPartStringToIntArray(collectionPart);
        
    }
    #endregion
    #endregion
    #region Find Method with collection item
    //Example: GetListFromMap(0)[0]
    private object FindMethodAndCollectionItem(string Target)
    {//Assume input: "GetListFromMap(0)[0]"
        string[] SplittedArray = Target.SplitIgnoringSplitterInBrackets(')');//["GetListFromMap(0", "[0]"]
        if (SplittedArray.Length != 2) return null;
        string MethodPart = SplittedArray[0]+")";//"GetListFromMap(0)"
        string CollectionPart = SplittedArray[1];//"[0]"
        object MethodResult = ReflectionSystem.GetValue(BaseMaster, Master, MethodPart);
        if (MethodResult == null) return null;
        int[] collectionIndexes = CollectionPartStringToIntArray(CollectionPart);
        return FindCollectionItem(MethodResult, collectionIndexes);

    }
    #endregion
}
