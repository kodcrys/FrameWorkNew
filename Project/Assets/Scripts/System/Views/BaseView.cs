using System;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public ViewIndex viewIndex;
    private BaseAnimation baseAnimation;

    private void Awake()
    {
        baseAnimation = gameObject.GetComponentInChildren<BaseAnimation>();
        if(baseAnimation == null)
        {
            GameObject goBA = new GameObject();
            goBA.transform.SetParent(transform);

            goBA.AddComponent<RectTransform>();
            goBA.AddComponent<BaseAnimation>();
            goBA.name = "BaseAnimtion";
            baseAnimation = goBA.GetComponent<BaseAnimation>();
        }
    }

    public virtual void OnSetUp(ViewParam param = null, Action callback = null)
    {
        OnShow(() =>
        {
            if (callback != null)
                callback();
        });
    }

    protected virtual void OnShow(Action callback)
    {
        gameObject.SetActive(true);
        baseAnimation.OnShowAnimation(callback);
    }

    public virtual void OnHide(Action callback)
    {
        gameObject.SetActive(false);
        if (callback != null)
            callback();
    }
}
