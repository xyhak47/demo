using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD.TheManager
{
    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        private bool IsLoading;
        //private bool IsLoadData;
        private AsyncOperation asyncLoad;
        //private float tmpff;
        //private bool IsLoading;
        internal void StartLoadScene(string _mapname)
        {
            IsLoading = true;
            if (UIManager.Instance)
            {
                //UIManager.Instance.SetLoadScene(true);
                //UIManager.Instance._LoadingUI.PlaySound(true);
            }
            StartCoroutine(LoadGameAsyncScene(_mapname));
        }

        private IEnumerator LoadGameAsyncScene(string _s)
        {
            asyncLoad = SceneManager.LoadSceneAsync(_s);
            asyncLoad.allowSceneActivation = true;
            
            //SceneManager.LoadScene(_s);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            Manager.Instance.LoadSceneDone();
        }
    }
}
