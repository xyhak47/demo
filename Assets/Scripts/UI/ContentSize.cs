using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSize : MonoBehaviour
{
    public RectTransform title;    //CTitle
    public RectTransform content;  //CData
    public RectTransform over;     //COver

    private RectTransform m_Content;
    private float titleHeight = 60f;
    private float contentHeight = 490f;
    private float overHeight = 100f;
    private float DeltaY;
    private float PosY;

    void Awake()
    {
        m_Content = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        //���ı���ͼƬ
        if (content.gameObject.GetComponent<Image>())
        {
            //�����ı���С����Ӧ
            if (content.gameObject.GetComponent<ContentSizeFitter>())
            {
                Destroy(content.gameObject.GetComponent<ContentSizeFitter>());
            }
            //�����ı����
            content.sizeDelta = new Vector2(785f, contentHeight);
            //���ݽ�����ť�Ƿ���ʾ�����������İ��Ŀ��
            if (over.gameObject.activeSelf)
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + contentHeight + overHeight);
            }
            else
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + contentHeight);
            }
            //�����ı���λ��
            PosY = 0 - (contentHeight / 2 + titleHeight);
            content.anchoredPosition = new Vector2(18, PosY);
        }
        else if (content.gameObject.GetComponent<Text>()) //���ı�������
        {
            if (!content.gameObject.GetComponent<ContentSizeFitter>())
            {
                content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            //�����ı����
            DeltaY = content.sizeDelta.y;  //�������ݶ�������Ӧ�ı���С�������
            content.sizeDelta = new Vector2(785f, DeltaY);
            //���ݽ�����ť�Ƿ���ʾ�����������İ��Ŀ��
            if (over.gameObject.activeSelf)
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + DeltaY + overHeight);
            }
            else
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + DeltaY);
            }
            //�����ı���λ��
            PosY = 0 - (DeltaY / 2 + titleHeight);
            content.anchoredPosition = new Vector2(18, PosY);
        }
    }
}
