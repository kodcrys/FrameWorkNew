using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TopBarManager : SingletonMono<TopBarManager>
{
    [Header("----------- Gold Bar -----------")]
    [SerializeField] private Text valueGoldTxt;

    [Header("----------- Top Bar Object -----------")]
    [SerializeField] private Transform topBarObj;
    [SerializeField] private Transform topBarShowPos;
    [SerializeField] private Transform topBarHidePos;

    [Header("----------- Variable -----------")]
    [SerializeField] private float timeAnim = 0.7f;
    [SerializeField] private Ease easeAnim = Ease.Linear;

    public void SetUp()
    {
        topBarObj.position = topBarHidePos.position;

        DataTrigger.RegisterValueChange(PathData.coin, (obj) =>
        {
            int coin = (int)obj;
            valueGoldTxt.text = coin.ToString();
        });
    }

    public void Show(Action callback = null)
    {
        valueGoldTxt.text = DataAPIManager.Instance.GetCoin().ToString();

        topBarObj.DOMoveY(topBarShowPos.position.y, timeAnim).SetEase(easeAnim).OnComplete(() => {
            if (callback != null)
                callback();
        });
    }

    public void Hide(Action callback = null)
    {
        topBarObj.DOMoveY(topBarHidePos.position.y, timeAnim).SetEase(easeAnim).OnComplete(() => {
            if (callback != null)
                callback();
        });
    }
}
