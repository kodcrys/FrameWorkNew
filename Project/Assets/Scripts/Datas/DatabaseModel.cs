using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Reflection;
using BestHTTP;
using System.Text;
using UnityEngine.Events;

public class DataEventTrigger : UnityEvent<object>
{

}

public static class DataTrigger
{
    public static Dictionary<string, DataEventTrigger> dicOnValueChange = new Dictionary<string, DataEventTrigger>();
    public static void RegisterValueChange(string s, UnityAction<object> delegateDataChange)
    {
        if (dicOnValueChange.ContainsKey(s))
        {
            dicOnValueChange[s].RemoveListener(delegateDataChange);
            dicOnValueChange[s].AddListener(delegateDataChange);
        }
        else
        {
            dicOnValueChange.Add(s, new DataEventTrigger());
            dicOnValueChange[s].AddListener(delegateDataChange);
        }
    }

    public static void TriggerEventData(this object data, string path)
    {
        if(DataTrigger.dicOnValueChange.ContainsKey(path))
            DataTrigger.dicOnValueChange[path].Invoke(data);
    }
}

public class DatabaseModel : MonoBehaviour
{
    //[SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private HTTPManager httpManager;
    [SerializeField]
    private static string userID;
    public static string UserID {
        get => userID;
        set => userID = value;
    }
    [SerializeField]
    private bool isUseDataServer;

    public void LoadData(Action callback)
    {
        if (!isUseDataServer)
        {
            if (PlayerPrefs.HasKey("DATA"))
                GetLocalData();
            else
            {
                PlayerData pl = new PlayerData();
                CreateNewData(pl);
            }
            if (callback != null)
                callback();
        }
        else
        {
            GetServerData<PlayerData>(URLConfig.getAlldbAddress, string.Empty, false, false, (playerData) =>
            {
                if (playerData != null)
                {
                    this.playerData = playerData;
                    if (callback != null)
                        callback();
                }
                else
                    CreateNewDataServer(callback);
            });
        }
    }
    public void CreateNewData(PlayerData data)
    {
        playerData = data;

        string json = JsonConvert.SerializeObject(data, Formatting.None);
        if (!isUseDataServer)
            SaveLocalData();
    }
    public void CreateNewDataServer(Action callback)
    {
        playerData = new PlayerData();
        CreateNewData(playerData);
        UpdateObjectData<PlayerData>(URLConfig.setNewdbAddress, string.Empty, playerData, null, false, false, (respone) =>
        {
            if (callback != null)
                callback();
        });
    }

    public T Read<T>(string path)
    {
        object data = null;

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);

        ReadDataBypath(paths, playerData, out data);

        return (T)data;
    }

    public T Read<T>(string path, object key, bool useToKey = true)
    {
        object data = null;

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);

        ReadDataBypath(paths, playerData, out data);
        Dictionary<string, T> newDic = (Dictionary<string, T>)data;

        string keyStr = key.ToString();
        if (useToKey)
            keyStr = key.ToKey();

        if (newDic.ContainsKey(keyStr))
        {
            return newDic[keyStr];
        }

        return default(T);
    }
    private void ReadDataBypath(List<string> paths, object data, out object dataOut)
    {
        string p = paths[0];

        Type t = data.GetType();

        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {
            dataOut = field.GetValue(data);
        }
        else
        {
            paths.RemoveAt(0);
            ReadDataBypath(paths, field.GetValue(data), out dataOut);
        }

    }
    public void UpdateData(string path, object dataNew, Action callback)
    {

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);
        UpdateDataBypath(paths, playerData, dataNew, callback);
        SaveLocalData();

        dataNew.TriggerEventData(path);
    }
    private void UpdateDataBypath(List<string> paths, object data, object datanew, Action callback)
    {
        string p = paths[0];

        Type t = data.GetType();

        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {
            field.SetValue(data, datanew);
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            paths.RemoveAt(0);
            UpdateDataBypath(paths, field.GetValue(data), datanew, callback);
        }

    }
    public void UpdateData<TValue>(string path, object key, TValue dataNew, Action callback)
    {
        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);
        UpdateDataDicBypath(paths, playerData, key.ToString(), dataNew, callback);
        SaveLocalData();
        dataNew.TriggerEventData(path);
    }
    private void UpdateDataDicBypath<TValue>(List<string> paths, object data, string key, TValue dataNew, Action callback)
    {
        string p = paths[0];

        Type t = data.GetType();

        FieldInfo field = t.GetField(p);


        if (paths.Count == 1)
        {

            object dic = field.GetValue(data);

            Dictionary<string, TValue> newDic = (Dictionary<string, TValue>)dic;
            newDic[key] = dataNew;
            field.SetValue(data, newDic);
            if (callback != null)
            {
                callback();
            }

        }
        else
        {
            paths.RemoveAt(0);
            UpdateDataDicBypath(paths, field.GetValue(data), key, dataNew, callback);
        }

    }
    public void Delete()
    {

    }

    private void SaveLocalData()
    {
        string s = JsonConvert.SerializeObject(playerData, Formatting.None);
        PlayerPrefs.SetString("DATA", s);
    }
    private void GetLocalData()
    {
        string s = PlayerPrefs.GetString("DATA");
        playerData = JsonConvert.DeserializeObject<PlayerData>(s);
    }

    public void GetServerData<T>(string pathFunction, string pathDatabase, bool isShowLoading, bool isHideLoading, Action<T> callback)
    {  
        httpManager.HTTPPost(pathFunction, pathDatabase, string.Empty, isShowLoading, isHideLoading, (res) =>
        {
            T data = JsonConvert.DeserializeObject<T>(res.DataAsText);
            if (callback != null)
                callback(data);
        });
    }

    public void UpdateObjectData<T>(string pathFunction, string pathDatabase, T data, object key, bool isShowLoading, bool isHideLoading, Action<HTTPResponse> callback)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.None);
        string pathDatabaseServer = pathDatabase;
        if(key != null)
            pathDatabaseServer += "/" + key.ToString();

        httpManager.HTTPPost(pathFunction, pathDatabaseServer, jsonData, isShowLoading, isHideLoading, (res) =>
        {
            if (key != null)
                UpdateData<T>(pathDatabase, key, data, null);
            else if (pathDatabase != string.Empty)
            {
                UpdateData(pathDatabase, data, null);
            }
            if (callback != null)
                callback(res);
        });
    }
}
