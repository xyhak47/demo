using System.Collections;
using System.Collections.Generic;
using XD.GameStatic;
using XD.TheManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uTools;

namespace XD.UI
{
    public class BtnUnit : MonoBehaviour, uIPointHandler
    {
        #region �ֶ�
        public BtnType _BtnType = BtnType.None;
        private int _SceneType;
        private int _EventSystemType;
        internal bool clickState;
        internal int selectState;
        public Image _Img;
        private string defst;  //Ĭ��״̬��ͼƬ·��
        private string clkst;  //������ͼƬ·��
        private bool _Tag;
        private string stateCode;  //����״̬��
        internal int ID;
        #endregion

        #region ���ò���
        /// <summary>
        /// UI����
        /// </summary>
        /// <param name="bm"></param>
        internal void SetBtn(BtnMenu bm)
        {
            if (bm.BtnData.Contains(","))
            {
                string[] _d = bm.BtnData.Split(',');
                if (_d[0].Equals("scene"))
                {
                    _BtnType = BtnType.Scene;
                    switch (_d[1])
                    {
                        case "scene1":
                            _SceneType = 1;
                            break;
                        case "scene2":
                            _SceneType = 2;
                            break;
                    }
                }
                else if (_d[0].Equals("mode"))
                {
                    _BtnType = BtnType.Mode;
                    switch (_d[1])
                    {
                        case "child1":
                            _EventSystemType = 1;
                            break;
                        case "child2":
                            _EventSystemType = 2;
                            break;
                        case "child3":
                            _EventSystemType = 3;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// ��ǩ��������
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sl"></param>
        internal void SetSubBtn(int index, SubList sl, string sc, bool _t)
        {
            ID = index;
            _BtnType = BtnType.Sub;
            _Tag = _t;
            stateCode = sc;
            if (index == 0)
                clickState = true;
            else clickState = false;
            if (sl.SubRes.Contains("|"))
            {
                string[] _s = sl.SubRes.Split('|');
                defst = _s[0];
                clkst = _s[1];
            }
            SetImg();
            SetContent();
        }

        /// <summary>
        /// ���ñ�ǩͼƬ
        /// </summary>
        /// <param name="_d"></param>
        internal void SetImg()
        {
            if (clickState)
                _Img.sprite = Resources.Load<Sprite>(clkst);
            else
                _Img.sprite = Resources.Load<Sprite>(defst);
        }

        internal void SetdefaultTag()
        {
            switch(selectState)
            {
                case 1:
                case 2:
                    _Img.sprite = Resources.Load<Sprite>(clkst);
                    break;
                case 3:
                    _Img.sprite = Resources.Load<Sprite>(defst);
                    break;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void SetContent()
        {
            foreach (Contents _d in Manager.Instance.AllContents.Values)
            {
                if (_d.StateCode.Equals(stateCode))
                {
                    if (clickState)
                    {
                        //�����Ѵ��ڵ�Text��Image
                        UIManager.Instance._IntroUI.RestComp();
                        if (_Tag)
                        {
                            UIManager.Instance._IntroUI.Content[0].GetComponent<Text>().text = _d.Title;
                            var txt = UIManager.Instance._IntroUI.Content[1].AddComponent<Text>();
                            txt.font = Resources.Load<Font>("Fonts/AlibabaPuHuiTi-2-55-Regular");
                            txt.text = _d.Data;
                            txt.color = new Color(0, 0, 0);
                            txt.fontSize = 22;
                        }
                        else
                        {
                            UIManager.Instance._IntroUI.Content[0].GetComponent<Text>().text = _d.Title;
                            UIManager.Instance._IntroUI.Content[1].AddComponent<Image>().sprite = Resources.Load<Sprite>(_d.Data);
                        }
                    }
                }
            }
        }

        
        #endregion

        #region ʵ�ֽӿ�
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_BtnType)
            {
                case BtnType.Scene:
                    GameUnitManager.Instance._SceneType = (SceneType)_SceneType;
                    UIManager.Instance.SetSelectUI("mode");  //�л���ģʽѡ��
                    break;
                case BtnType.Mode:
                    GameUnitManager.Instance._EventSystemType = (EventSystemType)_EventSystemType;
                    switch (_EventSystemType)
                    {
                        case 1:
                            Manager.Instance.SpLoadScene("Main"); //ѧϰֱ�ӽ���main����
                            break;
                        case 2:
                            if (GameUnitManager.Instance._SceneType == SceneType.xunshi)
                            {
                                UIManager.Instance._SelectPracticeUI.InitStart(); //Ѳ����ϰ�л���Ѳ��ģ��ѡ��
                            }
                            else if (GameUnitManager.Instance._SceneType == SceneType.daozha)
                            {
                                Manager.Instance.SpLoadScene("Main"); //��բ��ϰ����main����
                            }
                            break;
                        case 3:
                            //UIManager.Instance.SetSelectUI("exam"); //�����л������˽���
                            //20221116�Ŀ���ģʽ��Ѳ�ӵĿ��˸ĳ��ֳ����ˣ�Ҫ����ת��Main�����к�����ʾ���⣻��բ�����ֱ����ʾ����
                            if (GameUnitManager.Instance._SceneType == SceneType.xunshi)
                            {
                                Manager.Instance.SpLoadScene("Main");
                            }
                            else if (GameUnitManager.Instance._SceneType == SceneType.daozha)
                            {
                                UIManager.Instance.SetSelectUI("exam");
                            }
                            break;
                    }
                    break;
                case BtnType.Sub:
                    //�Ƚ����еı�ǩ�����Ĭ��״̬
                    foreach (BtnUnit _d in UIManager.Instance._IntroUI.tags)
                    {
                        if (_d.clickState)
                        {
                            _d.clickState = false;
                            _d.SetImg();
                        }
                    }
                    //��ǰ��ǩΪ���״̬
                    clickState = true;
                    SetImg();
                    SetContent();
                    Scrollbar _vb = UIManager.Instance._IntroUI._ScrollRect.verticalScrollbar;
                    if (_vb.gameObject.activeSelf)
                    {
                        _vb.value = 1;
                    }
                    break;
            }
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
