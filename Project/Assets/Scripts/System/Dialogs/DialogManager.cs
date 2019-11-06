using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : SingletonMono<DialogManager>
{
    private Dictionary<DialogIndex, BaseDialog> dicDialog = new Dictionary<DialogIndex, BaseDialog>();
    public DialogIndex currentDialogIndex;
    public List<BaseDialog> lsShow = new List<BaseDialog>();

    public void SetUpCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }

    private void Start()
    {
        foreach(DialogIndex e in DialogConfig.dialogIndices)
        {
            string nameDialog = e.ToString();
            GameObject goDialog = Instantiate(Resources.Load("Dialogs/" + nameDialog, typeof(GameObject))) as GameObject;
            goDialog.transform.SetParent(transform, false);

            RectTransform rectTrans = goDialog.GetComponent<RectTransform>();
            rectTrans.offsetMax = rectTrans.offsetMin = Vector2.zero;

            BaseDialog baseDialog = goDialog.GetComponent<BaseDialog>();
            dicDialog.Add(baseDialog.dialogIndex, baseDialog);

            goDialog.SetActive(false);
        }
    }

    public void ShowDialog(DialogIndex dialogIndex, DialogParam param = null, Action callback = null)
    {
        BaseDialog dialog = dicDialog[dialogIndex];

        dialog.ShowDialog(param, callback);
        lsShow.Add(dialog);
    }

    public void HideDialog(BaseDialog dialog, Action callback = null)
    {
        dialog.HideDialog(callback);
        lsShow.Remove(dialog);
    }

    public void HideDialog(DialogIndex dialogIndex, Action callback)
    {
        BaseDialog dialog = FindDialog(dialogIndex);
        if (dialog != null)
        {
            dialog.HideDialog(callback);
            lsShow.Remove(dialog);
        }
    }

    public void HideAllDialog(Action callback = null)
    {
        for (int i = 0; i < lsShow.Count - 1; i++)
        {
            lsShow[i].HideDialog(null);

        }
        lsShow[lsShow.Count - 1].HideDialog(callback);
        lsShow.Clear();
    }

    public BaseDialog FindDialogVisible(DialogIndex dialogIndex)
    {
        for(int i = 0; i < lsShow.Count; i++)
        {
            if (lsShow[i].dialogIndex == dialogIndex)
                return lsShow[i];
        }
        return null;
    }

    public BaseDialog FindDialog(DialogIndex dialogIndex)
    {
        return dicDialog[dialogIndex];
    }
}
