using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilitiesHandle
{
    public static string ToKey(this object data)
    {
        return "K_" + data.ToString();
    }

    public static int ToID(this string data)
    {
        string tempData = data.Remove(0, 2);
        try
        {
            int id = int.Parse(tempData);
            return id;
        }
        catch
        {
            return -1;
        }
    }

    public static List<T> GetListByString<T>(this string data, char key)
    {
        string[] s = data.Split(key);
        List<T> lst = new List<T>();
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = s[i].Trim();
            T temp = (T)Convert.ChangeType(s[i], typeof(T));
            lst.Add(temp);
        }
        return lst;
    }

    public static List<T> GetListEnumByString<T>(this string data, char key)
    {
        string[] s = data.Split(key);
        List<T> lst = new List<T>();
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = s[i].Trim();
            T temp = s[i].ToEnum<T>();
            lst.Add(temp);
        }
        return lst;
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
