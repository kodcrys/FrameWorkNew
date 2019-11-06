using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlotSpinDialog : BaseDialog
{
    [SerializeField] private Button slotSpinBtn;
    [SerializeField] private Button backBtn;

    [SerializeField] private Row[] rowSpin;
    [SerializeField] private PrizeSlotConfigData[] prizeEachRow;

    private bool isFirstSetUp = true;
    private bool availabeSpin = true;

    [SerializeField]
    private float[] ratioPrizeSumList;

    public override void OnSetUp(DialogParam param = null)
    {
        // Set up rate of Prize
        if (isFirstSetUp)
        {
            ratioPrizeSumList = new float[ConfigManager.Instance.PrizeSlotConfig.Count];
            ratioPrizeSumList[0] = ConfigManager.Instance.PrizeSlotConfig.GetPrize((PrizeSlotName)0).rate;

            for (int i = 1; i < ratioPrizeSumList.Length; i++)
            {
                float rateI = ConfigManager.Instance.PrizeSlotConfig.GetPrize((PrizeSlotName)(i)).rate;
                float rate = ratioPrizeSumList[i - 1] + rateI;
                if (i == ratioPrizeSumList.Length - 1)
                    rate = Mathf.Round(rate);

                ratioPrizeSumList[i] = rate;
            }

            isFirstSetUp = false;
        }

        slotSpinBtn.interactable = true;
        backBtn.interactable = true;

        availabeSpin = true;
        for (int i = 0; i < rowSpin.Length; i++)
            rowSpin[i].SetUp();

        base.OnSetUp(param);
    }

    public void RotateSpin(Action callback = null)
    {
        if (!availabeSpin)
            return;

        availabeSpin = false;
        prizeEachRow = RandomSprize();

        StopCoroutine("Spin");
        StartCoroutine(Spin(prizeEachRow, () =>
        {
            availabeSpin = true;
            if (callback != null)
                callback();
        }));
    }

    private PrizeSlotConfigData[] RandomSprize()
    {
        PrizeSlotConfigData[] allPrize = new PrizeSlotConfigData[rowSpin.Length];
        for (int i = 0; i < rowSpin.Length; i++)
        {
            int prizeRatio = Random.Range(1, (int)ratioPrizeSumList[ratioPrizeSumList.Length - 1]);
            int prizeIndex = 0;
            for (prizeIndex = 0; prizeRatio > ratioPrizeSumList[prizeIndex]; prizeIndex++) ;

            allPrize[i] = ConfigManager.Instance.PrizeSlotConfig.GetPrize((PrizeSlotName)prizeIndex);
        }
        return allPrize;
    }

    private IEnumerator Spin(PrizeSlotConfigData[] prizesSlot, Action callback)
    {
        int countFinsh = 0;
        for (int i = 0; i < rowSpin.Length; i++)
        {
            rowSpin[i].RotatePlay(prizesSlot[i], () =>
            {
                countFinsh++;
            });
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitUntil(() => countFinsh >= rowSpin.Length);

        if (callback != null)
            callback();
    }

    public void SlotSpinBtnHandle()
    {
        slotSpinBtn.interactable = false;
        backBtn.interactable = false;
        RotateSpin(() =>
        {
            slotSpinBtn.interactable = true;
            backBtn.interactable = true;
        });
    }

    public void BtnBackHandle()
    {
        DialogManager.Instance.HideDialog(this);
    }
}
