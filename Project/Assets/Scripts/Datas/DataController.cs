using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataController : MonoBehaviour
{
    [SerializeField]
    private DatabaseModel databaseModel;
    private PlayerData playerData;

    public void InitData(Action callback)
    {
        databaseModel.LoadData(callback);
    }

    public PlayerInfo GetPlayerInfo()
    {
        return databaseModel.Read<PlayerInfo>(PathData.playerInfo);
    }

    public void AddCoin(int coinAmount)
    {
        int currentCoin = GetCoin();
        currentCoin += coinAmount;

        databaseModel.UpdateData(PathData.coin, currentCoin, null);
    }

    public int GetCoin()
    {
        return databaseModel.Read<int>(PathData.coin);
    }
}
