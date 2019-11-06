using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneData
{
    public string sceneName;
    public Action<object> callback;
}

public class LoadSceneManager : SingletonMono<LoadSceneManager>
{
    public void OnLoadScene(string sceneName, Action<object> callback)
    {
        StopCoroutine("LoadScene");
        StartCoroutine("LoadScene", new LoadSceneData
        {
            sceneName = sceneName,
            callback = callback
        });
    }

    IEnumerator LoadScene(LoadSceneData data)
    {
        //LoadingView loadingView = null;
        //ViewManager.Instance.SwitchView(ViewIndex.LoadingView, null, () =>
        //{
        //    loadingView = ViewManager.Instance.currentView as LoadingView;
        //});
        //yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(() => loadingView != null);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(data.sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if(data.callback != null)
        {
            data.callback("Finish load");
        }

        ViewManager.Instance.SetUpCamera();
        DialogManager.Instance.SetUpCamera();

        //yield return new WaitForSeconds(1f);

        //if(ViewManager.Instance.currentView.viewIndex == ViewIndex.LoadingView)
        //{
        //    ViewManager.Instance.SwitchView(ViewIndex.EmptyView);
        //}
    }

    public void OnLoadScene(int index)
    {
        StopCoroutine("LoadScene");
        StartCoroutine("LoadScene", index);
    }

    IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(0.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
