using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public Test1 _test1;
    private bool IsClick = false;

    public Transform c1;
    public Transform c2;
    public float pos = 100;
    public float mouseScroll = 1200;
    public float rot = 500;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X");     //控制人物转向
            float y = Input.GetAxis("Mouse Y");     //控制镜头上下
            Camera.main.transform.localPosition += Camera.main.transform.right * -x * Time.deltaTime * pos;
            Camera.main.transform.localPosition += Camera.main.transform.up * -y * Time.deltaTime * pos;
        }
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X");     //控制人物转向
            float y = Input.GetAxis("Mouse Y");     //控制镜头上下
            
            Camera.main.transform.localEulerAngles += Camera.main.transform.up * x * Time.deltaTime * rot;
            Camera.main.transform.localEulerAngles += Camera.main.transform.right * -y * Time.deltaTime * rot;
        }
        float MousScroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.transform.localPosition += Camera.main.transform.forward * MousScroll * Time.deltaTime * mouseScroll;
    }

    internal void SetPosAndRot(Transform _transform)
    {
        transform.position = _transform.position;
        transform.rotation = _transform.rotation;
    }
}
