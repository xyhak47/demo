using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{
    public RectTransform _optionsWin;
    public Text _optionText;

    internal void SetOptionsWin(string _text,Vector3 _pos,bool _bol)
    {
        _optionText.text = _text;
        _optionsWin.anchoredPosition = _pos;
        _optionsWin.gameObject.SetActive(_bol);
    }

    public void OnClick()
    {
        Debug.Log(_optionText.text);

    }

}
