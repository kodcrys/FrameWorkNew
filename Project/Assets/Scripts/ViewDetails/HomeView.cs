using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeView : BaseView
{
    public void BtnPlayGameHandle()
    {

    }

    public void BtnSlotSpinGameHandle()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.SlotSpinDialog);
    }

    public void BtnLuckySpinGameHandle()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.LuckySpinDialog);
    }
}
