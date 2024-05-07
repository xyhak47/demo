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
    public class TipBtn : MonoBehaviour, uIPointHandler
    {
        public BtnType btnType = BtnType.None;
        internal string enterBefore;
        internal string enterAfter;
        private int step;
        internal int stepStart;
        private string currArea;

        /// <summary>
        /// ��ʼ����ʾ��Ϣ��ť����Ϣ
        /// </summary>
        /// <param name="pic1">��ť����ǰ��ͼƬ·��</param>
        /// <param name="pic2">��ť���º��ͼƬ·��</param>
        /// <param name="st">���ڵĳ�ʼ����ֵ</param>
        /// <param name="area">�������һ�����ڳ���������</param>
        internal void Init(string pic1, string pic2, int st, string area)
        {
            enterBefore = pic1;
            enterAfter = pic2;
            step = UIManager.Instance._DzTipUI.step;  //��ǰ���ڱ��
            stepStart = st;
            currArea = area;
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(enterBefore);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //��ǰ���ڻ�δ�����һ������
            if (step < Manager.Instance.dzTips.Count)
            {
                //��������
                if (btnType == BtnType.TipBtn1)
                {
                    UIManager.Instance.RestAllUI();
                    UIManager.Instance._Title.uiobj.SetActive(true);
                    //������ָ�����ǰ���ڿ�ʼ���裬��1�����ں������м�1
                    GameUnitManager.Instance._Scene2.Tmpstep = stepStart - 1;
                    //�������ı�ǩ�ָ�����ǰ����
                    UIManager.Instance._Title.task.TaskCancel(GameUnitManager.Instance._Scene2.togID);
                    GameUnitManager.Instance._Scene2.togID--;
                    UIManager.Instance._Title.task.TaskSelect(GameUnitManager.Instance._Scene2.togID);
                    //���������������ڳ���
                    if (!currArea.Equals(Manager.Instance.AllDzSteps[stepStart].Area))
                    {
                        int area = int.Parse(Manager.Instance.AllDzSteps[stepStart].Area);
                        PlayerMove.Instance.ResetPosAndRot(area);
                    }
                    //�����Ѿ�ִ�й��Ķ���
                    if (GameUnitManager.Instance._Scene2.tmpAnimObj.Count > 0)
                    {
                        List<TmpAnimObj> tmp = GameUnitManager.Instance._Scene2.tmpAnimObj;
                        foreach(TmpAnimObj tp in tmp)
                        {
                            tp.Rest();
                        }
                        GameUnitManager.Instance._Scene2.tmpAnimObj.Clear();
                    }
                }
                //����ѧϰ
                UIManager.Instance._DzTipUI.uiobj.SetActive(false);
                GameUnitManager.Instance.Next();
            }
            else if (step == Manager.Instance.dzTips.Count)  //���һ�����ڽ��������ʾ��Ϣ��ť
            {
                //����̽��
                if (btnType == BtnType.TipBtn1)
                {
                    UIManager.Instance._DzTipUI.uiobj.SetActive(false);
                }
                else if(btnType == BtnType.TipBtn2)  //����������
                {
                    UIManager.Instance.Back();
                }
            }
            return;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(enterAfter);
            return;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(enterBefore);
            return;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            return;
        }
    }
}