using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uTools;
using XD.TheManager;

namespace XD.UI
{
    public class TagBtnItem : MonoBehaviour, uIPointHandler
    {
        public int ID;
        public Image img;
        public Text num;

        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance._ExamUI.answerCount = ID;
            UIManager.Instance._ExamUI.answerCard.SetPaper(UIManager.Instance._ExamUI.paper, ID);
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