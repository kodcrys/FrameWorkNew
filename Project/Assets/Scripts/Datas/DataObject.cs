using System;
using System.Collections;
using System.Collections.Generic;

public class DataObject 
{
    public string uuid;

    public DataObject()
    {
        uuid = Guid.NewGuid().ToString();
    }
}

[Serializable]
public class PlayerData : DataObject
{
    public PlayerInfo playerInfo = new PlayerInfo();
    public PlayerInventory playerInventory = new PlayerInventory();
}

[Serializable]
public class PlayerInfo
{
    public string name;
    public int level;
    public int exp;

    public PlayerInfo()
    {
        this.name = "Player";
        this.level = 1;
        this.exp = 0;
    }
}

[Serializable]
public class PlayerInventory
{
    public int coin;
    public int cash;

    public PlayerInventory()
    {
        this.coin = 0;
        this.cash = 0;
    }
}