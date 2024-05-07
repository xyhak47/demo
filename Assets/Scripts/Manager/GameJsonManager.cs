using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace XD.TheManager
{
    public class GameJsonManager : Singleton<GameJsonManager>
    {
        internal void LoadJsonData<T>(string _s, System.Action<List<T>> action)
        {
            loadAllData<T>(_s, action);
        }

        private void loadAllData<T>(string _s, System.Action<List<T>> action1)
        {
            GetRequest(_s, (string str) =>
            {
                List<T> _ls = new List<T>();
                JObject json1 = (JObject)JsonConvert.DeserializeObject(str);
                JArray array = (JArray)json1["RECORDS"];
                for (int i = 0; i < array.Count; i++)
                {
                    T _jm = JsonConvert.DeserializeObject<T>(array[i].ToString());
                    _ls.Add(_jm);
                }
                action1(_ls);
            });
        }

        //internal byte[] LoadStreamingData(string path)
        //{
        //    UnityWebRequest getRequest = UnityWebRequest.Get(path);
        //    getRequest.SendWebRequest();
        //    bool auto = true;
        //    byte[] Data = new byte[0];
        //    //ËÀÑ­­h
        //    while (auto)
        //    {
        //        if (!string.IsNullOrEmpty(getRequest.error))
        //        {
        //            throw new System.Exception($"”µ“þ¼ÓÝdÊ§”¡{path}");
        //        }

        //        if (getRequest.downloadHandler.isDone)
        //        {
        //            auto = false;
        //            Data = getRequest.downloadHandler.data;
        //        }
        //    }
        //    return Data;
        //}

        public void GetRequest(string path, System.Action<string> action)
        {
            TextAsset ta = Resources.Load<TextAsset>(path);
            action(ta.text);
            //UnityWebRequest webRequest = UnityWebRequest.Get(path);
            ////yield return webRequest.SendWebRequest();
            //webRequest.SendWebRequest();
            //while (!webRequest.isDone)
            //{
            //    yield return null;
            //}
            //if (!string.IsNullOrEmpty(webRequest.error))
            //{
            //    Debug.LogError(webRequest.error);
            //}
            //else
            //{
            //    action(webRequest.downloadHandler.text);
            //}
        }
    }
}
