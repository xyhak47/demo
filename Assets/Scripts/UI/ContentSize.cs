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
        //若文本是图片
        if (content.gameObject.GetComponent<Image>())
        {
            //销毁文本大小自适应
            if (content.gameObject.GetComponent<ContentSizeFitter>())
            {
                Destroy(content.gameObject.GetComponent<ContentSizeFitter>());
            }
            //设置文本宽高
            content.sizeDelta = new Vector2(785f, contentHeight);
            //根据结束按钮是否显示，设置总体文案的宽高
            if (over.gameObject.activeSelf)
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + contentHeight + overHeight);
            }
            else
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + contentHeight);
            }
            //设置文本的位置
            PosY = 0 - (contentHeight / 2 + titleHeight);
            content.anchoredPosition = new Vector2(18, PosY);
        }
        else if (content.gameObject.GetComponent<Text>()) //若文本是文字
        {
            if (!content.gameObject.GetComponent<ContentSizeFitter>())
            {
                content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            //设置文本宽高
            DeltaY = content.sizeDelta.y;  //根据内容多少自适应文本大小后的增量
            content.sizeDelta = new Vector2(785f, DeltaY);
            //根据结束按钮是否显示，设置总体文案的宽高
            if (over.gameObject.activeSelf)
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + DeltaY + overHeight);
            }
            else
            {
                m_Content.sizeDelta = new Vector2(785f, titleHeight + DeltaY);
            }
            //设置文本的位置
            PosY = 0 - (DeltaY / 2 + titleHeight);
            content.anchoredPosition = new Vector2(18, PosY);
        }
    }
}
