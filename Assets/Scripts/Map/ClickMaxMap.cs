using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XD.Map;

public class ClickMaxMap : MonoBehaviour,IPointerClickHandler
{
    public MiniMap miniMap;
    private Vector2 tempVector; //�����λ��
    private Vector2 rayPoint;   //ͨ�÷ֱ���
    public void OnPointerClick(PointerEventData eventData)
    {
        tempVector = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        Debug.Log("���λ�ã�" + tempVector);

        RectTransform maxMap = GameObject.Find("MapContent").GetComponent<RectTransform>();

        rayPoint = new Vector2((tempVector.x - (((Screen.width - maxMap.sizeDelta.x) / 2) / Screen.width)) / (maxMap.sizeDelta.x / Screen.width),
                 (tempVector.y - (((Screen.height - maxMap.sizeDelta.y) / 2) / Screen.height)) / (maxMap.sizeDelta.x / Screen.height));

        Debug.Log("ͨ�÷ֱ��ʣ�"+rayPoint);

        Ray ray = miniMap.mapCamera.ViewportPointToRay(rayPoint); //����ͼ���ڵ��λ��ת�����������
        RaycastHit hit;
        Debug.Log("RayD:" + ray.direction);
        Debug.Log("Ray:" + ray);
        if (Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            miniMap.player.position = hit.point;
            Debug.Log("λ�ã�" + tempVector);
        }
        //System.IO.File.ReadAllText("D://");//webgl�²�����
        // System.IO.File.ReadAllText��Application.streamingAssetsPath+"file.txt"��//����
        //Application.streamingAssetsPath,��AB����ʱ�����ʹ��

        //Application.persistentDataPath;

            //APP
    }
}
