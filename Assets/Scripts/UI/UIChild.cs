using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XD.TheManager;
/// <summary>
/// 步骤流程条
/// </summary>
public class UIChild : MonoBehaviour,IPointerClickHandler
{
    private Text _infoText;     //流程名称
    public Image _image;
    public Color _selfColor;     //本身的颜色
    public Color _highlight;     //高光颜色
    internal void SetInfoText(string _info)
    {
        _infoText = this.GetComponentInChildren<Text>();
        _infoText.text = _info;
    }
    #region 鼠标方法
    /// <summary>
    /// 鼠标点击
    /// </summary>
    /// <param name="eventData">点击数据</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (Manager.Instance._stateType != XD.GameStatic.StateType.CheckPractice)
        {
            Manager.Instance._stepUI.SetInfoText(_infoText.text);
            Manager.Instance._stepUI._textParent.SetActive(true);
        }*/
    }
    #endregion
    /// <summary>
    /// 自己高光
    /// </summary>
    public void SelfHighlight()
    {
        foreach (KeyValuePair<int,UIChild> item in Manager.Instance._uiChild)
        {
            item.Value._image.color = _selfColor;
        }
        _image.color = _highlight;
    }
    /// <summary>
    /// 关闭自己的高光
    /// </summary>
    public void CloseHighlight()
    {
        _image.color = _selfColor;
    }

}
