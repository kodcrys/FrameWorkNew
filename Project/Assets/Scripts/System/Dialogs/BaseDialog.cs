using System;
using UnityEngine;

public class BaseDialog : MonoBehaviour
{
    public DialogIndex dialogIndex;
    BaseAnimation baseAnimation;
    RectTransform rectTrans;

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        baseAnimation = gameObject.GetComponentInChildren<BaseAnimation>();

        if (baseAnimation == null)
        {
            GameObject goBA = new GameObject();
            goBA.transform.SetParent(transform);

            goBA.AddComponent<RectTransform>();
            goBA.AddComponent<BaseAnimation>();
            goBA.name = "BaseAnimtion";
            baseAnimation = goBA.GetComponent<BaseAnimation>();
        }

        OnAwake();
    }

    public void ShowDialog(DialogParam param = null, Action callback = null)
    {
        gameObject.SetActive(true);
        rectTrans.SetAsLastSibling();
        OnSetUp(param);
        baseAnimation.OnShowAnimation(() =>
        {
            OnShow();
            if (callback != null)
                callback();
        });
    }

    protected virtual void OnAwake()
    {

    }

    public virtual void OnSetUp(DialogParam param = null)
    {

    }

    public virtual void OnShow(DialogParam param = null)
    {

    }

    public void HideDialog(Action callback = null)
    {
        baseAnimation.OnHideAnimation(() =>
        {
            OnHide();
            gameObject.SetActive(false);
            if (callback != null)
                callback();
        });
    }

    public virtual void OnHide()
    {

    }
}
