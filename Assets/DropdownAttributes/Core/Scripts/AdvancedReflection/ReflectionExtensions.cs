using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ReflectionExtensions
{
    public static string ToStringWithQuotes(this object obj)
    {
        //"hi"  ->  " \"hi\" " but not "hi"
        //'h' -> "'h'" but not "h"
        Type type = obj?.GetType();
        string result = obj?.ToString();
        if (type == typeof(string))
            return $"\"{result}\"";
        else if (type == typeof(char))
            return $"'{result}'";
        else
            return result;
    }
    public static FieldInfo GetFieldBypassProtection(this Type type, string fieldName)
    {//even can get private field
        FieldInfo field = null;

        do
        {
            field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Default | BindingFlags.Static);
            type = type.BaseType;

        } while (field == null && type != null);

        return field;
    }
    public static PropertyInfo GetPropertyBypassProtection(this Type type, string fieldName)
    {//even can get private
        PropertyInfo property = null;

        do
        {
            property = type.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Default | BindingFlags.Static);
            type = type.BaseType;

        } while (property == null && type != null);

        return property;
    }
    #region Method splitting
    public static string[] SplitIgnoringSplitterInBrackets(this string str, char splitter)
    {//split ignoring splitter in brackets, the splitter would disappear after splitting
        if (str.Trim().Length <= 0) return new string[0];//no arguments
        List<int> splitIndexList = new List<int>();//the list of split points
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (c == splitter)
            {//check if it is not in a bracket
                if (!IndexIsInBrackets(str, i))
                {
                    splitIndexList.Add(i);
                    //Debug.Log($"will split at: {i} in \"{str}\"");
                }
            }
        }
        return SplitAt(str, splitIndexList.ToArray());
    }
    public static string[] SplitAt(string source, params int[] index)
    {
        index = index.Distinct().OrderBy(x => x).ToArray();
        string[] output = new string[index.Length + 1];
        int pos = -1;

        for (int i = 0; i < index.Length; pos = index[i++])
        {
            output[i] = source.Substring(pos + 1, index[i] - pos - 1);
        }

        output[index.Length] = source.Substring(pos + 1);
        return output;
    }

    public static bool IndexIsInBrackets(string str, int index)
    {
        bool bracket1Openned = false;//  (
        bool bracket2Openned = false;//  [
        bool bracket3Openned = false;//  {
        bool bracket4Openned = false;//  "
        bool bracket5Openned = false;//  '
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            switch (c)
            {
                case '(':
                    bracket1Openned = true;
                    break;
                case ')':
                    bracket1Openned = false;
                    break;
                case '[':
                    bracket2Openned = true;
                    break;
                case ']':
                    bracket2Openned = false;
                    break;
                case '{':
                    bracket3Openned = true;
                    break;
                case '}':
                    bracket3Openned = false;
                    break;
                case '"':
                    bracket4Openned = bracket4Openned ? false : true;
                    break;
                case '\'':
                    bracket5Openned = bracket5Openned ? false : true;
                    break;
            }
            if (i == index)
            {//check if it is in a bracket
                return bracket1Openned || bracket2Openned || bracket3Openned || bracket4Openned || bracket5Openned;
            }
        }
        return false;
    }
    public static bool AllCharacterAreInBrackets(string str, char c)
    {
        //AllCharacterAreInBrackets("a + b", '+')  ->  false
        //AllCharacterAreInBrackets(" \"a + b\" ", '+')  ->  true
        //AllCharacterAreInBrackets(" \"a + b\" + \"c\" ", '+')  ->  false
        //AllCharacterAreInBrackets(" (\"a + b\" + \"c\" )", '+')  ->  true
        for (int i = 0; i < str.Length; i++)
        {
            char checkingChar = str[i];
            if (checkingChar == c)
            {
                if (!IndexIsInBrackets(str, i))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static bool MathCharacterNotInBracketsExists(string str)
    {
        //MathCharacterNotInBracketsExists("a + b", '+')  ->  true
        //MathCharacterNotInBracketsExists(" \"a + b\" ", '+')  ->  false
        //MathCharacterNotInBracketsExists(" \"a + b\" + \"c\" ", '+')  ->  true
        //MathCharacterNotInBracketsExists(" (\"a + b\" + \"c\" )", '+')  ->  false
        return !AllCharacterAreInBrackets(str, '+') || !AllCharacterAreInBrackets(str, '-') || !AllCharacterAreInBrackets(str, '*') || !AllCharacterAreInBrackets(str, '/') || !AllCharacterAreInBrackets(str, '=') || !AllCharacterAreInBrackets(str, '%') || !AllCharacterAreInBrackets(str, '^');
    }
    public static string RemoveAllCharacterThatNotInBrackets(string str, char c)
    {
        //RemoveAllCharacterThatNotInBrackets("a + b", '+')  ->  "a  b"
        //RemoveAllCharacterThatNotInBrackets(" \"a + b\" ", '+')  ->  " \"a + b\" "
        //RemoveAllCharacterThatNotInBrackets("map.getValue(5  , 3)    ", ' ')  ->  "map.getValue(5  , 3)"
        //RemoveAllCharacterThatNotInBrackets("  5  , 3    ", ' ')  ->  "5,3"

        //imagin "12345", take out '2', 
        string result = str;
        for (int i = str.Length - 1; i > 0; i--)
        {//trying 5 -> 4 -> 3 -> 2
            char checkingChar = str[i];
            if (checkingChar == c)
            {//c == '2'
                if (!IndexIsInBrackets(str, i))
                {//'2' is not in a bracket, i == 1
                    result = result.Substring(0, i) + result.Substring(i + 1, result.Length - (i + 1));//"12345" -> "1"+"345"
                    //Debug.Log($"removing progess >{result}<");
                }
            }
        }
        return result;
    }
    #endregion

}
