using UnityEngine.Networking;

namespace XD.TheManager
{
    public class LoadStreamingHelp:Singleton<LoadStreamingHelp>
    {
        internal byte[] LoadStreamingData(string path)
        {
            UnityWebRequest getRequest = UnityWebRequest.Get(path);
            getRequest.SendWebRequest();
            bool auto = true;
            byte[] Data = new byte[0];
            while (auto)
            {
                if (getRequest.downloadHandler.isDone)
                {
                    auto = false;
                    Data = getRequest.downloadHandler.data;
                }
            }
            return Data;
        }
    }
}
