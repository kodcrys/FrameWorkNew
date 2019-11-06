using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : SingletonMono<ConfigManager>
{
    private PrizeSlotConfig prizeSlotConfig;
    public PrizeSlotConfig PrizeSlotConfig { get => prizeSlotConfig; }

    private PrizeLuckySpinConfig prizeLuckySpinConfig;
    public PrizeLuckySpinConfig PrizeLuckySpinConfig { get => prizeLuckySpinConfig; }


    public void InitConfig(Action callback)
    {
        StartCoroutine(LoopLoadConfig(callback));
    }

    IEnumerator LoopLoadConfig(Action callback)
    {
        string addDress = "Configs/";

        yield return new WaitForSeconds(0.1f);

        prizeSlotConfig = Resources.Load(addDress + "PrizeSlotConfig", typeof(ScriptableObject)) as PrizeSlotConfig;
        yield return new WaitUntil(() => prizeSlotConfig != null);

        prizeLuckySpinConfig = Resources.Load(addDress + "PrizeLuckySpinConfig", typeof(ScriptableObject)) as PrizeLuckySpinConfig;
        yield return new WaitUntil(() => prizeLuckySpinConfig != null);

        if (callback != null)
            callback();
    }
}
