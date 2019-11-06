using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingView : BaseView
{
    public Image progess;

    public override void OnSetUp(ViewParam param = null, Action callback = null)
    {
        progess.fillAmount = 0;
        progess.DOKill();
        base.OnSetUp(param, callback);
    }

    public void OnUpdateProgess(float value, float time = 0.3f, Action callback = null)
    {
        float valueTarget = value / 100;
        progess.DOFillAmount(valueTarget, time).OnComplete(() =>
        {
            progess.fillAmount = value;
            if (callback != null)
                callback();
        });
    }

    public override void OnHide(Action callback)
    {
        base.OnHide(callback);
    }
}
