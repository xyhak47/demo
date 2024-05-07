using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XD.TheManager;
/// <summary>
/// ����������
/// </summary>
public class UIChild : MonoBehaviour,IPointerClickHandler
{
    private Text _infoText;     //��������
    public Image _image;
    public Color _selfColor;     //�������ɫ
    public Color _highlight;     //�߹���ɫ
    internal void SetInfoText(string _info)
    {
        _infoText = this.GetComponentInChildren<Text>();
        _infoText.text = _info;
    }
    #region ��귽��
    /// <summary>
    /// �����
    /// </summary>
    /// <param name="eventData">�������</param>
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
    /// �Լ��߹�
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
    /// �ر��Լ��ĸ߹�
    /// </summary>
    public void CloseHighlight()
    {
        _image.color = _selfColor;
    }

}
