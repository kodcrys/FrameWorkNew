using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LuckySpinDialog : BaseDialog
{
    private bool isFirstSetUp = true;

    private PrizeLuckyItem[] prizeLuckyItems;
    private float[] ratioPrizeSumList;

    [SerializeField]
    private Transform spin;
    [SerializeField]
    private Button spinBtn;
    [SerializeField]
    private Button backBtn;

    public override void OnSetUp(DialogParam param = null)
    {
        if(isFirstSetUp)
        {
            ratioPrizeSumList = new float[ConfigManager.Instance.PrizeLuckySpinConfig.Count];
            ratioPrizeSumList[0] = ConfigManager.Instance.PrizeLuckySpinConfig.GetPrize(0).rate;

            for (int i = 1; i < ratioPrizeSumList.Length; i++)
            {
                float rateI = ConfigManager.Instance.PrizeLuckySpinConfig.GetPrize(i).rate;
                float rate = ratioPrizeSumList[i - 1] + rateI;
                if (i == ratioPrizeSumList.Length - 1)
                    rate = Mathf.Round(rate);

                ratioPrizeSumList[i] = rate;
            }

            prizeLuckyItems = GetComponentsInChildren<PrizeLuckyItem>();

            isFirstSetUp = false;
        }

        for(int i = 0; i<prizeLuckyItems.Length; i++)
        {
            PrizeLuckySpinConfigData config = ConfigManager.Instance.PrizeLuckySpinConfig.GetPrize(i);
            prizeLuckyItems[i].SetUp(config);
        }
        spin.transform.eulerAngles = Vector3.zero;

        spinBtn.interactable = true;
        backBtn.interactable = true;

        base.OnSetUp(param);
    }

    public void BtnBackHandle()
    {
        DialogManager.Instance.HideDialog(this);
    }

    public void BtnSpinHandle()
    {
        spinBtn.interactable = false;
        backBtn.interactable = false;

        var prizeRatio = Random.Range(1, ratioPrizeSumList[ratioPrizeSumList.Length - 1]);
        int prizeIndex = 0;
        for (prizeIndex = 0; prizeRatio > ratioPrizeSumList[prizeIndex]; prizeIndex++) ;

        //float curAngle = spin.localEulerAngles.z;
        float randAngle = (prizeIndex - ratioPrizeSumList.Length) * 360 / ratioPrizeSumList.Length;

        float offsetAngle = Random.Range(0, 12f);

        int randTemp = Random.Range(0, 100);
        if (randTemp % 2 == 0)
            randAngle -= offsetAngle;
        else
            randAngle += offsetAngle;

        int randRound = Random.Range(6, 11);
        float rotateAngle = /*-curAngle +*/ randRound * -360.0f + (randAngle - 360);

        PrizeLuckySpinConfigData prizeConfig = ConfigManager.Instance.PrizeLuckySpinConfig.GetPrize(prizeIndex);

        Vector3 rotateVector = new Vector3(0, 0, rotateAngle);
        
        spin.DORotate(rotateVector, 4.5f, RotateMode.FastBeyond360).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            Transform effect = PoolManager.dicPools[GameDefines.POOL_COIN_EFFECT].GetObjectInstance();
            effect.SetParent(this.transform, false);
            effect.position = Vector3.zero;
            EffectCoin effCoin = effect.GetComponent<EffectCoin>();
            effCoin.OnEndEffect += () =>
            {
                DataAPIManager.Instance.AddCoin(prizeConfig.value);
                spinBtn.interactable = true;
                backBtn.interactable = true;
                effCoin.OnEndEffect = null;
            };

            effCoin.SetUp(prizeConfig.value);
        });
    }
}
