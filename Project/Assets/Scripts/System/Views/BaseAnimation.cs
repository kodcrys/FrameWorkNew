using UnityEngine;
using System;

public class BaseAnimation : MonoBehaviour
{
    public virtual void OnShowAnimation(Action callback)
    {
        if (callback != null)
            callback();
    }

    public virtual void OnHideAnimation(Action callback)
    {
        if (callback != null)
            callback();
    }
}
