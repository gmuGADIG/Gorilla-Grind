using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReflectionSystem
{
    public static string arrowSymbol = " → ";
    #region DebugMode
    public static bool DebugMode = false;
    public static void DebugModeStart()
    {
        EnableDebugMode();
    }
    public static void DebugModeEnd()
    {
        DisableDebugMode();
    }

    public static void EnableDebugMode()
    {
        DebugMode = true;
    }
    public static void DisableDebugMode()
    {
        DebugMode = false;
    }
    private static void debug(object obj)
    {
        if (DebugMode)
        {
            Debug.Log(obj);
        }
    }
    #endregion
    public static object GetValue(object baseMaster, object currentMaster, string path)
    {//ReflectionSystem.GetValue(baseMaster, this, "SkillDatabase.Instance.SkillList[0].skillID");
        return GetValue(baseMaster, currentMaster, ReflectionExtensions.SplitIgnoringSplitterInBrackets(path, '.'));
    }
    public static object GetValue(object baseMaster, string path)
    {//ReflectionSystem.GetValue(this, "SkillDatabase.Instance.SkillList[0].skillID");
        return GetValue(baseMaster, baseMaster, ReflectionExtensions.SplitIgnoringSplitterInBrackets(path, '.'));
    }
    private static object GetValue(object baseMaster, object currentMaster, params string[] args)
    {//ReflectionSystem.GetValue("SkillDatabase", "Instance", "SkillList[0]", "skillID");
        //Debug.Log("args length: " + args.Length.ToString());
        #region Display path way
        string display = "<color=blue><b>↓↓↓↓↓↓↓↓START Reflection↓↓↓↓↓↓↓↓</b></color> \n PATH:    <b>";
        display += $"{baseMaster.ToString()}{arrowSymbol}...{arrowSymbol}{currentMaster.ToString()}{arrowSymbol}";
        foreach (string str in args)
        {
            display += str + arrowSymbol;
        }
        display = display.Substring(0, display.Length-arrowSymbol.Length); //remove the arrow
        display += "</b>";
        debug(display);
        #endregion

        //Trying to know if the SkillDatabase is instance var or static class
        Type currentMasterType = currentMaster.GetType();

        ReflectionItem reflectionItem;
        string currentPath = "";
        for (int i = 0; i < args.Length; i++)
        {
            string target = args[i];
            if (i != 0) currentPath += ".";//first one dun need to add dot "."
            currentPath += target;
            if (currentMaster == null)
            {//static class
                reflectionItem = new ReflectionItem(baseMaster, currentMasterType, target);
            }
            else
            {//instance variable
                reflectionItem = new ReflectionItem(baseMaster, currentMaster, target);
            }

            if (reflectionItem.ReflectionItemType == ReflectionItemType.CLASS)
            {//static class
                currentMaster = null;//Don't have master for static class
                currentMasterType = reflectionItem.GetStaticClass();
            }
            else
            {//instance variable
                /*try
                {*/
                    currentMaster = reflectionItem.GetValue();
                /*}
                catch (Exception e)
                {
                    Debug.LogError($"Reflection Exception: {currentPath}   \nDetails: {e.ToString()}");
                }*/
            }

        }


        //if (currentMaster == null) return (object)currentMasterType;
        debug($"<color=green><b>[RESULT]</b></color> <b> {currentMaster.ToStringWithQuotes()} </b>\n");
        debug("<color=maroon><b>↑↑↑↑↑↑↑↑END Reflection↑↑↑↑↑↑↑↑</b></color>\n");
        return currentMaster;
    }
}
