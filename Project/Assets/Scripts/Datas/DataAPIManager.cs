using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataAPIManager : SingletonMono<DataAPIManager>
{
    [SerializeField]
    private DataController dataController; 

    public void InitData(Action callback)
    {
        dataController.InitData(callback);
    }

    public void AddCoin(int goldAmount)
    {
        dataController.AddCoin(goldAmount);
    }

    public int GetCoin()
    {
        return dataController.GetCoin();
    }
}

