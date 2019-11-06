using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrizeSlotConfigData
{
    public PrizeSlotName prizeName;
    public int value;
    public float rate;
}

public class PrizeSlotConfig : ConfigDataTable<PrizeSlotConfigData>
{
    public int Count { get => records.Count; }

    public override void AddKeySort()
    {
        OnAddKeySort(new ConfigComparePrimaryKey<PrizeSlotConfigData>("prizeName"));
    }

    public PrizeSlotConfigData GetPrize(PrizeSlotName prizeSlotName)
    {
        return records.Find(e => e.prizeName == prizeSlotName);
    }
}
