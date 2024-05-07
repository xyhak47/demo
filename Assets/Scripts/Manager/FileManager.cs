#define log
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace XD.TheManager
{
    public class FileManager : Singleton<FileManager>
    {
        public enum FileType
        {
            None,
            AudioCilp,
            String,
            Texture,
        }
        public enum FolderType
        {
            None,
            StreamingAssets,
            PersistentData,
        }



        /// <summary>
        /// 显示各个文件夹路径
        /// </summary>
        public void ShowPath()
        {
            log("dataPath: ======> " + Application.dataPath);
            log("persistentDataPath: ======> " + Application.persistentDataPath);
            log("streamingAssetsPath: ======> " + Application.streamingAssetsPath);
            log("temporaryCachePath: ======> " + Application.temporaryCachePath);

        }

        /// <summary>
        /// 将StreamingAssets里的文件复制到PersistentDataPath下
        /// </summary>
        public void CopyStreamingFileToPersistentDataPath(params string[] filePath)
        {
            if (Application.platform == RuntimePlatform.Android)                    //如果是Android平台
            {
                foreach (string path in filePath)
                {
                    StartCoroutine(CopyFile_Android(path));
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)          //如果是Windows平台
            {
                foreach (string path in filePath)
                {
                    File.Copy(Application.streamingAssetsPath + "/" + path, Application.persistentDataPath + "/" + path, true);

                    log("CopyMusic Success!" + "\n" + "Path: ======> " + Application.persistentDataPath + "/" + path);
                }
            }

        }


        /// <summary>
        /// 从StreamingAssets加载文件
        /// </summary>
        public void LoadFileByStreamingAssets(string file, FileType fileType, Action<object> Callback)
        {
            FileType m_FileType = fileType;
            FolderType m_FolderType = FolderType.StreamingAssets;

            StartCoroutine(LoadFileIEnumerator(file, Callback, m_FileType, m_FolderType));
        }



        /// <summary>
        /// 从PersistentDataPath加载文件
        /// </summary>
        public void LoadFileByPersistentDataPath(string file, FileType fileType, Action<object> Callback)
        {
            FileType m_FileType = fileType;
            FolderType m_FolderType = FolderType.PersistentData;

            StartCoroutine(LoadFileIEnumerator(file, Callback, m_FileType, m_FolderType));
        }


        //===============================================================================

        /// <summary>
        /// 加载文件
        /// </summary>
        IEnumerator LoadFileIEnumerator(string file, Action<object> Callback, FileType fileType, FolderType m_FolderType)
        {
            string filePath = "";

            switch (m_FolderType)
            {
                case FolderType.StreamingAssets:
                    if (Application.platform == RuntimePlatform.Android)                    //如果是Android平台
                    {
                        filePath = Application.streamingAssetsPath + "/" + file;
                    }
                    else if (Application.platform == RuntimePlatform.WindowsEditor)          //如果是Windows平台
                    {
                        filePath = "file:///" + Application.streamingAssetsPath + "/" + file;
                    }
                    break;
                case FolderType.PersistentData:
                    if (Application.platform == RuntimePlatform.Android)                    //如果是Android平台
                    {
                        filePath = "file://" + Application.persistentDataPath + "/" + file;
                    }
                    else if (Application.platform == RuntimePlatform.WindowsEditor)          //如果是Windows平台
                    {
                        filePath = "file:///" + Application.persistentDataPath + "/" + file;
                    }
                    break;
            }


            WWW w = new WWW(filePath);

            yield return w;


            if (w.error == null)
            {
                switch (fileType)
                {
                    case FileType.AudioCilp:
                        //我使用的Unity是2017.3.0.此处 这里只 支持 .ogg 格式的 音频
                        Callback(w.GetAudioClip());
                        break;
                    case FileType.String:
                        string data = System.Text.Encoding.UTF8.GetString(w.bytes);
                        Callback(data);
                        break;
                    case FileType.Texture:
                        Callback(w.texture);
                        break;
                    default:
                        break;
                }


                switch (m_FolderType)
                {

                    case FolderType.StreamingAssets:
                        log("StreamingAssetsPath Load Success...");
                        break;
                    case FolderType.PersistentData:
                        log("PersistentDataPath Load Success...");
                        break;
                }

            }
            else
            {

                log("Error : ======> " + w.error);

            }

        }


        /// <summary>
        /// 安卓端复制文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        IEnumerator CopyFile_Android(string fileName)
        {
            WWW w = new WWW(Application.streamingAssetsPath + "/" + fileName);

            yield return w;

            if (w.error == null)
            {
                FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + fileName);

                //判断文件是否存在
                if (!fi.Exists)
                {
                    FileStream fs = fi.OpenWrite();

                    fs.Write(w.bytes, 0, w.bytes.Length);

                    fs.Flush();

                    fs.Close();

                    fs.Dispose();

                    log("CopyTxt Success!" + "\n" + "Path: ======> " + Application.persistentDataPath + fileName);

                }

            }
            else
            {
                log("Error : ======> " + w.error);
            }

        }

        /// <summary>
        /// 向我的控制台打印内容
        /// </summary>
        /// <param name="content">内容</param>
        void log(string content)
        {
#if log
            Debug.Log(content);
#endif

        }
    }
}
