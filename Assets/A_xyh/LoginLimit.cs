using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class LoginLimit : MonoBehaviour
{
    [SerializeField]
    private GameObject MainGame;

    [SerializeField]
    private string private_key = "test123";


    public InputField inputField;

    public Text result;

    private void Start()
    {
        string mac_adress_before = MySystemInfo.GetMacAddress();
        inputField.text = mac_adress_before;
    }

    private string Encrypt(string mac_adress)
    {
        return XXTEAManager.Encrypt(private_key + mac_adress);
    }

    private string Decrypt(string encrypted_code)
    {
        string mac_adress = XXTEAManager.Decrypt(encrypted_code);
        mac_adress = mac_adress.Replace(private_key, string.Empty);
        return mac_adress;
    }


    public void OnClick_Encrypt()
    {
        string mac_adress_before = MySystemInfo.GetMacAddress();
        Debug.Log("mac_adress_before = " + mac_adress_before);

        string encrypted_code = Encrypt(mac_adress_before);
        Debug.Log("encrypted_code = " + encrypted_code);

        inputField.text = encrypted_code;
    }

    public void OnClick_Decrypt()
    {
        string mac_adress_after = Decrypt(inputField.text);
        Debug.Log("mac_adress_after = " + mac_adress_after);

        string mac_adress_before = MySystemInfo.GetMacAddress();

        if (mac_adress_after.Equals(mac_adress_before))
        {
            result.color = Color.green;
            result.text = "解密成功！";
        }
        else
        {
            result.color = Color.red;
            result.text = "解密失败！";
        }
    }
}



