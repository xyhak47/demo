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
        /// 初始化设置标签按钮信息
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
        /// 解析按钮类型
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
        /// 设置按钮图片
        /// </summary>
        /// <param name="_s">按钮资源</param>
        private void SetSprite(string _s)
        {
            if (_s.Contains("|"))
            {
                string[] _ss = _s.Split('|');
                clickBefore = Resources.Load<Sprite>(_ss[0]);
                clickAfter = Resources.Load<Sprite>(_ss[1]);
            }
        }

        #region 实现接口
        public void OnPointerClick(PointerEventData eventData)
        {
            //当标签被点下时，跳转到该标签
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