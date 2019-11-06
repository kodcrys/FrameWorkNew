using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;
using System.Text;

public class HTTPManager : MonoBehaviour
{
    public void HTTPPost(string pathFunction, string pathDatabase, string data, bool isShowWaitingDialog, bool isHideWaitingDialog, Action<HTTPResponse> callback)
    {
        //if (isShowWaitingDialog)
        //    DialogManager.Instance.ShowDialog(DialogIndex.WaitDataDialog);

        string address = URLConfig.dbFunctionAddress + pathFunction;

        HTTPRequest request = new HTTPRequest(new Uri(address), HTTPMethods.Post, (req, res) =>
        {
            if (res.StatusCode == 200)
            {
                if (callback != null)
                    callback(res);
            }
            else
            {
                string errorStr = "Error: " + res.StatusCode + " "+ res.Message;
                //ErrorDialogParam errorParam = new ErrorDialogParam { error = errorStr };
                //DialogManager.Instance.ShowDialog(DialogIndex.ErrorDialog, errorParam);
            }
            //if(isHideWaitingDialog)
            //    DialogManager.Instance.HideDialog(DialogIndex.WaitDataDialog);
        });

        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.SetHeader("uuid", DatabaseModel.UserID);
        if (!pathDatabase.Equals(string.Empty))
            request.SetHeader("path", pathDatabase);
        if (!data.Equals(string.Empty))
            request.RawData = Encoding.UTF8.GetBytes(data);

        request.ConnectTimeout = TimeSpan.FromSeconds(60);
        request.DisableCache = true;
        request.Send();
    }
}
