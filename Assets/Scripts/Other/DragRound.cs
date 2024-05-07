using UnityEngine;
using System.Collections;
public class DragRound : MonoBehaviour
{
    private GameObject obj;
    public float mouseMovespeed = 2;
    public float rotationspeed = 1;

    public bool isCanAutogiration = true;          //自动旋转
    public bool horizontalRotate = true;    //水平旋转
    public bool verticalRotate;             //垂直旋转
    public bool isCanManualRotation;        //控制旋转

    private void Start()
    {
        obj = transform.gameObject;
    }
    void Update()
    {
        if (isCanManualRotation)
        {
            if (horizontalRotate)
            {
                HorizontalRotate();
            }
            if (verticalRotate)
            {
                VerticalRotate();
            }
        }
        if (isCanAutogiration)
        {
            //自动旋转
            obj.transform.Rotate(Vector3.up, -Time.deltaTime * 50 * rotationspeed);
        }
        //if (Input.GetMouseButtonUp(0))
        //{
        //    isCanAutogiration = true;
        //}
    }
    private void HorizontalRotate()
    {
        if (Input.GetMouseButton(0))
        {
            //isCanAutogiration = false;
            obj.transform.Rotate(Vector3.up, -Time.deltaTime * 200 * Input.GetAxis("Mouse X")* mouseMovespeed);
        }
    }
    private void VerticalRotate()
    {
        if (Input.GetMouseButton(0))
        {
            //isCanAutogiration = false;
            obj.transform.Rotate(Vector3.left, -Time.deltaTime * 200 * Input.GetAxis("Mouse Y")* mouseMovespeed);
        }
    }

}
