using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class ConfigCompare<T> : IComparer<T> where T : class
{
    public int Compare(T x, T y)
    {
        return ICompareHandle(x, y);
    }

    public abstract int ICompareHandle(T x, T y);
    public abstract T SetValueHandle(object searchValue);
}

public class ConfigComparePrimaryKey<T2> : ConfigCompare<T2> where T2 : class, new()
{
    private FieldInfo keyInfo;
    public ConfigComparePrimaryKey(string keyInfoName)
    {
        keyInfo = typeof(T2).GetField(keyInfoName);
    }

    public override int ICompareHandle(T2 x, T2 y)
    {
        object val_1 = keyInfo.GetValue(x);
        object val_2 = keyInfo.GetValue(y);
        if (val_1 == null && val_2 == null)
            return 0;
        else if (val_1 == null && val_2 != null)
            return -1;
        else if (val_1 != null && val_2 == null)
            return 1;
        else
            return ((IComparable)val_1).CompareTo(val_2);
    }

    public override T2 SetValueHandle(object searchValue)
    {
        T2 key = new T2();
        keyInfo.SetValue(key, searchValue);
        return key;
    }
}

public class ConfigDatabase : ScriptableObject
{
    public virtual void CreateBinaryFile(TextAsset csvFile)
    {

    }
}

public class ConfigDataTable<T> : ConfigDatabase where T:class, new()
{
    [SerializeField]
    public List<T> records;

    [SerializeField]
    private ConfigCompare<T> icompare;

    private void OnEnable()
    {
        AddKeySort();
    }

    public virtual void AddKeySort()
    {

    }

    public void OnAddKeySort(ConfigCompare<T> compare_)
    {
        icompare = compare_;
    }

    public override void CreateBinaryFile(TextAsset csvFile)
    {
        AddKeySort();
        OnParsejson(csvFile);
    }

    private void OnParsejson(TextAsset csvText)
    {
        if (records != null)
        {
            records.Clear();
        }
        else
        {
            records = new List<T>();
        }
        //1 split csv file to grids;
        List<List<string>> textData = SplitCSVFile(csvText.text);
        Type mType = typeof(T);
        FieldInfo[] fieldInfos = mType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        for (int i = 1; i < textData.Count; i++)
        {
            string jsonText = string.Empty;
            List<string> dataInput = textData[i];
            jsonText += "{";
            for (int x = 0; x < fieldInfos.Length; x++)
            {
                if (x > 0)
                {
                    jsonText += ",";
                }
                if (fieldInfos[x].FieldType != typeof(string))
                {
                    jsonText += "\"" + fieldInfos[x].Name + "\":" + dataInput[x];
                }
                else
                {
                    jsonText += "\"" + fieldInfos[x].Name + "\":\"" + dataInput[x] + "\"";
                }

            }
            jsonText += "}";
            Debug.LogError(jsonText);
            T data = JsonUtility.FromJson<T>(jsonText);
            records.Add(data);
        }

        records.Sort(icompare);

    }

    private List<List<string>> SplitCSVFile(string csvText)
    {
        List<List<string>> grids = new List<List<string>>();
        string[] lines = csvText.Split('\n');

        foreach (string e in lines)
        {
            //1
            string[] fields = SplitCsvLine(e);
            List<string> ls = new List<string>();
            foreach (string eField in fields)
            {
                //2
                eField.Replace("\"\"", "\"");
                ls.Add(eField);
            }
            grids.Add(ls);
        }
        return grids;
    }

    // split a csv row
    static public string[] SplitCsvLine(string line)
    {
        return 
        (
            from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches
            (
                line, @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                System.Text.RegularExpressions.RegexOptions.ExplicitCapture
            )
            select m.Groups[1].Value
        ).ToArray();
    }

    public T GetRecordByKey(object value)
    {

        T key = icompare.SetValueHandle(value);

        int index = records.BinarySearch(key, icompare);

        return CopyData2(records[index], key);
    }

    private T CopyData(object data)
    {
        string s = JsonUtility.ToJson(data);

        return JsonUtility.FromJson<T>(s);
    }
    private T CopyData2(object data, T dataOut)
    {

        Type type = typeof(T);
        FieldInfo[] members = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        for (int i = 1; i < members.Length; i++)
        {
            FieldInfo fieldInfo = members[i];
            var inputData = fieldInfo.GetValue(data);

            fieldInfo.SetValue(dataOut, inputData);
        }
        return dataOut;
    }
}