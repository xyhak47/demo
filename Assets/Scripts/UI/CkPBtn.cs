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
    public class CkPBtn : MonoBehaviour, uIPointHandler
    {
        private int ID;
        public BtnType _BtnType = BtnType.None;
        private BtnMenu btnMenu;
        public Image _Img;
        internal void SetBtn(int index, BtnMenu bm)
        {
            ID = index;
            btnMenu = bm;
            _BtnType = BtnType.ChkBtn;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_BtnType == BtnType.ChkBtn)
            {
                UIManager.Instance.AnalyzeCkPBtn(btnMenu);
                Manager.Instance.SpLoadScene("Main");
            }
            //GameUnitManager.Instance.Next();
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
    }

}
