using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoaderManager : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        yield return new WaitForSeconds(0.5f);
        TopBarManager.Instance.SetUp();
        ViewManager.Instance.Init(() =>
        {
            LoadingView loadingView = null;
            ViewManager.Instance.SwitchView(ViewIndex.LoadingView, null, () =>
            {
                loadingView = ViewManager.Instance.currentView as LoadingView;

                ConfigManager.Instance.InitConfig(() =>
                {
                    loadingView.OnUpdateProgess(50);
                    DataAPIManager.Instance.InitData(() =>
                    {
                        loadingView.OnUpdateProgess(70, 0.3f, ()=> {
                            LoadSceneManager.Instance.OnLoadScene("Buffer", (obj) =>
                            {
                                loadingView.OnUpdateProgess(100);
                                ViewManager.Instance.SwitchView(ViewIndex.HomeView);
                                TopBarManager.Instance.Show();
                            });
                        });
                       
                    });
                });
            });
        }); 
    }
}
