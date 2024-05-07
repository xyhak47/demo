using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uTools;
using XD.GameStatic;
using XD.TheManager;

namespace XD.UI
{
    public class ModelToggle : MonoBehaviour, uIPointHandler
    {
        public BtnType _BtnType = BtnType.None;
        public int _ToggleType;
        public Toggle _toggle;
        internal int ID;
        internal Sprite clickBefore;
        internal Sprite clickAfter;

        /// <summary>
        /// ��ʼ�����ñ�ǩ��ť��Ϣ
        /// </summary>
        /// <param name="index"></param>
        /// <param name="btnData"></param>
        /// <param name="btnRes"></param>
        internal void SetModelToggle(int index, string btnData, string btnRes)
        {
            ID = index;
            _BtnType = BtnType.ModelIntro;
            AnalyzeBtnData(btnData);
            SetSprite(btnRes);
        }

        /// <summary>
        /// ������ť����
        /// </summary>
        /// <param name="_s"></param>
        private void AnalyzeBtnData(string _s)
        {
            if (_s.Contains(","))
            {
                string[] _ss = _s.Split(',');
                switch (_ss[1])
                {
                    case "sub1":
                        _ToggleType = 1;
                        break;
                    case "sub2":
                        _ToggleType = 2;
                        break;
                    case "sub3":
                        _ToggleType = 3;
                        break;
                    case "sub4":
                        _ToggleType = 4;
                        break;
                    case "sub5":
                        _ToggleType = 5;
                        break;
                    case "sub6":
                        _ToggleType = 6;
                        break;
                }
            }
        }

        /// <summary>
        /// ���ð�ťͼƬ
        /// </summary>
        /// <param name="_s">��ť��Դ</param>
        private void SetSprite(string _s)
        {
            if (_s.Contains("|"))
            {
                string[] _ss = _s.Split('|');
                clickBefore = Resources.Load<Sprite>(_ss[0]);
                clickAfter = Resources.Load<Sprite>(_ss[1]);
            }
        }

        #region ʵ�ֽӿ�
        public void OnPointerClick(PointerEventData eventData)
        {
            //����ǩ������ʱ����ת���ñ�ǩ
            UIManager.Instance._ModelUI.SetToggle(ID);
            return;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            return;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            return;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            return;
        }
        #endregion
    }
}