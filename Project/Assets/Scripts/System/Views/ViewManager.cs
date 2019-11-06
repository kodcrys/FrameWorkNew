using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : SingletonMono<ViewManager>
{
    public Dictionary<ViewIndex, BaseView> dicView = new Dictionary<ViewIndex, BaseView>();

    public BaseView currentView;

    public void SetUpCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }

    public void Init(Action callback = null)
    {
        foreach(ViewIndex e in ViewConfig.viewIndices)
        {
            string nameView = e.ToString();

            GameObject goView = Instantiate(Resources.Load("Views/" + nameView, typeof(GameObject))) as GameObject;
            goView.transform.SetParent(transform, false);

            RectTransform rectTrans = goView.GetComponent<RectTransform>();
            rectTrans.offsetMax = rectTrans.offsetMin = Vector2.zero;

            BaseView baseView = goView.GetComponent<BaseView>();
            dicView.Add(baseView.viewIndex, baseView);
            goView.SetActive(false);
        }

        SwitchView(ViewIndex.EmptyView, null,()=> {
            if (callback != null)
                callback();
        });
    }

    public void SwitchView(ViewIndex viewIndex, ViewParam param = null, Action callback = null)
    {
        if(currentView != null)
        {
            currentView.OnHide(() =>
            {
                ShowNewView(viewIndex, param, callback);
            });
        } else
            ShowNewView(viewIndex, param, callback);
    }

    private void ShowNewView(ViewIndex viewIndex, ViewParam param = null, Action callback = null)
    {
        currentView = dicView[viewIndex];
        currentView.OnSetUp(param, callback);
    }
}
