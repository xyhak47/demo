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
    /// <summary>
    /// 倒闸中用到的按钮控制
    /// </summary>
    public class ControlBtn : MonoBehaviour, uIPointHandler
    {
        public BtnType btnType = BtnType.None; 
        internal string enterBefore;
        internal string enterAfter;

        internal void Init(string pic1, string pic2)
        {
            enterBefore = pic1;
            enterAfter = pic2;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(btnType == BtnType.Prompt || btnType == BtnType.AnyArea)
            {
                if (!GameUnitManager.Instance._Scene2.isStop)
                {
                    GameUnitManager.Instance._Scene2.isStop = true;
                }
                UIManager.Instance._DaoZhaUI.IsAlpha = true;
                gameObject.SetActive(false);
            }
            else if(btnType == BtnType.Queren)
            {
                UIManager.Instance._DaoZhaUI.queRenUI.master.SetActive(false);
            }
            else if(btnType == BtnType.PopKg)
            {
                Image img = transform.parent.parent.gameObject.GetComponent<Image>();
                img.sprite = Resources.Load<Sprite>(enterAfter);
                transform.parent.gameObject.SetActive(false);
                UIManager.Instance._DaoZhaUI.IsAlpha = true;
            }
            else if(btnType == BtnType.PopDl)
            {
                transform.parent.gameObject.SetActive(false);
                UIManager.Instance._DaoZhaUI.IsAlpha = true;
            }
            GameUnitManager.Instance.Next();
            return;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (btnType)
            {
                case BtnType.Prompt:
                case BtnType.Queren:
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(enterAfter);
                    gameObject.GetComponent<Image>().SetNativeSize();
                    break;
            }
            return;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            switch (btnType)
            {
                case BtnType.Prompt:
                case BtnType.Queren:
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(enterBefore);
                    gameObject.GetComponent<Image>().SetNativeSize();
                    break;
            }
            return;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            return;
        }
    }

}