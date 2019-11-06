using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrizeLuckySpinConfigData
{
    public PrizeLuckySpinName prizeName;
    public int value;
    public float rate;
}

public class PrizeLuckySpinConfig : ConfigDataTable<PrizeLuckySpinConfigData>
{
    public int Count { get => records.Count; }

    public override void AddKeySort()
    {
        OnAddKeySort(new ConfigComparePrimaryKey<PrizeLuckySpinConfigData>("prizeName"));
    }

    public PrizeLuckySpinConfigData GetPrize(int index)
    {
        if (index >= records.Count)
            return null;

        return records[index];
    }

    public PrizeLuckySpinConfigData GetPrize(PrizeLuckySpinName prizeName)
    {
        return records.Find(e => e.prizeName == prizeName);
    }
}
