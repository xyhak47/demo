using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XD.Map;

public class ClickMaxMap : MonoBehaviour,IPointerClickHandler
{
    public MiniMap miniMap;
    private Vector2 tempVector; //鼠标点击位置
    private Vector2 rayPoint;   //通用分辨率
    public void OnPointerClick(PointerEventData eventData)
    {
        tempVector = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        Debug.Log("点击位置：" + tempVector);

        RectTransform maxMap = GameObject.Find("MapContent").GetComponent<RectTransform>();

        rayPoint = new Vector2((tempVector.x - (((Screen.width - maxMap.sizeDelta.x) / 2) / Screen.width)) / (maxMap.sizeDelta.x / Screen.width),
                 (tempVector.y - (((Screen.height - maxMap.sizeDelta.y) / 2) / Screen.height)) / (maxMap.sizeDelta.x / Screen.height));

        Debug.Log("通用分辨率："+rayPoint);

        Ray ray = miniMap.mapCamera.ViewportPointToRay(rayPoint); //将视图窗口点击位置转化到相机射线
        RaycastHit hit;
        Debug.Log("RayD:" + ray.direction);
        Debug.Log("Ray:" + ray);
        if (Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            miniMap.player.position = hit.point;
            Debug.Log("位置：" + tempVector);
        }
        //System.IO.File.ReadAllText("D://");//webgl下不允许
        // System.IO.File.ReadAllText（Application.streamingAssetsPath+"file.txt"）//允许
        //Application.streamingAssetsPath,有AB包的时候才能使用

        //Application.persistentDataPath;

            //APP
    }
}
