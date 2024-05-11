using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MySystemInfo
{
    public static string GetMacAddress()
    {
        string physicalAddress = "";
        NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adaper in nice)
        {
            Debug.Log(adaper.Description);
            if (adaper.Description == "en0")
            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();
                break;
            }
            else
            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();
                if (physicalAddress != "")
                {
                    break;
                };
            }
        }

        return physicalAddress;
    }

}