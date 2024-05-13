using System;
using System.Collections;
using System.Collections.Generic;
using XD.GameStatic;
using XD.UI;
using XD.Map;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using CurvedUI;
using uTools;
using System.Xml;
using UnityEngine.Networking;
using XD.Tasks;


namespace XD.TheManager
{
    public class UIManager : MonoBehaviour
    {
        #region ���л���
        [Serializable]
        public class WarringUI
        {
            public GameObject uiobj;

            public void SetWarringMsg(string _s)
            {

            }
        }

        [Serializable]
        public class LoadingUI
        {
            public GameObject uiobj;
            public Image _Image;

            public void PlaySound(bool _bool)
            {

            }
            public void SetLoadLine(float _f)
            {
                uiobj.SetActive(true);
                _Image.fillAmount = _f;
            }
        }

        [Serializable]
        public class SelectSceneUI
        {
            public GameObject uiobj;
            public Transform Master;
            private List<BtnUnit> AllUnit = new List<BtnUnit>();

            /// <summary>
            /// ��ʼ��UIʱ����ʾ������ť�ؼ���Ѳ�ӡ���բ��
            /// </summary>
            internal void InitStart()
            {
                ClearAll();
                foreach (BtnMenu _d in Instance.menu_1)
                {
                    if (_d.BtnRes.Contains("|"))
                    {
                        string[] _s = _d.BtnRes.Split('|');
                        BtnUnit _res = Manager.Instance.InitResObj(Master, _s[0]).GetComponent<BtnUnit>();
                        _res._Img.sprite = Resources.Load<Sprite>(_s[1]);
                        _res.SetBtn(_d);
                        AllUnit.Add(_res);
                    }
                }
                uiobj.SetActive(true);
            }

            /// <summary>
            /// ����֮ǰ���ɵ�Ԥ����
            /// </summary>
            private void ClearAll()
            {
                foreach (BtnUnit _d in AllUnit)
                    Destroy(_d.gameObject);
                AllUnit.Clear();
            }
        }

        [Serializable]
        public class SelectModeUI
        {
            public GameObject uiobj;
            public Transform Master;
            private List<BtnUnit> AllUnit = new List<BtnUnit>();

            /// <summary>
            /// ѧ����UI������ʾ
            /// </summary>
            internal void InitStart()
            {
                ClearAll();
                foreach (BtnMenu _d in Instance.menu_2)
                {
                    if (_d.BtnRes.Contains("|"))
                    {
                        string[] _s = _d.BtnRes.Split('|');
                        BtnUnit _res = Manager.Instance.InitResObj(Master, _s[0]).GetComponent<BtnUnit>();
                        _res._Img.sprite = Resources.Load<Sprite>(_s[1]);
                        _res.SetBtn(_d);
                        AllUnit.Add(_res);
                    }
                }
                uiobj.SetActive(true);
            }

            /// <summary>
            /// ����֮ǰ���ɵ�Ԥ����
            /// </summary>
            private void ClearAll()
            {
                foreach (BtnUnit _d in AllUnit)
                    Destroy(_d.gameObject);
                AllUnit.Clear();
            }
        }

        [Serializable]
        public class Title
        {
            public GameObject uiobj;
            public List<Toggle> toggles = new List<Toggle>();
            public GameObject taskBg1; //Ѳ�������б�ı���
            public GameObject taskBg2; //��բ�����б�ı���
            public GameObject _taskList; //�����б�����
            public Task task = new Task();
            public GameObject ElecImage; //����ͼ

            internal void InitStart()
            {
                //���е�Toggle���ɽ���
                for (int i = 0; i < toggles.Count; i++)
                {
                    toggles[i].interactable = true;
                }
                //Ϊ����Toggle���ֵ�ı�����¼�
                toggles[0].onValueChanged.AddListener(MapToggleChanged);
                toggles[1].onValueChanged.AddListener(TaskToggleChanged);
                toggles[2].onValueChanged.AddListener(HelpToggleChanged);
                toggles[3].onValueChanged.AddListener(ElecToggleChanged);
                uiobj.SetActive(true);
                task.Init();
            }

            //��ͼToggle
            private void MapToggleChanged(bool isOn)
            {
                //��ȡMain�����еĵ�ͼ
                GameObject.Find("Maps").GetComponent<MiniMap>().minMap.SetActive(isOn);
            }

            //������Toggle
            private void TaskToggleChanged(bool isOn)
            {
                //task.taskTogList.Clear();
                _taskList.SetActive(isOn);
                //�滻Ѳ���뵹բ������������
                switch(GameUnitManager.Instance._SceneType)
                {
                    case SceneType.xunshi:
                        taskBg1.SetActive(isOn);
                        break;
                    case SceneType.daozha:
                        taskBg2.SetActive(isOn);
                        break;
                }
            }

            //����Toggle
            private void HelpToggleChanged(bool isOn)
            {
                //���ص�ǰ�����е�ui
                Instance.RestAllUI();
                if(Instance._ModelUI.ModelRawImg)
                    Instance._ModelUI.ModelRawImg.SetActive(false);
                //ֹͣ�����ڲ��ŵ�����
                AudioPlay.Instance.StopAudio();
                //ֹͣ��������
                AudioSource ZBVoice = GameObject.Find("ZhubianVoice").GetComponent<AudioSource>();
                if (ZBVoice)
                {
                    ZBVoice.Stop();
                }
                Instance._GuideUI.Promuiobj.SetActive(true);
            }

            //����ͼToggle
            private void ElecToggleChanged(bool isOn)
            {
                ElecImage.SetActive(isOn);
            }
        }

        [Serializable]
        public class IntroUI
        {
            public GameObject uiobj;
            public Image bg;
            public Transform TagObj;  //��ǩ������
            internal List<BtnUnit> tags = new List<BtnUnit>();
            public List<GameObject> Content = new List<GameObject>();
            public ScrollRect _ScrollRect;
            public GameObject OverBtn;  //�鿴��ϰ�ť
            private int index;
            private string stateCode;
            internal bool IsImmediate = true;   //��������
            internal PlayerMove pm;    //�����ڲ鿴����uiʱ����Ӱ�쵽����϶��ı������ӽ�

            /// <summary>
            /// ��ʼ��IntroUI
            /// </summary>
            /// <param name="prefab">Ԥ����·��</param>
            /// <param name="sprite">IntroUI����ͼ·��</param>
            internal void InitStart(string prefab, string sprite)
            {
                OverBtn.SetActive(false);
                if (tags != null)
                    ClearData();

                bg.sprite = Resources.Load<Sprite>(sprite);
                index = 0;  //��ǩ��Ŵ�0��ʼ����
                foreach (SubList _d in GameUnitManager.Instance.SubBtn)
                {
                    //������ǩ��Ӧ�����ı�����ͼƬ
                    bool _tag = AnalyzeContents(_d);
                    //���ر�ǩԤ����
                    BtnUnit _res = Manager.Instance.InitResObj(TagObj, prefab).GetComponent<BtnUnit>();
                    //���ñ�ǩ��ť��Ϣ
                    _res.SetSubBtn(index, _d, stateCode, _tag);
                    tags.Add(_res);
                    index++;
                }

                Content[2].SetActive(false);
                uiobj.SetActive(true);
                //��ʼ����������������
                if (_ScrollRect.verticalScrollbar.gameObject.activeSelf)
                {
                    _ScrollRect.verticalScrollbar.value = 1;
                }

                //�ڿ�IntroUIʱ�ر������ӽǵ���ת
                pm = GameObject.Find("Player").GetComponent<PlayerMove>();
                pm.IsRotationControl = false;
            }

            /// <summary>
            /// �����ı����ݣ��ı�/ͼƬ��
            /// </summary>
            /// <param name="sl"></param>
            /// <returns></returns>
            private bool AnalyzeContents(SubList sl)
            {
                bool flag = true;
                if (sl.ContentData.Contains(":"))
                {
                    string[] _s = sl.ContentData.Split(':');
                    switch (_s[0])
                    {
                        case "txt":
                            flag = true;
                            break;
                        case "pic":
                            flag = false;
                            break;
                    }
                    stateCode = _s[1];
                }
                return flag;
            }

            /// <summary>
            /// ���ٲ������ǩ�б���Ϣ
            /// </summary>
            private void ClearData()
            {
                foreach (BtnUnit _d in tags)
                    Destroy(_d.gameObject);
                tags.Clear();
            }

            /// <summary>
            /// �������
            /// </summary>
            internal void RestComp()
            {
                if (Content[1].GetComponent<Text>())
                {
                    DestroyImmediate(Content[1].GetComponent<Text>());
                }
                else if (Content[1].GetComponent<Image>())
                {
                    DestroyImmediate(Content[1].GetComponent<Image>());
                }
                else
                {
                    return;
                }
            }
        }

        [Serializable]
        public class ModelUI
        {
            public GameObject uiobj;
            //��ǩ��ť���壬��Inspector����
            public List<Transform> TagObj = new List<Transform>();
            public GameObject ModelRawImg;
            public Image ModelIntroImg;
            public List<GameObject> ActiveObj;
            //�洢�����ѳ�ʼ����ɵı�ǩ��Ϣ
            internal List<ModelToggle> toggleList = new List<ModelToggle>();
            //�洢����ģ����Ϣ
            internal SortedList<int, ModelTable> models = new SortedList<int, ModelTable>();

            private int index;   //��ǩ��ť��ID
            internal int tgid;   //Ŀǰ����ִ�еı�ǩID
            private int tgmax;   //Ŀǰ�ѿ�����ɵı�ǩ���ID
            internal int tgtmp;  //��ʱ�洢Ŀǰ����ִ�еı�ǩID

            //����ģ�Ͳ������
            internal int Objstep;
            //��ǩ��Ӧ��ģ�Ͳ����ʼ��������
            internal int stepStart;
            internal int stepEnd;
            //��ǩ��Ӧ��ģ�Ͳ������
            internal int step;  
            internal URPHighlightableObject hc;
            internal int toggleType;

            //����ڲ��ṹ����ʾ��ģ��
            internal GameObject _innerobj;

            //����������ť
            public GameObject HearBtn;
            //�·�������ʾ
            public GameObject _Tips;

            internal PlayerMove pm;    //�����ڲ鿴ģ�ͽ���ʱ����Ӱ�쵽�������϶��ı������ӽ�

            /// <summary>
            /// ��ʼ������ģ��չʾUI
            /// </summary>
            internal void InitStart()
            {
                switch (GameUnitManager.Instance._SceneType)
                {
                    case SceneType.xunshi:
                        AnalyzeModels();  //������ȡ��Ҫչʾ��ģ�Ͳ���
                        ModelRawImg = GameObject.Find("Canvas").transform.Find("ModelRaw").gameObject;
                        ModelRawImg.SetActive(true);
                        index = 1;   //��ǩ��ť��ID��1��ʼ����
                        toggleList.Clear();
                        foreach (BtnMenu _d in Instance.menu_3)
                        {
                            ModelToggle _res;
                            if (!TagObj[index - 1].gameObject.GetComponent<ModelToggle>())
                                _res = TagObj[index - 1].gameObject.AddComponent<ModelToggle>();
                            else
                                _res = TagObj[index - 1].gameObject.GetComponent<ModelToggle>();
                            _res._toggle = TagObj[index - 1].gameObject.GetComponent<Toggle>();
                            //���ñ�ǩ��Ӧ����Ϣ
                            _res.SetModelToggle(index, _d.BtnData, _d.BtnRes);
                            toggleList.Add(_res);
                            index++;
                        }
                        uiobj.SetActive(true);
                        /*if(PlayerMove.Instance._param.URPCam != null)
                            //���������������outline��
                            PlayerMove.Instance._param.URPCam.GetUniversalAdditionalCameraData().volumeLayerMask &= -(1 << 8);*/
                        break;
                }

                pm = GameObject.Find("Player").GetComponent<PlayerMove>();
                pm.IsRotationControl = false;

                tgid = 1;  //Ĭ�ϳ�ʼ�ӵ�һ����ǩ��ʼ����
                SetToggle(1);
            }

            /// <summary>
            /// ������ǰģ�͵����в�������
            /// </summary>
            private void AnalyzeModels()
            {
                int id = 1;
                models.Clear();
                foreach (ModelTable _d in Manager.Instance.AllModels.Values)
                {
                    if (_d.ModelType.Equals(GameUnitManager.Instance._Scene1._modelType))
                    {
                        models.Add(id, _d);
                        id++;
                        if (_d.ImgRes.Contains("|"))
                        {
                            string[] _s = _d.ImgRes.Split('|');
                            tgmax = int.Parse(_s[1]);
                        }
                    }
                }
            }

            /// <summary>
            /// ���ñ�ǩ�����¶�Ӧ����Ϣ
            /// </summary>
            /// <param name="id">�����µı�ǩID</param>
            internal void SetToggle(int id)
            {
                SetTogglePic(id);
                IsToggle();
            }

            /// <summary>
            /// ���ñ�ǩͼ��
            /// </summary>
            /// <param name="id">�����µı�ǩID</param>
            internal void SetTogglePic(int id)
            {
                //�������б�ǩ
                foreach (ModelToggle mt in toggleList)
                {
                    //�����б�ǩ�����óɹر�״̬
                    mt._toggle.isOn = false;
                    mt.transform.gameObject.GetComponent<Image>().sprite = mt.clickBefore;
                    //�����������ı�ǩ�����豻���µı�ǩ
                    if (mt.ID == id)
                    {
                        //�򿪸ñ�ǩ
                        mt._toggle.isOn = true;
                        mt.transform.gameObject.GetComponent<Image>().sprite = mt.clickAfter;
                    }
                }
                if (hc)
                {
                    hc.enabled = false;
                    Manager.Instance.ClearData(Instance._ModelUI.hc.gameObject);
                }
            }

            /// <summary>
            /// �����ǩ�����£�����չʾģ�͵ĳ�ʼֵ
            /// </summary>
            internal void IsToggle()
            {
                //�������б�ǩ
                foreach (ModelToggle mt in toggleList)
                {
                    //�жϸñ�ǩ�Ƿ񱻰���
                    if (mt._toggle.isOn)
                    {
                        DevelopOver(mt.ID);
                    }
                }
            }

            /// <summary>
            /// ���ݱ�ǩ���ݿ������������Ӧ������
            /// </summary>
            /// <param name="ty">��ǩID����ֵ���ǩ����һ��</param>
            private void DevelopOver(int ty)
            {
                //���ñ�ǩ�Ѿ��������
                if (ty <= tgmax)
                {
                    tgtmp = ty;   //��¼��ǰ��ǩ���
                    SetStep(ty);  //���ò����ʼֵ
                    ActiveModelUI(true);  //��ʾģ�����UI
                    Instance._TodoUI.SetActive(false);
                    ExDisplay();  //ִ��ģ�ͽ���
                    _Tips.SetActive(true);
                }
                else  //��ǩ��Ӧ����δ�������
                {
                    ActiveModelUI(false); //�ر�ģ�����UI
                    AudioPlay.Instance.StopAudio(); //ֹͣ��������
                    Instance._TodoUI.SetActive(true); //��δ������ʾ��Ϣ
                    _Tips.SetActive(false);
                }
            }

            /// <summary>
            /// ����ģ�ͽ������UI
            /// </summary>
            /// <param name="_bool">�Ƿ�Ҫ������ʾ</param>
            private void ActiveModelUI(bool _bool)
            {
                foreach (GameObject _o in ActiveObj)
                {
                    _o.SetActive(_bool);
                }
                ModelRawImg.SetActive(_bool);
            }

            /// <summary>
            /// ���ñ�ǩ��Ӧģ�ͳ�ʼֵ
            /// </summary>
            /// <param name="toggleType">��ǩ���ͱ��</param>
            internal void SetStep(int toggleType)
            {
                List<ModelTable> temp = new List<ModelTable>();
                //��������ģ�Ͳ���
                foreach (ModelTable mt in models.Values)
                {
                    if (mt.ImgRes.Contains("|"))
                    {
                        string[] _s = mt.ImgRes.Split('|');
                        //��¼���ϵ�ǰ��ǩ��ģ�Ͳ�����
                        if (int.Parse(_s[1]) == toggleType)
                        {
                            temp.Add(mt);
                            //ImgRes�����������洢�������ʼֵ
                            Objstep = int.Parse(_s[2]);
                        }
                    }
                }
                stepStart = 1;  //��ǩ��Ӧ��ģ�Ͳ�����ʼֵ
                stepEnd = temp.Count; //��ǩ��Ӧ��ģ�Ͳ�������
                step = 1;  //Ĭ�ϱ�ǩ��Ӧ��ģ�Ͳ�����1��ʼ����
            }

            /// <summary>
            /// ����UI�е�ģ�ͽ���
            /// </summary>
            internal void ExDisplay()
            {
                if (step >= stepStart && step <= stepEnd)
                {
                    //����
                    GameObject _obj = GameObject.Find(models[Objstep].ModelObj);
                    if (_obj)
                    {
                        hc = _obj.AddComponent<URPHighlightableObject>();
                    }
                    //ѧϰ������ڲ��ṹ,ֻ��ʾ����
                    if (models[Objstep].BtnRes.Contains("Split"))
                    {
                        SplitModel.Instance.SplitObject();
                    }
                    else if(models[Objstep].BtnRes.Contains("Merge"))
                    {
                        SplitModel.Instance.MergeObject();
                    }
                    if(models[Objstep].BtnRes.Contains("Transparent"))
                    {
                        TransparentModel.Instance.TransparentOthers(GameObject.Find("zb_a_outside_No0").gameObject.name);
                    }
                    else if(models[Objstep].BtnRes.Contains("TOil") && models[Objstep].BtnRes.Contains("Transparent"))
                    {
                        TransparentModel.Instance.TransparentOthers(GameObject.Find("zb_a_inside_No2").gameObject.name);
                    }
                    else
                    {
                        TransparentModel.Instance.RecoveryTransparent();
                    }
                    //ģ�ͽ���ͼ
                    if (models[Objstep].ImgRes.Contains("|"))
                    {
                        string[] _s = models[Objstep].ImgRes.Split('|');
                        ModelIntroImg.sprite = Resources.Load<Sprite>(_s[0]);
                    }
                    //����������ť
                    if (models[Objstep].BtnRes.Contains("Hearing"))
                    {
                        HearBtn.SetActive(true);
                    }
                    else
                    {
                        HearBtn.SetActive(false);
                    }
                    //ͼƬ����Ӧ
                    ModelIntroImg.SetNativeSize();
                    //����ģ�ͽ�������
                    AudioPlay.Instance.audioClip = Resources.Load<AudioClip>(models[Objstep].SoundRes);
                    AudioPlay.Instance.PlayAudio();
                }
            }
        }

        [Serializable]
        public class DaoZhaUI
        {
            public GameObject uiobj;
            public GameObject btn;
            public DianHuaUI dianHuaUI;
            public HuiFuUI huiFuUI;
            public QueRenUI queRenUI;
            public PopUpUI popUpUI;
            public ToolIntro toolIntro;
            public GameObject clickTag;

            internal ControlBtn cb;
            internal int ActiveArea;   //��ǰ�Ի�����
            internal bool IsAlpha = false;

            /// <summary>
            /// UI��ʼ����
            /// </summary>
            internal void RestUI()
            {
                btn.SetActive(false);
                dianHuaUI.Rest();
                huiFuUI.master.SetActive(false);
                queRenUI.master.SetActive(false);
                popUpUI.Rest();
                toolIntro.master.SetActive(false);
                clickTag.SetActive(false);
            }

            /// <summary>
            /// ���ð�ť
            /// </summary>
            /// <param name="pic1">��ť����ǰͼƬ</param>
            /// <param name="pic2">��ť������ͼƬ</param>
            internal void SetBtn(string pic1, string pic2)
            {
                //���ð�ť�¼�
                cb = btn.GetComponent<ControlBtn>();
                cb.btnType = BtnType.Prompt;
                cb.Init(pic1, pic2);
                //��ťͼƬ��ȡ
                btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(pic1);
                btn.GetComponent<Image>().SetNativeSize();
                //��ť��ʾ
                btn.SetActive(true);
            }

            /// <summary>
            /// ����͸����
            /// </summary>
            internal void SetAlpha()
            {
                IsAlpha = false;
                //��ǰ�Ի�������ֵ�೤����
                if (ActiveArea == 1)
                {
                    uTweenAlpha.Begin(dianHuaUI.bg, 1f, 0f, 1f, 0.5f);
                    uTweenAlpha.Begin(dianHuaUI.content.gameObject, 1f, 0f, 1f, 0.5f);
                }
                else if (ActiveArea == 2)  //��ǰ�Ի������ǲ���������
                {
                    uTweenAlpha.Begin(huiFuUI.bg, 1f, 0f, 1f, 0.5f);
                    uTweenAlpha.Begin(huiFuUI.content.gameObject, 1f, 0f, 1f, 0.5f);
                    clickTag.SetActive(false);
                }
            }
        }

        /// <summary>
        /// ��բѧϰģʽ��ÿһ�󲽽��������ʾ��Ϣ
        /// </summary>
        [Serializable]
        public class DzTipUI
        {
            public GameObject uiobj;
            public Image tip;
            public GameObject btn1;
            public GameObject btn2;
            internal int step;
            private int stepStart;

            /// <summary>
            /// ��ʾUI��ʼ����
            /// </summary>
            /// <param name="_s">��ǰ���ڱ��</param>
            /// <param name="start">��ǰ���ڵĳ�ʼ����</param>
            /// <param name="area">��ǰ�������һ�����ڳ���������</param>
            internal void Init(string _s, string start, string area)
            {
                step = int.Parse(_s);
                stepStart = int.Parse(start);
                //�������л��ڽ�����ʾ��Ϣ
                foreach (DzStepTip dt in Manager.Instance.dzTips.Values)
                {
                    //��ȡ����ǰ���ڶ�Ӧ����ʾ��Ϣ
                    if (dt.ID == step)
                    {
                        //������ʾ��ϢUI
                        AnalyzeTip(dt, area);
                        uiobj.SetActive(true);
                    }
                }
            }

            /// <summary>
            /// �����ݿ��е���Ϣ�������õ�UI��
            /// </summary>
            /// <param name="dt">���ݿ��д洢����ʾ��Ϣ�������</param>
            /// <param name="area">��ǰ�������һ�����ڳ���������</param>
            private void AnalyzeTip(DzStepTip dt, string area)
            {
                //��ť������֮ǰ��ͼƬ
                string[] _s1 = dt.BtnBefore.Split('|');
                //��ť������֮���ͼƬ
                string[] _s2 = dt.BtnAfter.Split('|');
                //��ʾ��Ϣ����
                tip.sprite = Resources.Load<Sprite>(dt.ImgRes);
                //��ȡ��ť�ϵ�TipBtn���
                TipBtn tb1 = btn1.GetComponent<TipBtn>();
                TipBtn tb2 = btn2.GetComponent<TipBtn>();
                //����������ť�İ�ť����
                tb1.btnType = BtnType.TipBtn1;
                tb2.btnType = BtnType.TipBtn2;
                //��ʼ��������ť����Ϣ
                tb1.Init(_s1[0], _s2[0], stepStart, area);
                tb2.Init(_s1[1], _s2[1], stepStart, area);
            }
        }

        /// <summary>
        /// Ѳ��ʵ��ģʽ��ѡ��UI
        /// </summary>
        [Serializable]
        public class SelectPracticeUI
        {
            public GameObject uiobj; //UI����
            public Transform Master; //�����ɰ�ťԤ����ĸ�����λ��
            private SortedList<int, CkPBtn> AllUnit = new SortedList<int, CkPBtn>();
            private int id = 0;

            /// <summary>
            /// Ѳ��ѡ��UI��ť��ʾ
            /// </summary>
            internal void InitStart()
            {
                Instance._SelectModeUI.uiobj.SetActive(false);
                ClearAll();
                foreach (BtnMenu _d in Instance.menu_4)
                {
                    if (_d.BtnRes.Contains("|"))
                    {
                        string[] _s = _d.BtnRes.Split('|');
                        CkPBtn _res = Manager.Instance.InitResObj(Master, _s[0]).GetComponent<CkPBtn>();
                        _res._Img.sprite = Resources.Load<Sprite>(_s[1]);
                        _res.SetBtn(id, _d);
                        AllUnit.Add(id, _res);
                        id++;
                    }
                }
                uiobj.SetActive(true);
            }


            /// <summary>
            /// ����֮ǰ���ɵ�Ԥ����
            /// </summary>
            private void ClearAll()
            {
                foreach (CkPBtn _d in AllUnit.Values)
                    Destroy(_d.gameObject);
                AllUnit.Clear();
            }
        }

        [Serializable]
        public class CheckPracticeUI
        {
            public GameObject uiobj;
            public HintUI hintUI;
            public NextUI nextUI;
            public JumpUI jumpUI;
            public AudioSource ZhubianVoice; //�����������

            internal void RestUI()
            {
                uiobj.SetActive(false);
                hintUI.Rest();
                nextUI.Rest();
                jumpUI.Rest();
            }

            internal void InitHint()
            {
                uiobj.SetActive(true);
                nextUI.Rest();
                jumpUI.Rest();
            }

        }

        [Serializable]
        public class GuideUI
        {
            public GameObject uiobj;
            public GameObject Promuiobj;    //�Ƿ�鿴�̳�ui
            public Button yesBtn;   //�ǰ�ť
            public Button noBtn;    //��ť
            public Button closeBtn; //���

            public GuidanceUI guidanceUI;
            public Transform _ui;
            internal int index = 1;
            private SortedList<int, LightTable> light = new SortedList<int, LightTable>();

            public GameObject Doneuiobj;    //ѧϰ�̳����ui
            public Button againBtn;     //�������鰴ť
            public Button nextBtn;      //����̽����ť

            internal void Init()
            {
                light = Manager.Instance.AllLightData;
                if (!uiobj.activeSelf)
                    uiobj.SetActive(true);
                Promuiobj.SetActive(true);
                //AnalyzeLightData();
                yesBtn.onClick.AddListener(OnYesBtn);
                noBtn.onClick.AddListener(OnNoBtn);
                closeBtn.onClick.AddListener(OnNoBtn);
                guidanceUI.Rest();
                Doneuiobj.SetActive(false);
                againBtn.onClick.AddListener(OnAgainBtn);
                nextBtn.onClick.AddListener(OnNextBtn);
            }

            internal void RestUI()
            {
                Promuiobj.SetActive(false);
                Doneuiobj.SetActive(false);
            }

            internal void OnYesBtn()
            {
                //����ʵ�ʵ�ģ��
                Instance._ModelUI.uiobj.SetActive(false);
                if(Instance._ModelUI.ModelRawImg)
                    Instance._ModelUI.ModelRawImg.SetActive(false);
                RestUI();
                guidanceUI.Init();
                guidanceUI.target = GameObject.Find(light[index].LightTarget).gameObject.GetComponent<RectTransform>();
                //����
                guidanceUI.guideController.info.text = light[index].TextResL;
                guidanceUI.guideController.hint.text = light[index].TextResR;
                guidanceUI.guideController.Guide(guidanceUI.target, guidanceUI.isSoft, guidanceUI.scale, guidanceUI.time);
                guidanceUI.target.gameObject.GetComponent<Button>().onClick.AddListener(OnNextLight);
            }

            internal void OnNextLight()
            {
                if (index.Equals(light.Count))
                {
                    index = 1;
                    return;
                }
                index++;
                if (light[index].LightTarget.Contains(","))
                {
                    string[] _s = light[index].LightTarget.Split(',');
                    _ui = guidanceUI.uiobj.transform.Find(_s[0]);
                    if (_s[1].Equals("1"))
                    {
                        _ui.gameObject.SetActive(true);
                    }
                    else if (_s[1].Equals("2"))
                    {
                        _ui.gameObject.SetActive(false);
                    }
                    OnNextLight();
                }
                else
                {
                    guidanceUI.target = GameObject.Find(light[index].LightTarget).gameObject.GetComponent<RectTransform>();
                    guidanceUI.guideController.info.text = light[index].TextResL;
                    guidanceUI.guideController.hint.text = light[index].TextResR;
                    guidanceUI.guideController.Guide(guidanceUI.target, guidanceUI.isSoft, guidanceUI.scale, guidanceUI.time);
                    if (guidanceUI.target.gameObject.GetComponent<Button>())
                    {
                        guidanceUI.target.gameObject.GetComponent<Button>().onClick.AddListener(OnNextLight);
                    }
                }

            }
            internal void OnNoBtn()
            {
                RestUI();
                GameUnitManager.Instance.isGuide = false;
                GoOnTheStep();
            }

            /// <summary>
            /// �̳���ʾ��Ϣ�����������������ƫ��һ��λ��
            /// </summary>
            /// <param name="arrow">������ͷͼ��</param>
            /// <param name="m_Target">����������</param>
            internal void MoveText(Image arrow, RectTransform m_Target)
            {
                if (light[index].LorRDistance.Contains("|"))
                {
                    string[] _s = light[index].LorRDistance.Split('|');
                    int dis;
                    switch (_s[0].ToLower())
                    {
                        case "right":
                            dis = int.Parse(_s[1]);
                            arrow.gameObject.GetComponent<RectTransform>().position = m_Target.position + Vector3.down * light[index].DownDistance + Vector3.right * dis;
                            break;
                        case "left":
                            dis = int.Parse(_s[1]);
                            arrow.gameObject.GetComponent<RectTransform>().position = m_Target.position + Vector3.down * light[index].DownDistance + Vector3.left * dis;
                            //�滻������ͷ�ĳ���
                            arrow.sprite = Resources.Load<Sprite>("UI/Pics/xd-ydjt0");
                            break;
                    }
                }
            }

            internal void OnAgainBtn()
            {
                RestUI();
                index = 1;
                GameUnitManager.Instance.isGuide = true;
                guidanceUI.Init();
                guidanceUI.target = GameObject.Find(light[index].LightTarget).gameObject.GetComponent<RectTransform>();
                guidanceUI.guideController.info.text = light[index].TextResL;
                guidanceUI.guideController.hint.text = light[index].TextResR;
                guidanceUI.guideController.Guide(guidanceUI.target, guidanceUI.isSoft, guidanceUI.scale, guidanceUI.time);
                guidanceUI.target.gameObject.GetComponent<Button>().onClick.AddListener(OnNextLight);
            }

            internal void OnNextBtn()
            {
                RestUI();
                GoOnTheStep();
                uiobj.SetActive(false);
            }

            /// <summary>
            /// �ж��Ƿ��ǳ����������Ƿ������һ��ִ�еĲ���
            /// </summary>
            internal void GoOnTheStep()
            {
                //���ǳ�������
                if (GameUnitManager.Instance.isFirst)
                {
                    Instance._Title.InitStart();
                    GameUnitManager.Instance.SelectUnit();
                    GameUnitManager.Instance.isFirst = false;
                }
                else
                {
                    Instance._Title.uiobj.SetActive(true);
                    //���ݵ�ǰ��������ִ�е�ǰ����
                    switch (GameUnitManager.Instance._SceneType)
                    {
                        case SceneType.xunshi:
                            switch (GameUnitManager.Instance._EventSystemType)
                            {
                                case EventSystemType.Study:
                                    GameUnitManager.Instance._Scene1.ExOperate();
                                    break;
                                case EventSystemType.Practice:
                                    GameUnitManager.Instance._ScenePractice.ExOperate();
                                    break;
                            }
                            break;
                        case SceneType.daozha:
                            GameUnitManager.Instance._Scene2.ExOperate();
                            break;
                    }
                }
            }
        }


        [Serializable]
        public class ExamUI
        {
            public GameObject uiobj;
            public Text time;
            public Text record;
            //���⿨
            public GameObject ecMaster;
            //�����Ľ����
            public AnswerCard answerCard;
            //����δȫ�����ȷ�Ͽ�
            public GameObject cfMaster;
            //����ɹ�
            public GameObject resMaster;
            //�÷�
            public Text Point;
            //��ѡ�⸸����
            public Transform sinBtnContent;
            //��ѡ�⸸����
            public Transform mulBtnContent;
            //����⸸����
            public Transform judgeBtnContent;
            //����ť
            public Button submit;
            public Button previousBtn;//��һ�ⰴť
            public Button nextBtn;//��һ�ⰴť
            //��Ŀ����
            public Paper paper;
            //�����Ŀ�б����ݣ��洢��Ŀ��ť����Ӧ��Ŀ����
            internal List<QuestionNum> _question = new List<QuestionNum>();
            internal int questionCount;//��ǰ��Ŀ���
            internal int answerCount; //��ǰ�����
            private int id; //��Ŀ����
            internal int SelectedNum;
            internal bool isSubmit;
            internal int score;  //�÷�
            private Coroutine stopC;

            //20221117���������Ҫ�����ֳ�����Ŀ���
            internal SortedList<int, FaultState> faultScene = new SortedList<int, FaultState>();

            //20221128������ѡ����Ŀ
            public Button modifyBtn;
            public ExamChoiceUI _examChoiceUI = new ExamChoiceUI();

            /// <summary>
            /// ����UI��ʼ��
            /// </summary>
            internal void Init()
            {
                uiobj.SetActive(true);
                _examChoiceUI.Init();
                SetQuestion();
            }

            internal void SetQuestion()
            {
                ecMaster.SetActive(true);
                answerCard.master.SetActive(false);
                cfMaster.SetActive(false);
                questionCount = 0;
                answerCount = 0;
                id = 0;
                SelectedNum = 0;
                score = 0;
                isSubmit = false;
                time.text = "00��00��";
                ClearObj();
                faultScene.Clear();
                Instance.StopAllCoroutines();
                //��Ӱ�ť��������¼�
                previousBtn.onClick.AddListener(PreviousClick);
                nextBtn.onClick.AddListener(NextClick);
                submit.onClick.AddListener(SubmitClick);
                switch (GameUnitManager.Instance._SceneType)
                {
                    case SceneType.xunshi:
                        //��ѡ
                        InitPanel(_examChoiceUI.XsSingleTopic, 10, 1);
                        //��ѡ
                        InitPanel(_examChoiceUI.XsDoubleTopic, 5, 2);
                        //�ж�
                        InitPanel(_examChoiceUI.XsJudgeTopic, 5, 3);
                        break;
                    case SceneType.daozha:
                        //��ѡ
                        InitPanel(_examChoiceUI.DzSingleTopic, 10, 1);
                        //��ѡ
                        InitPanel(_examChoiceUI.DzDoubleTopic, 5, 2);
                        //�ж�
                        InitPanel(_examChoiceUI.DzJudgeTopic, 5, 3);
                        break;
                }
                record.transform.parent.GetComponent<Text>().text = "�Ѵ��⣺";
                record.text = SelectedNum.ToString() + "/20";
                //��ʼ��ʾ��һ��
                SetPaper(questionCount);
                Instance.spendTime = 0;
                stopC = Instance.StartCoroutine(Instance.CountTime());
            }

            /// <summary>
            /// �������
            /// </summary>
            private void ClearObj()
            {
                _question.Clear();
                answerCard._answer.Clear();
                ClearQstBtnItem(sinBtnContent);
                ClearQstBtnItem(mulBtnContent);
                ClearQstBtnItem(judgeBtnContent);
                ClearTagBtnItem(answerCard.sinTagContent);
                ClearTagBtnItem(answerCard.mulTagContent);
                ClearTagBtnItem(answerCard.judgeTagContent);
                ClearToggleItem(paper.answer);
                //�Ƴ���ť�ϵĵ���¼�����
                previousBtn.onClick.RemoveAllListeners();
                nextBtn.onClick.RemoveAllListeners();
                submit.onClick.RemoveAllListeners();
            }

            /// <summary>
            /// ���ٴ��⿨�ϵ�Ԥ����
            /// </summary>
            /// <param name="master"></param>
            private void ClearQstBtnItem(Transform master)
            {
                QuestionBtnItem[] qst = master.gameObject.GetComponentsInChildren<QuestionBtnItem>();
                if (qst.Length > 0)
                {
                    foreach (QuestionBtnItem _qst in qst)
                    {
                        Destroy(_qst.gameObject);
                    }
                }
            }

            /// <summary>
            /// ���ٽ���������ϵ�Ԥ����
            /// </summary>
            /// <param name="master"></param>
            private void ClearTagBtnItem(Transform master)
            {
                TagBtnItem[] tag = master.gameObject.GetComponentsInChildren<TagBtnItem>();
                if (tag.Length > 0)
                {
                    foreach (TagBtnItem _tag in tag)
                    {
                        Destroy(_tag.gameObject);
                    }
                }
            }

            /// <summary>
            /// ���ٴ�ѡ��Ԥ����
            /// </summary>
            /// <param name="master"></param>
            private void ClearToggleItem(Transform master)
            {
                ToggleItem[] toggle = master.gameObject.GetComponentsInChildren<ToggleItem>();
                if (toggle.Length > 0)
                {
                    foreach (ToggleItem _toggle in toggle)
                    {
                        Destroy(_toggle.gameObject);
                    }
                }
            }

            /// <summary>
            /// ��ʼ���������
            /// </summary>
            /// <param name="qd">����ж�Ӧ���͵�������Ŀ</param>
            /// <param name="count">��������������Ŀ����</param>
            /// <param name="type">��������</param>
            private void InitPanel(List<QuestionItemDatas> qd, int count, int type)
            {
                //�����ȡ��Ŀ����
                List<QuestionItemDatas> temp = qd.GetRandomChilds<QuestionItemDatas>(count);
                for (int i = 0; i < temp.Count; i++)
                {
                    switch (type)
                    {
                        case 1:
                            SetQuestion(i, temp[i], "UI/Pics/xd-kh-dxt", sinBtnContent);
                            break;
                        case 2:
                            SetQuestion(i, temp[i], "UI/Pics/xd-kh-dx", mulBtnContent);
                            break;
                        case 3:
                            SetQuestion(i, temp[i], "UI/Pics/xd-kh-pdt", judgeBtnContent);
                            break;
                    }
                    //20221129����������Ŀ��������
                    if (temp[i].existFault == 1)
                    {
                        if (!faultScene.ContainsKey(id))
                        {
                            FaultState fs = new FaultState();
                            fs.qID = temp[i].questionID;
                            fs.showFault = temp[i].showFault;
                            faultScene.Add(id, fs);
                        }
                    }
                    id++;
                }
            }

            /// <summary>
            /// ������Ŀ����
            /// </summary>
            /// <param name="i">��Ŀ���</param>
            /// <param name="temp">��Ӧ��Ŀ�������</param>
            /// <param name="pic">��Ŀ����</param>
            /// <param name="btnContent">��Ŀ��ťԤ�����ڴ��⿨�ϵĸ�����</param>
            private void SetQuestion(int i, QuestionItemDatas temp, string pic, Transform btnContent)
            {
                //��Ŀ����
                QuestionItem qi = new QuestionItem();
                ToggleGroup BtnGroup;
                //��Ŀ��Ӧ��ť
                QuestionBtnItem btnItem;
                QuestionNum questionNum;

                qi.bg = pic;
                qi.qItemData = temp;
                BtnGroup = btnContent.transform.parent.GetComponent<ToggleGroup>();
                //ʵ�������⿨��ĿToggle
                btnItem = Manager.Instance.InitResObj(btnContent, "UI/Prefabs/QuestionTogg").GetComponent<QuestionBtnItem>();
                btnItem.Init(id, i + 1);
                btnItem.gameObject.GetComponent<Toggle>().group = BtnGroup;
                //��Toggle����Ŀ����
                questionNum = new QuestionNum(i + 1, btnItem, qi);
                _question.Add(questionNum);
            }

            /// <summary>
            /// ���õ�ǰ��ĿӦ��ҳ������ʾ������
            /// </summary>
            /// <param name="index"></param>
            internal void SetPaper(int index)
            {
                //20221117�������������ϵ�
                Instance._FaultModUI.Init();
                foreach (int i in faultScene.Keys)
                {
                    if(i == index)
                    {
                        Instance._FaultModUI.SetScene(faultScene[i]);
                    }
                }
                _question[index].SetVisible(index, paper);
            }

            /// <summary>
            /// ��һ�����¼�
            /// </summary>
            private void PreviousClick()
            {
                //�ж��Ǵ��⿨���Ǵ𰸿�
                if (!isSubmit)
                {
                    if (questionCount > 0)
                    {
                        questionCount--;
                        CheckOtherQuestion();
                        //�޸���Ŀ��ť״̬
                        _question[questionCount].bitem.ChangeQuestionState(QuestionState.Select);
                        SetPaper(questionCount);
                    }
                }
                else
                {
                    if (answerCount > 0)
                    {
                        answerCount--;
                        answerCard.SetPaper(paper, answerCount);
                    }
                }
            }
            /// <summary>
            /// ��һ�����¼�
            /// </summary>
            private void NextClick()
            {
                //�ж��Ǵ��⿨���Ǵ𰸿�
                if (!isSubmit)
                {
                    if (questionCount < _question.Count - 1)
                    {
                        questionCount++;
                        CheckOtherQuestion();
                        _question[questionCount].bitem.ChangeQuestionState(QuestionState.Select);
                        SetPaper(questionCount);
                    }
                }
                else
                {
                    if (answerCount < answerCard._answer.Count - 1)
                    {
                        answerCount++;
                        answerCard.SetPaper(paper, answerCount);
                    }
                }
            }
            /// <summary>
            /// �������δ�������Ŀ
            /// </summary>
            private void CheckOtherQuestion()
            {
                for (int i = 0; i < _question.Count; i++)
                {
                    if (!_question[i].bitem.isSelectAnswer)
                    {
                        _question[i].bitem.ChangeQuestionState(QuestionState.Default);//δ����ָ�Ĭ��״̬
                    }
                }
            }
            /// <summary>
            /// �ύ�¼�
            /// </summary>
            private void SubmitClick()
            {
                CalculateScore(); //����������
                for (int i = 0; i < _question.Count; i++)
                {
                    //�ж��Ƿ�������Ŀ���Ѵ���
                    if (!_question[i].bitem.isSelectAnswer)
                    {
                        cfMaster.SetActive(true);
                        return;
                    }
                }
                ConfirmSubmit(); //������Ŀ���꣬ȷ�Ͻ���
            }

            /// <summary>
            /// ����ɹ�
            /// </summary>
            internal void ConfirmSubmit()
            {
                cfMaster.SetActive(false);
                resMaster.SetActive(true);
                Point.text = score.ToString() + "��";
                Instance.StopCoroutine(stopC);  //�رռ�ʱЭ��
            }

            /// <summary>
            /// ȡ������
            /// </summary>
            internal void CancelSubmit()
            {
                isSubmit = false;
                cfMaster.SetActive(false);
                resMaster.SetActive(false);
            }

            /// <summary>
            /// �鿴����
            /// </summary>
            internal void LookAnswer()
            {
                isSubmit = true;
                resMaster.SetActive(false);
                ecMaster.SetActive(false);
                record.transform.parent.GetComponent<Text>().text = "�����ʣ�";
                time.text = string.Format("{0:D2}��{1:D2}��", Instance.minute, Instance.second);
                answerCard.master.SetActive(true);
                answerCard.Init(_question);
                answerCard.SetPaper(paper, answerCount);
            }

            /// <summary>
            /// �ٴδ���
            /// </summary>
            internal void Again()
            {
                isSubmit = false;
                resMaster.SetActive(false);
                Init();
            }

            /// <summary>
            /// �����ֵ
            /// </summary>
            private void CalculateScore()
            {
                foreach (QuestionNum _q in _question)
                {
                    if (_q.answerResult)
                    {
                        score += (int)_q.qitem.qItemData.proScore;
                    }
                }
            }

            /// <summary>
            /// 20221128����������޸���Ŀ��ť����������
            /// </summary>
            internal void ModifyClick()
            {
                Instance._FaultModUI.uiobj.SetActive(false);
                _examChoiceUI.SetChoice();
                _examChoiceUI.uiobj.SetActive(true);
            }
        }

        /// <summary>
        /// 20221128���������˿���ѡ��
        /// </summary>
        [Serializable]
        public class ExamChoiceUI
        {
            public GameObject uiobj;
            public Transform ContentParent;  //����Ԥ���常����
            public Button cancelBtn;         //ȡ����ť
            public Button confirmBtn;        //ȷ�ϰ�ť
            public GameObject tipUI;         //δ����ѡ��������������ʾ��
            public Text[] TopicCnt;          //��ǰѡ�е���Ŀ����������ʾ
            internal bool isFirst = true;    //�Ƿ��ǵ�һ�γ�ʼ��

            private Coroutine stop;
            internal int sin = 0;
            internal int dou = 0;
            internal int judg = 0;

            //Ѳ�ӿ�����Ŀ����ѡ����ѡ���жϣ�
            internal List<QuestionItemDatas> XsSingleTopic;
            internal List<QuestionItemDatas> XsDoubleTopic;
            internal List<QuestionItemDatas> XsJudgeTopic;
            //��բ������Ŀ����ѡ����ѡ���жϣ�
            internal List<QuestionItemDatas> DzSingleTopic;
            internal List<QuestionItemDatas> DzDoubleTopic;
            internal List<QuestionItemDatas> DzJudgeTopic;

            /// <summary>
            /// ѡ��ṹ��
            /// </summary>
            internal struct Choice
            {
                public QuestionItemDatas qData;
                public bool isKaoHe;
                public FaultType type;
            }
            
            internal SortedList<int, Choice> XsChoice = new SortedList<int, Choice>();   //��¼Ѳ����������Ϣ
            internal SortedList<int, Choice> DzChoice = new SortedList<int, Choice>();   //��¼��բ��������Ϣ
            internal SortedList<int, ChoiceUIItem> items = new SortedList<int, ChoiceUIItem>();  //�洢ѡ��Ԥ���������Ϣ

            /// <summary>
            /// ��ʼ���޸�ǰ����
            /// </summary>
            internal void Init()
            {
                uiobj.SetActive(false);
                tipUI.SetActive(false);
                if (isFirst)
                {
                    isFirst = false;
                    XsSingleTopic = Manager.Instance.XsSingleTopic;
                    XsDoubleTopic = Manager.Instance.XsDoubleTopic;
                    XsJudgeTopic = Manager.Instance.XsJudgeTopic;
                    DzSingleTopic = Manager.Instance.DzSingleTopic;
                    DzDoubleTopic = Manager.Instance.DzDoubleTopic;
                    DzJudgeTopic = Manager.Instance.DzJudgeTopic;
                    InitData("xunshi", XsSingleTopic, XsDoubleTopic, XsJudgeTopic);
                    InitData("daozha", DzSingleTopic, DzDoubleTopic, DzJudgeTopic);
                }
            }

            /// <summary>
            /// ��ѡ����Ŀ��������ѡ��
            /// </summary>
            internal void SetChoice()
            {
                SortedList<int, Choice> temp = new SortedList<int, Choice>();
                switch (GameUnitManager.Instance._SceneType)
                {
                    case SceneType.xunshi:
                        temp = XsChoice;
                        break;
                    case SceneType.daozha:
                        temp = DzChoice;
                        break;
                }
                int i = 1;
                foreach(Choice c in temp.Values)
                {
                    string s = "";
                    switch (c.qData.type)
                    {
                        case 1:
                            s = "����ѡ��";
                            break;
                        case 2:
                            s = "����ѡ��";
                            break;
                        case 3:
                            s = "���жϡ�";
                            break;
                    }
                    //�����ݲ��������
                    ChoiceUIItem item = Manager.Instance.InitResObj(ContentParent, "UI/Prefabs/choiceItem").GetComponent<ChoiceUIItem>();
                    item.itemCnt.text = s + c.qData.topic;
                    item.isKaoHeBtn.isOn = c.isKaoHe;
                    //�����Ƿ���Ҫ���ù�����ȷ������ѡ���Ƿ�ɽ���
                    if(c.qData.existFault == 0)
                    {
                        item.faultChoiceTggl.SetActive(false);
                        item.faultChoiceText.SetActive(true);
                    }
                    else if(c.qData.existFault == 1)
                    {
                        item.faultChoiceTggl.SetActive(true);
                        item.faultChoiceText.SetActive(false);
                        foreach (Toggle t in item.choiceBtn)
                        {
                            t.interactable = true;
                        }
                        if (c.type == FaultType.Normal)
                        {
                            item.choiceBtn[0].isOn = true;
                        }
                        else
                        {
                            item.choiceBtn[1].isOn = true;
                        }
                    }
                    //�滻���ݻ��������
                    if (items.ContainsKey(i))
                    {
                        items[i] = item;
                    }
                    else
                    {
                        items.Add(i, item);
                    }
                    i++;
                }
                stop = Instance.StartCoroutine(Instance.CountTopic());
            }

            /// <summary>
            /// ���ѡ��itemԤ����
            /// </summary>
            private void ClearObj()
            {
                ChoiceUIItem[] childs = ContentParent.GetComponentsInChildren<ChoiceUIItem>();
                if (childs.Length != 0)
                {
                    foreach (ChoiceUIItem c in childs)
                    {
                        Destroy(c.gameObject);
                    }
                }
            }

            /// <summary>
            /// ����������
            /// </summary>
            /// <param name="sin"></param>
            /// <param name="dou"></param>
            /// <param name="judg"></param>
            private void ClearData(List<QuestionItemDatas> sin, List<QuestionItemDatas> dou, List<QuestionItemDatas> judg)
            {
                sin.Clear();
                dou.Clear();
                judg.Clear();
            }

            /// <summary>
            /// ��ʼ���б����ݣ�ֻ�ڵ�һ�γ�ʼ��ChoiceUIʱ����
            /// </summary>
            /// <param name="type"></param>
            /// <param name="sin"></param>
            /// <param name="dou"></param>
            /// <param name="judg"></param>
            internal void InitData(string type, List<QuestionItemDatas> sin, List<QuestionItemDatas> dou, List<QuestionItemDatas> judg)
            {
                int i = 1;
                //��ѡ
                foreach(QuestionItemDatas s in sin)
                {
                    Choice c = new Choice();
                    c.qData = s;
                    c.isKaoHe = true;
                    c.type = FaultType.Fault;
                    SetList(type, c, i);
                    i++;
                }
                //��ѡ
                foreach (QuestionItemDatas d in dou)
                {
                    Choice c = new Choice();
                    c.qData = d;
                    c.isKaoHe = true;
                    c.type = FaultType.Fault;
                    SetList(type, c, i);
                    i++;
                }
                //�ж�
                foreach (QuestionItemDatas j in judg)
                {
                    Choice c = new Choice();
                    c.qData = j;
                    c.isKaoHe = true;
                    c.type = FaultType.Fault;
                    SetList(type, c, i);
                    i++;
                }
            }

            /// <summary>
            /// ��������ӵ��б���
            /// </summary>
            /// <param name="type"></param>
            /// <param name="c"></param>
            private void SetList(string type, Choice c, int i)
            {
                switch (type)
                {
                    case "xunshi":
                        XsChoice.Add(i, c);
                        break;
                    case "daozha":
                        DzChoice.Add(i, c);
                        break;
                }
            }

            /// <summary>
            /// ���ȡ����ť
            /// </summary>
            internal void CancelClick()
            {
                ClearObj();
                if (stop != null)
                {
                    Instance.StopCoroutine(stop);
                }
                uiobj.SetActive(false);
            }

            /// <summary>
            /// ���ȷ�ϰ�ť
            /// </summary>
            internal void ConfirmClick()
            {
                //δ��������
                if (sin < 10 || dou < 5 || judg < 5)
                {
                    tipUI.SetActive(true);
                    return;
                }
                //����������
                if(GameUnitManager.Instance._SceneType == SceneType.xunshi)
                    ClearData(XsSingleTopic, XsDoubleTopic, XsJudgeTopic);
                else if(GameUnitManager.Instance._SceneType == SceneType.daozha)
                    ClearData(DzSingleTopic, DzDoubleTopic, DzJudgeTopic);
                for (int i = 1;i <= items.Count; i++)
                {
                    bool isFix = false;   //�����Ƿ��޸Ĺ�����
                    switch (GameUnitManager.Instance._SceneType)
                    {
                        case SceneType.xunshi:
                            //�����޸�����
                            Choice tp = XsChoice[i];
                            if (items[i].isKaoHeBtn.isOn != XsChoice[i].isKaoHe)
                            {
                                tp.isKaoHe = items[i].isKaoHeBtn.isOn;
                                isFix = true;
                            }
                            else
                            {
                                tp.isKaoHe = XsChoice[i].isKaoHe;
                            }
                            if (items[i].JudgFaultChoice())
                            {
                                FaultType fault = GetFaultType(items[i].choiceBtn);
                                if (fault != XsChoice[i].type)
                                {
                                    tp.type = fault;
                                    isFix = true;
                                }
                                else
                                {
                                    tp.type = XsChoice[i].type;
                                }
                            }
                            if (isFix)
                            {
                                XsChoice.Remove(i);
                                XsChoice.Add(i, tp);
                            }
                            //���ǿ��ˣ�����Ŀ���뵽�����
                            if (XsChoice[i].isKaoHe)
                            {
                                //�����óɹ������ͣ�������showFaultΪTRUE
                                if (XsChoice[i].qData.existFault == 1 && XsChoice[i].type == FaultType.Fault)
                                {
                                    XsChoice[i].qData.showFault = true;
                                }
                                else
                                {
                                    XsChoice[i].qData.showFault = false;
                                }
                                XsChoice[i].qData.ResetAnswer();   //���ô�
                                if (XsChoice[i].qData.type == 1)
                                {
                                    if (!XsSingleTopic.Contains(XsChoice[i].qData))
                                    {
                                        XsSingleTopic.Add(XsChoice[i].qData);
                                    }
                                }
                                else if(XsChoice[i].qData.type == 2)
                                {
                                    if (!XsDoubleTopic.Contains(XsChoice[i].qData))
                                    {
                                        XsDoubleTopic.Add(XsChoice[i].qData);
                                    }
                                }
                                else if (XsChoice[i].qData.type == 3)
                                {
                                    if (!XsJudgeTopic.Contains(XsChoice[i].qData))
                                    {
                                        XsJudgeTopic.Add(XsChoice[i].qData);
                                    }
                                }
                            }
                            break;
                        case SceneType.daozha:
                            //�����޸�����
                            Choice tp1 = DzChoice[i];
                            if (items[i].isKaoHeBtn.isOn != DzChoice[i].isKaoHe)
                            {
                                tp1.isKaoHe = items[i].isKaoHeBtn.isOn;
                                isFix = true;
                            }
                            else
                            {
                                tp1.isKaoHe = DzChoice[i].isKaoHe;
                            }
                            if (items[i].JudgFaultChoice())
                            {
                                FaultType fault1 = GetFaultType(items[i].choiceBtn);
                                if (fault1 != DzChoice[i].type)
                                {
                                    tp1.type = fault1;
                                    isFix = true;
                                }
                                else
                                {
                                    tp1.type = DzChoice[i].type;
                                }
                            }
                            if (isFix)
                            {
                                DzChoice.Remove(i);
                                DzChoice.Add(i, tp1);
                            }
                            //���ǿ��ˣ�����Ŀ���뵽�����
                            if (DzChoice[i].isKaoHe)
                            {
                                //�����óɹ������ͣ�������showFaultΪTRUE
                                if (DzChoice[i].qData.existFault == 1 && DzChoice[i].type == FaultType.Fault)
                                {
                                    DzChoice[i].qData.showFault = true;
                                }
                                else
                                {
                                    DzChoice[i].qData.showFault = false;
                                }
                                if (DzChoice[i].qData.type == 1)
                                {
                                    if (!DzSingleTopic.Contains(DzChoice[i].qData))
                                    {
                                        DzSingleTopic.Add(DzChoice[i].qData);
                                    }
                                }
                                else if (DzChoice[i].qData.type == 2)
                                {
                                    if (!DzDoubleTopic.Contains(DzChoice[i].qData))
                                    {
                                        DzDoubleTopic.Add(DzChoice[i].qData);
                                    }
                                }
                                else if (DzChoice[i].qData.type == 3)
                                {
                                    if (!DzJudgeTopic.Contains(DzChoice[i].qData))
                                    {
                                        DzJudgeTopic.Add(DzChoice[i].qData);
                                    }
                                }
                            }
                            break;
                    }
                }
                Instance._ExamUI.SetQuestion();
                ClearObj();
                Instance.StopCoroutine(stop);
                uiobj.SetActive(false);
            }

            /// <summary>
            /// ��ȡ��������
            /// </summary>
            /// <param name="toggles"></param>
            /// <returns></returns>
            private FaultType GetFaultType(Toggle[] toggles)
            {
                FaultType fault = FaultType.None;
                if (toggles[0].isOn)
                {
                    fault = FaultType.Normal;
                }
                else if (toggles[1].isOn)
                {
                    fault = FaultType.Fault;
                }
                return fault;
            }

            internal void SetTopicCntText()
            {
                TopicCnt[0].text = sin.ToString();
                TopicCnt[1].text = dou.ToString();
                TopicCnt[2].text = judg.ToString();
            }
        }

        /// <summary>
        /// ����ִ����ʾ����
        /// </summary>
        [Serializable]
        public class ExAnimWin
        {
            public GameObject uiobj;
            public bool isShow = false;

            /// <summary>
            /// �����Ƿ���ʾ
            /// </summary>
            public void ShowWin()
            {
                if (isShow)
                {
                    uiobj.SetActive(true);
                }
                else
                {
                    uiobj.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 20221117����������UI
        /// </summary>
        [Serializable]
        public class FaultModUI
        {
            public GameObject uiobj;
            public Button enterScene;
            public Button returnExamUI;
            public GameObject tip;

            internal void Init()
            {
                uiobj.SetActive(false);
                enterScene.gameObject.SetActive(false);
                returnExamUI.gameObject.SetActive(false);
                tip.SetActive(false);
            }

            internal void SetScene(FaultState fs)
            {
                foreach (FaultMod fm in Manager.Instance.faultMod.Values)
                {
                    SceneType ts = (SceneType)(int.Parse(fm.SceneType));
                    if(ts == GameUnitManager.Instance._SceneType)
                    {
                        if(int.Parse(fm.QuestionID) == fs.qID)
                        {
                            uiobj.SetActive(true);
                            enterScene.gameObject.SetActive(true);
                            PlayerMove.Instance.SetFaultArgs(fs.showFault, fm.FaultModName, fm.Location);
                        }
                    }
                }
            }
        }
        #endregion

        public static UIManager Instance;

        public WarringUI _WarringUI = new WarringUI();
        public LoadingUI _LoadingUI = new LoadingUI();
        public SelectSceneUI _SelectSceneUI = new SelectSceneUI();
        public SelectModeUI _SelectModeUI = new SelectModeUI();
        public Title _Title = new Title();
        public IntroUI _IntroUI = new IntroUI();
        public ModelUI _ModelUI = new ModelUI();
        public DaoZhaUI _DaoZhaUI = new DaoZhaUI();
        public DzTipUI _DzTipUI = new DzTipUI();
        public SelectPracticeUI _SelectPracticeUI = new SelectPracticeUI();
        public CheckPracticeUI _CheckPracticeUI = new CheckPracticeUI();
        public GuideUI _GuideUI = new GuideUI();
        public GameObject _TodoUI;
        public GameObject _StartTipsUI;
        public GameObject _GuideTips;
        public ExamUI _ExamUI = new ExamUI();
        public ExAnimWin _ExAnimWin = new ExAnimWin();
        public FaultModUI _FaultModUI = new FaultModUI();

        //20230208�������洢�������Ͻǲ˵���ʾ�����ֵ�����
        public GUISkin guiSkin;

        internal List<BtnMenu> menu_1 = new List<BtnMenu>();
        internal List<BtnMenu> menu_2 = new List<BtnMenu>();
        internal List<BtnMenu> menu_3 = new List<BtnMenu>();
        internal List<BtnMenu> menu_4 = new List<BtnMenu>();
        internal List<CheckPracticeTable> checkPracticeData = new List<CheckPracticeTable>();

        //20221123�����洢��Ҫ�滻�ַ����ļ�ֵ������
        private Dictionary<string, string> StrDic = new Dictionary<string, string>();

        internal float spendTime;
        internal int hour;
        internal int minute;
        internal int second;

        private void Awake()
        {
            Instance = this;
        }
        internal void InitStart()
        {
            AnalyzeBtnMenu();
            RestAllUI();
        }

        internal void RestAllUI()
        {
            _SelectSceneUI.uiobj.SetActive(false);
            _SelectModeUI.uiobj.SetActive(false);
            _Title.uiobj.SetActive(false);
            _IntroUI.uiobj.SetActive(false);
            _ModelUI.uiobj.SetActive(false);
            _DaoZhaUI.RestUI();
            _DaoZhaUI.uiobj.SetActive(false);
            _DzTipUI.uiobj.SetActive(false);
            _SelectPracticeUI.uiobj.SetActive(false);
            //_CheckPracticeUI.uiobj.SetActive(false);
            _CheckPracticeUI.RestUI();
            _GuideUI.Promuiobj.SetActive(false);
            _GuideUI.Doneuiobj.SetActive(false);
            _GuideUI.guidanceUI.Rest();
            _ExamUI.uiobj.SetActive(false);
            _StartTipsUI.SetActive(false);
            _GuideTips.SetActive(false);
            _ExAnimWin.isShow = false;
            _ExAnimWin.ShowWin();
            _FaultModUI.Init();
        }

        #region ���ݱ��ֶν���
        internal void AnalyzeBtnMenu()
        {
            foreach (BtnMenu _d in Manager.Instance.AllBtn.Values)
            {
                switch (_d.BtnType)
                {
                    case "menu_1":
                        menu_1.Add(_d);
                        break;
                    case "menu_2":
                        menu_2.Add(_d);
                        break;
                    case "menu_3":
                        menu_3.Add(_d);
                        break;
                    case "menu_4":
                        menu_4.Add(_d);
                        break;
                }
            }
        }

        internal void AnalyzeCkPBtn(BtnMenu bm)
        {
            if (bm.BtnData.Contains(","))
            {
                string[] _s = bm.BtnData.Split(',');
                Instance.checkPracticeData.Clear();
                switch (_s[1])
                {
                    case "model1":
                        foreach (CheckPracticeTable _da in Manager.Instance.AllCkPData.Values)
                        {
                            if (_da.ModelType.Equals("1"))
                                checkPracticeData.Add(_da);
                        }
                        break;
                    case "model2":
                        foreach (CheckPracticeTable _da in Manager.Instance.AllCkPData.Values)
                        {
                            if (_da.ModelType.Equals("2"))
                                checkPracticeData.Add(_da);
                        }
                        break;
                    case "model3":
                        foreach (CheckPracticeTable _da in Manager.Instance.AllCkPData.Values)
                        {
                            if (_da.ModelType.Equals("3"))
                                checkPracticeData.Add(_da);
                        }
                        break;
                    case "model4":
                        foreach (CheckPracticeTable _da in Manager.Instance.AllCkPData.Values)
                        {
                            if (_da.ModelType.Equals("4"))
                                checkPracticeData.Add(_da);
                        }
                        break;
                    case "model5":
                        foreach (CheckPracticeTable _da in Manager.Instance.AllCkPData.Values)
                        {
                            if (_da.ModelType.Equals("5"))
                                checkPracticeData.Add(_da);
                        }
                        break;
                    case "model6":
                        foreach (CheckPracticeTable _da in Manager.Instance.AllCkPData.Values)
                        {
                            if (_da.ModelType.Equals("6"))
                                checkPracticeData.Add(_da);
                        }
                        break;
                }
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// �л�UI
        /// scene��ʾ��ʾѡ�񳡾�UI
        /// mode��ʾ��ʾģʽѡ��UI
        /// main��ʾ����������Ҫһֱ��ʾ��UI
        /// modelintro��ʾģ�ͽ��������ʾ��UI
        /// </summary>
        /// <param name="_s"></param>
        internal void SetSelectUI(string _s)
        {
            RestAllUI();
            switch (_s)
            {
                case "scene":
                    _SelectSceneUI.InitStart();
                    break;
                case "mode":
                    _SelectModeUI.InitStart();
                    break;
                case "main":
                    GameUnitManager.Instance.isGuide = true;
                    break;
                case "exam":
                    _Title.uiobj.SetActive(true);
                    //���ñ������г�������������İ�ť
                    for(int i = 0; i < _Title.toggles.Count; i++)
                    {
                        _Title.toggles[i].interactable = false;
                    }
                    _ExamUI.Init();
                    break;
            }
        }

        /// <summary>
        /// ���س�ʼ����Ĺ���ʵ��
        /// </summary>
        public void Back()
        {
            foreach(Toggle t in _Title.toggles)
            {
                t.isOn = false;
            }
            if (PlayerMove.Instance)
            {
                if (!PlayerMove.Instance.IsRotationControl)
                {
                    PlayerMove.Instance.IsRotationControl = true;
                }
            }
            GameUnitManager.Instance.isFirst = true;
            AudioPlay.Instance.StopAudio();
            _ExamUI._examChoiceUI.CancelClick();
            RestAllUI();
            Manager.Instance.SpLoadScene("Login");
        }

        /// <summary>
        /// Ѳ���ж���UI��"��һ��"��ť����
        /// ��Unity�а�
        /// </summary>
        public void NextObjBtn()
        {
            //ֹͣ��������
            AudioPlay.Instance.StopAudio();
            Instance._ModelUI.Objstep++;
            Instance._ModelUI.step++;

            if (Instance._ModelUI.hc)
            {
                Instance._ModelUI.hc.enabled = false;
                Manager.Instance.ClearData(Instance._ModelUI.hc.gameObject);
            }
            
            if (Instance._ModelUI.step > Instance._ModelUI.stepEnd)
            {
                //�����в���չʾ���ǰ����һ���ص�
                if (Instance._ModelUI.Objstep > Instance._ModelUI.models.Count)
                {
                    _ModelUI.uiobj.SetActive(false);
                    _ModelUI.ModelRawImg.SetActive(false);
                    _ModelUI.pm.IsRotationControl = true;
                    GameUnitManager.Instance._Scene1.NextStep();
                }
                else
                {
                    Instance._ModelUI.tgid++;  //ǰһ����ǩ�������ܣ�������һ����ǩ
                    Instance._ModelUI.SetToggle(Instance._ModelUI.tgid);
                }
            }
            else
            {
                Instance._ModelUI.ExDisplay();
            }
        }

        /// <summary>
        /// ȡ��δ������ʾ��Ϣ���ָ���֮ǰ��ģ�ͽ�����
        /// </summary>
        public void CancelDeveTip()
        {
            _TodoUI.SetActive(false);
            foreach (GameObject _o in _ModelUI.ActiveObj)
            {
                _o.SetActive(true);
            }
            _ModelUI.ModelRawImg.SetActive(true);
            _ModelUI.SetTogglePic(_ModelUI.tgtmp);
            _ModelUI.Objstep--;
            _ModelUI.step--;
            NextObjBtn();
        }

        /// <summary>
        /// �����н����ύ��ʾ��ȷ�ϼ�
        /// </summary>
        public void ExamConfirmSub()
        {
            _ExamUI.ConfirmSubmit();
        }

        /// <summary>
        /// �����н����ύ��ʾ��ȡ����
        /// </summary>
        public void ExamConfirmCan()
        {
            _ExamUI.CancelSubmit();
        }

        /// <summary>
        /// �����в鿴���ť
        /// </summary>
        public void ExamLookAnswer()
        {
            _ExamUI.LookAnswer();
        }

        /// <summary>
        /// �������ٴδ���
        /// </summary>
        public void ExamAgain()
        {
            _ExamUI.Again();
        }

        /// <summary>
        /// ���㿼����ʱ��Э��
        /// </summary>
        /// <returns></returns>
        public IEnumerator CountTime()
        {
            while (true)
            {
                spendTime += Time.deltaTime;  //��ʱ��ת��Ϊʱ����

                hour = (int)spendTime / 3600;
                minute = (int)(spendTime - hour * 3600) / 60;
                second = (int)(spendTime - hour * 3600 - minute * 60);
                _ExamUI.time.text = string.Format("{0:D2}��{1:D2}��", minute, second);
                yield return new WaitForEndOfFrame();
            }
        } 

        public IEnumerator CountTopic()
        {
            while (true)
            {
                int sin = 0;
                int dou = 0;
                int judg = 0;
                foreach (ChoiceUIItem it in _ExamUI._examChoiceUI.items.Values)
                {
                    if (it.isKaoHeBtn.isOn)
                    {
                        if (it.itemCnt.text.Contains("����ѡ��"))
                            sin++;
                        else if (it.itemCnt.text.Contains("����ѡ��"))
                            dou++;
                        else if (it.itemCnt.text.Contains("���жϡ�"))
                            judg++;
                    }
                }
                _ExamUI._examChoiceUI.sin = sin;
                _ExamUI._examChoiceUI.dou = dou;
                _ExamUI._examChoiceUI.judg = judg;
                _ExamUI._examChoiceUI.SetTopicCntText();
                yield return new WaitForEndOfFrame();
            }
        }

        public void UIUpdate()
        {
            //��Intro UI��ʱ����ⰴ���Ƿ������һ���һ������ڵײ�
            if (_IntroUI.uiobj.activeSelf)
            {
                GameObject _ob = _IntroUI.OverBtn;
                Scrollbar _vb = _IntroUI._ScrollRect.verticalScrollbar;
                foreach (BtnUnit btn in _IntroUI.tags)
                {
                    if(btn.clickState && (btn.ID == GameUnitManager.Instance.SubBtn.Count - 1))
                    {
                        _IntroUI.Content[2].SetActive(true);
                        if (_vb.gameObject.activeSelf)
                        {
                            if ((_vb.size == 1 && _vb.value == 1) || (_vb.size != 1 && _vb.value == 0))
                            {
                                _ob.SetActive(true);
                            }
                        }
                        else
                        {
                            _ob.SetActive(true);
                        }
                    }
                    else
                    {
                        _IntroUI.Content[2].SetActive(false);
                        _ob.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// ���ְ�ť�ĵ���¼�����
        /// </summary>
        /// <param name="_o"></param>
        public void BtnEvent(GameObject _o)
        {
            string name = _o.name;
            switch (name.ToLower())
            {
                case "tuichubtn":
                    _ExAnimWin.isShow = false;
                    _ExAnimWin.ShowWin();
                    break;
                case "enterscene":
                    _o.SetActive(false);
                    _ExamUI.uiobj.SetActive(false);
                    _FaultModUI.tip.SetActive(true);
                    _FaultModUI.returnExamUI.gameObject.SetActive(true);
                    ClickHelp.Instance.isAllow = true;
                    break;
                case "returnexam":
                    _o.SetActive(false);
                    ClickHelp.Instance.isAllow = false;
                    if (_ExAnimWin.isShow)
                    {
                        _ExAnimWin.isShow = false;
                        _ExAnimWin.ShowWin();
                    }
                    _FaultModUI.tip.SetActive(false);
                    _ExamUI.uiobj.SetActive(true);
                    _FaultModUI.enterScene.gameObject.SetActive(true);
                    break;
                case "modifyicon":
                    _ExamUI.ModifyClick();
                    break;
                case "cancelbtn":
                    _ExamUI._examChoiceUI.CancelClick();
                    break;
                case "submitbtn":
                    _ExamUI._examChoiceUI.ConfirmClick();
                    break;
            }
        }

        /// <summary>
        /// �滻�ı��е�ʱ�䴮
        /// </summary>
        public string ReplaceCnt(string cnt, int type)
        {
            string tmpStr = cnt;
            if (tmpStr.Contains("x��x��x��xʱx��"))
            {
                //���Ǽ໤��˵��ʱ����¼��ǰʱ�䣻�����˽��и��У��ظ��໤�˼�¼��ʱ��
                if(type == 1)
                {
                    StrDic.Clear();
                    string year = DateTime.Now.Year.ToString() + "��";
                    string month = DateTime.Now.Month.ToString() + "��";
                    string day = DateTime.Now.Day.ToString() + "��";
                    string hour = DateTime.Now.Hour.ToString() + "ʱ";
                    string minute = DateTime.Now.Minute.ToString() + "��";
                    StrDic.Add("x��", year);
                    StrDic.Add("x��", month);
                    StrDic.Add("x��", day);
                    StrDic.Add("xʱ", hour);
                    StrDic.Add("x��", minute);
                }
                foreach (KeyValuePair<string, string> ky in StrDic)
                {
                    if (tmpStr.Contains(ky.Key))
                    {
                        tmpStr = tmpStr.Replace(ky.Key, ky.Value);
                    }
                }
            }
            return tmpStr;
        }
        #endregion
    }
}
