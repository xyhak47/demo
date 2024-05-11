using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class xyhtest : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetMac();


    // Start is called before the first frame update
    void Start()
    {

        //string mac_adress_before = MySystemInfo.GetMacAddress();

        //Debug.Log("mac_adress_before = " + SystemInfo.deviceUniqueIdentifier);
        //Debug.Log("graphicsDeviceID = " + SystemInfo.graphicsDeviceID);


        //Debug.Log("get mac from js = " + GetMac());
        //StartProcess();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void StartProcess()
    {
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        process.StandardInput.WriteLine(@"svn log d:\Projects\dev-A");
        process.StandardInput.AutoFlush = true;
        process.StandardInput.WriteLine("exit");

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        process.Close();
    }

}
