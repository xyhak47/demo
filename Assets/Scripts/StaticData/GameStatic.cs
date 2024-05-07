using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uTools;
using XD.TheManager;
using XD.UI;

namespace XD.GameStatic
{
    #region ö����
    //��������
    public enum SceneType
    {
        None,
        xunshi,
        daozha,
    }

    //ģʽ����
    public enum EventSystemType
    {
        None,
        Study,
        Practice,
        Exam,
    }

    //����λ������
    public enum HighlightType
    {
        None,
        main,
        sub,
    }

    //�󶨵İ�ťʹ������
    public enum BtnType
    {
        None,
        Scene,   //����ѡ��
        Mode,    //ģʽѡ��
        Sub,     //��ǩ�б�ѡ��
        ModelIntro, //ģ�ͽ���UI�еı�ǩ
        ChkBtn,     //Ѳ����ϰUI��ѡ��ť
        Prompt,     //��բUI�е���ʾ��ť
        AnyArea,    //��բUI�еĻظ�����������
        Queren,     //��բUI�е�ȷ��UI��ť
        PopKg,      //��բUI�еĵ������ذ�ť
        PopDl,      //��բUI�еĵ�������ָʾ��ť
        TipBtn1,    //��բ���������ʾ��ť1
        TipBtn2,    //��բ���������ʾ��ť2
    }

    public enum UIType
    {
        None,
        menu,
        common,
    }

    public enum AreaType
    {
        None,
        kongzhishi,
        gongjushi,
        xianchang,
    }

    //����ģʽ�����͵ķ�ֵ
    public enum ProblemType
    {
        Single = 5,
        Multiple = 5,
        Judgement = 5,
    }

    //�����й�����������
    public enum FaultType
    {
        None,
        Normal,   //����
        Fault,    //����
    }
    #endregion

    #region �ṹ��
    //������Ŀ�Ĺ���ѡ��״̬
    public struct FaultState
    {
        public int qID;
        public bool showFault;
    }
    #endregion

    #region ������
    [Serializable]
    public class BtnMenu
    {
        public int ID { get; set; }

        public string BtnType { get; set; }

        public string BtnString { get; set; }

        public string BtnData { get; set; }

        public string BtnRes { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), BtnType, BtnString, BtnData, BtnRes };
        }
    }

    [Serializable]
    public class SubList
    {
        public int ID { get; set; }

        public string SubType { get; set; }

        public string SubString { get; set; }

        public string SubRes { get; set; }

        public string TitleRes { get; set; }

        public string ContentData { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), SubType, SubString, SubRes, TitleRes, ContentData };
        }
    }

    [Serializable]
    public class Contents
    {
        public int ID { get; set; }

        public string StateCode { get; set; }

        public string Title { get; set; }

        public string Data { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), StateCode, Title, Data };
        }
    }

    [Serializable]
    public class CheckGuideTable
    {
        public int ID { get; set; }

        public string Index { get; set; }

        public string Operate { get; set; }

        public string Position { get; set; }

        public string SoundRes { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), Index, Operate, Position, SoundRes };
        }
    }

    [Serializable]
    public class ModelTable
    {
        public int ID { get; set; }

        public string ModelType { get; set; }

        public string ModelObj { get; set; }

        public string ImgRes { get; set; }

        public string SoundRes { get; set; }

        public string BtnRes { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), ModelType, ModelObj, ImgRes, SoundRes , BtnRes};
        }
    }

    [Serializable]
    public class DaoZhaTable
    {
        public int ID { get; set; }

        public string Index { get; set; }

        public string Area { get; set; }

        public string Practice { get; set; }

        public string Operate { get; set; }

        public string TextData { get; set; }

        public string SoundRes { get; set; }

        public string ShowAnim { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), Index, Area, Practice, Operate, TextData, SoundRes, ShowAnim};
        }
    }

    [Serializable]
    public class DzStepTip
    {
        public int ID { get; set; }

        public string ImgRes { get; set; }

        public string BtnBefore { get; set; }

        public string BtnAfter { get; set; }
    }

    [Serializable]
    public class CheckPracticeTable
    {
        public int ID { get; set; }

        public string ModelType { get; set; }

        public string Operate { get; set; }

        public string Position { get; set; }

        public string Rotation { get; set; }

        public string TextRes { get; set; }

        public string SpriteRes { get; set; }

        public string SoundRes { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), ModelType, Operate, Position, Rotation, TextRes, SpriteRes, SoundRes };
        }
    }

    [Serializable]
    public class LightTable
    {
        public int ID { get; set; }

        public string LightTarget { get; set; }

        public string TextResL { get; set; }

        public string TextResR { get; set; }

        public int DownDistance { get; set; }

        public string LorRDistance { get; set; }
        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), LightTarget, TextResL, TextResR, DownDistance.ToString(), LorRDistance };
        }
    }

    [Serializable]

    public class TaskList
    {
        public int ID { get; set; }

        public string TaskName { get; set; }

        public string TaskSprite { get; set; }

        public string Scene { get; set; }

        public string StudyStep { get; set; }

        public string PracticeStep { get; set; }

        public string[] GetAllData()
        {
            return new string[] { ID.ToString(), TaskName, TaskSprite, Scene, StudyStep, PracticeStep };
        }
    }

    public class XunShiExam
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Fault { get; set; }
        public string Problem { get; set; }
        public string Item { get; set; }
        public string Answer { get; set; }
    }

    public class DaoZhaExam
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Fault { get; set; }
        public string Problem { get; set; }
        public string Item { get; set; }
        public string Answer { get; set; }
    }

    public class FaultMod
    {
        public int ID { get; set; }
        public string SceneType { get; set; }
        public string QuestionID { get; set; }
        public string FaultModName { get; set; }
        public string Location { get; set; }
    }

    public class LookAtClick
    {
        public int ID { get; set; }
        public string ModelName { get; set; }
        public string BeforePos { get; set; }
        public string BeforeRot { get; set; }
        public string AfterPos { get; set; }
        public string AfterRot { get; set; }

        public Vector3[] GetLoction()
        {
            return new Vector3[] {Manager.Instance.Vector3Parse(BeforePos), Manager.Instance.Vector3Parse(BeforeRot) ,
                Manager.Instance.Vector3Parse(AfterPos) , Manager.Instance.Vector3Parse(AfterRot) };
        }
    }
    #endregion

    #region UI�Ӷ�����
    /// <summary>
    /// ��բUI�еĵ绰UI
    /// </summary>
    [Serializable]
    public class DianHuaUI
    {
        public GameObject master;
        public Image icon;
        public GameObject nameType; //��ʾ�໤�˵��ı�
        public GameObject info;   //������ʾ������
        public GameObject bg;
        public Text content;
        public GameObject anyKey;

        /// <summary>
        /// �ָ�UI
        /// </summary>
        internal void Rest()
        {
            icon.gameObject.SetActive(true);
            info.SetActive(false);
            anyKey.SetActive(false);
            master.SetActive(false);
        }

        /// <summary>
        /// ����UI
        /// </summary>
        /// <param name="cnt">��������</param>
        internal void SetUI(string cnt)
        {
            if (!master.activeSelf)
                master.SetActive(true);
            info.SetActive(true);
            string text = UIManager.Instance.ReplaceCnt(cnt, 1);
            content.text = text;
            uTweenAlpha.Begin(bg, 1f, 0f, 0.5f, 1f);
            uTweenAlpha.Begin(content.gameObject, 1f, 0f, 0.5f, 1f);
        }

        /// <summary>
        /// ����ͷ��ͼƬ
        /// </summary>
        /// <param name="pic">ͼƬ·��</param>
        internal void SetIcon(string pic)
        {
            icon.sprite = Resources.Load<Sprite>(pic);
            if (pic.Contains("jhr"))
            {
                nameType.SetActive(true);
            }
            else
            {
                nameType.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��բUI�еĻظ���ϢUI
    /// </summary>
    [Serializable]
    public class HuiFuUI
    {
        public GameObject master;  //������ʾ������
        public GameObject bg;
        public Text content;
        public GameObject anykey;

        internal void SetUI(string pic, string cnt)
        {
            bg.GetComponent<Image>().sprite = Resources.Load<Sprite>(pic);
            anykey.SetActive(true);
            anykey.GetComponent<ControlBtn>().btnType = BtnType.AnyArea;
            anykey.GetComponent<ControlBtn>().enabled = false;
            string text = UIManager.Instance.ReplaceCnt(cnt, 2);
            content.text = text;
            master.SetActive(true);
            uTweenAlpha.Begin(bg, 1f, 0f, 0.5f, 1f);
            uTweenAlpha.Begin(content.gameObject, 1f, 0f, 0.5f, 1f);
            //������Ч�������������
            anykey.GetComponent<ControlBtn>().enabled = true;
        }
    }

    /// <summary>
    /// ��բUI�е�ȷ����ϢUI
    /// </summary>
    [Serializable]
    public class QueRenUI
    {
        public GameObject master;
        public Image info;
        public Image btn;

        internal void Init(string[] pic)
        {
            //����ͼƬ
            info.sprite = Resources.Load<Sprite>(pic[0]);
            btn.sprite = Resources.Load<Sprite>(pic[1]);
            //ͼƬ����Ӧ
            info.SetNativeSize();
            btn.SetNativeSize();
            //�󶨰�ť�����¼�
            ControlBtn cb =  btn.gameObject.GetComponent<ControlBtn>();
            cb.btnType = BtnType.Queren;
            cb.Init(pic[1], pic[2]);

            master.SetActive(true);
        }

    }

    /// <summary>
    /// ��բUI�еĵ���UI
    /// </summary>
    [Serializable]
    public class PopUpUI
    {
        public GameObject master;
        public List<GameObject> popup = new List<GameObject>(); //Inspector�е������е���
        public List<GameObject> btn = new List<GameObject>(); //Inspector�е������а�ť
        internal string _pic1, _pic2;
        private GameObject _b;

        /// <summary>
        /// ���ص���UI
        /// </summary>
        internal void Rest()
        {
            master.SetActive(false);
            foreach(GameObject _o in popup)
            {
                _o.SetActive(false);
            }
            RestBtn();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pic1"></param>
        /// <param name="pic2"></param>
        internal void Init(string name, string pic1, string pic2)
        {
            GameObject _o = ActivePop(name);
            if (!pic1.Equals("null"))
            {
                _o.GetComponent<Image>().sprite = Resources.Load<Sprite>(pic1);
                _pic1 = pic1;
                _pic2 = pic2;
            }
        }

        /// <summary>
        /// ���õ�����ť
        /// </summary>
        internal void SetBtn()
        {
            ControlBtn cb = _b.GetComponentInChildren<ControlBtn>();
            if (_b.name.Equals("kg"))
            {
                cb.btnType = BtnType.PopKg;
                cb.Init(_pic1, _pic2);
            }
            else if (_b.name.Equals("dl"))
            {
                cb.btnType = BtnType.PopDl;
            }
        }

        /// <summary>
        /// ���ݵ������ƻ�ȡҪ����ĵ���UI����
        /// </summary>
        /// <param name="uiname">��������</param>
        /// <returns></returns>
        internal GameObject ActivePop(string uiname)
        {
            foreach(GameObject _o in popup)
            {
                if (_o.name.Equals(uiname))
                {
                    _o.SetActive(true);
                    return _o;
                }
            }
            return null;
        }

        /// <summary>
        /// ���ݰ�ť���Ƽ������ť
        /// </summary>
        /// <param name="btnname">��ť����</param>
        internal void ActiveBtn(string btnname)
        {
            RestBtn();
            foreach (GameObject _o in btn)
            {
                if (_o.name.Equals(btnname))
                {
                    _o.SetActive(true);
                    _b = _o;
                }
            }
        }

        /// <summary>
        /// ���ݵ������ƹرյ���
        /// </summary>
        /// <param name="uiname">��������</param>
        internal void ClosePop(string uiname)
        {
            foreach (GameObject _o in popup)
            {
                if (_o.name.Equals(uiname))
                    _o.SetActive(false);
            }
        }

        /// <summary>
        /// �������е�����ť
        /// </summary>
        internal void RestBtn()
        {
            foreach(GameObject _o in btn)
            {
                _o.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��բUI�еĹ��߽���UI
    /// </summary>
    [Serializable]
    public class ToolIntro
    {
        public GameObject master;
        public Image info;
        public Button tuichu;
        public Button next;

        /// <summary>
        /// ��ʼ�����߽���UI��Ϣ
        /// </summary>
        /// <param name="_s"></param>
        internal void Init(string _s)
        {
            master.SetActive(true);
            info.sprite = Resources.Load<Sprite>(_s);
            tuichu.onClick.RemoveAllListeners();
            next.onClick.RemoveAllListeners();
            tuichu.onClick.AddListener(Tuichu);
            next.onClick.AddListener(Tuichu);
        }

        /// <summary>
        /// �˳����߽��ܣ�������һ��
        /// </summary>
        private void Tuichu()
        {
            master.SetActive(false);
            GameUnitManager.Instance.Next();
        }
    }

    /// <summary>
    /// Ѳ����ϰÿһ�׶���ɺ������һ�׶ε�ui��ʾ
    /// </summary>
    [Serializable]
    public class NextUI
    {
        public GameObject uiobj;
        public GameObject Bg;
        public Button OwnWayBtn;    //����̽����ť
        public Button GoOnBtn;      //����Ѳ�Ӱ�ť

        internal void Rest()
        {
            uiobj.SetActive(false);
        }

        internal void Init()
        {
            uiobj.SetActive(true);
            GameUnitManager.Instance._ScenePractice.AnalyzeNextPracModel();
            GoOnBtn.onClick.AddListener(OnGoOn);
            OwnWayBtn.onClick.AddListener(OnOwnWay);
        }


        internal void OnOwnWay()
        {
            uiobj.SetActive(false);

        }

        internal void OnGoOn()
        {
            uiobj.SetActive(false);
            UIManager.Instance._SelectPracticeUI.uiobj.SetActive(true);
        }

    }

    /// <summary>
    /// ����Ѳ�ӵ�ui,Ŀǰ��δ����
    /// </summary>
    [Serializable]
    public class JumpUI
    {
        public GameObject uiobj;
        public Button FreeJumpBtn;
        public Sprite Jump0;
        public Sprite Jump1;

        internal void Rest()
        {
            uiobj.SetActive(false);
        }
        internal void Init()
        {
            uiobj.SetActive(true);
            FreeJumpBtn.gameObject.GetComponent<Image>().sprite = Jump0;
        }
    }
    /// <summary>
    /// Ѳ����ϰ��������ʾ��ϰui
    /// </summary>
    [Serializable]
    public class HintUI
    {
        public GameObject uiobj;
        public Text content;

        internal void Rest()
        {
            uiobj.SetActive(false);
        }

        //�����ʾ��Ϣ�ı�
        internal void SetUI(string ss)
        {
            content.text = ss;
            uiobj.SetActive(true);
        }

    }

    /// <summary>
    /// ��������ui
    /// </summary>
    [Serializable]
    public class GuidanceUI
    {
        public GameObject uiobj;
        public GameObject TaskList;
        public GuideController guideController;
        public Button skip; //�����̳̰�ť
        internal RectTransform target;
        public bool isSoft;
        public float scale;
        public float time;

        internal void Rest()
        {
            uiobj.SetActive(false);
        }

        internal void Init()
        {
            uiobj.SetActive(true);
            skip.onClick.AddListener(OnSkip);
        }

        internal void OnSkip()
        {
            Rest();
            GameUnitManager.Instance.isGuide = false;
            UIManager.Instance._GuideUI.GoOnTheStep();
        }


    }

    #region ����ģʽUI
    [Serializable]
    public class Paper
    {
        public GameObject master;
        public Image bg;
        public Text problemText;
        public Transform answer;
    }

    [Serializable]
    public class AnswerCard
    {
        public GameObject master;
        public Transform sinTagContent;
        public Transform mulTagContent;
        public Transform judgeTagContent;
        public Text rightText;
        public Text yourText;
        internal List<AnswerNum> _answer = new List<AnswerNum>();
        private bool isSelect;
        private int wrongNum;

        internal void Init(List<QuestionNum> question)
        {
            int i = 0;
            wrongNum = 0;
            foreach (QuestionNum qn in question)
            {
                //���ô�������ť
                TagBtnItem tag = new TagBtnItem();
                switch (qn.qitem.qItemData.type)
                {
                    case 1:
                        tag = Manager.Instance.InitResObj(sinTagContent, "UI/Prefabs/Tag").GetComponent<TagBtnItem>();
                        break;
                    case 2:
                        tag = Manager.Instance.InitResObj(mulTagContent, "UI/Prefabs/Tag").GetComponent<TagBtnItem>();
                        break;
                    case 3:
                        tag = Manager.Instance.InitResObj(judgeTagContent, "UI/Prefabs/Tag").GetComponent<TagBtnItem>();
                        break;
                }
                tag.ID = i;
                //��������ťͼ��
                isSelect = qn.bitem.isSelectAnswer;
                if (!isSelect)
                {
                    tag.img.sprite = Resources.Load<Sprite>("UI/Pics/xd-kh-dtk-wd");
                    wrongNum++;
                }
                else
                {
                    if (qn.answerResult)
                    {
                        tag.img.sprite = Resources.Load<Sprite>("UI/Pics/xd-kh-dtk-yd");
                    }
                    else
                    {
                        tag.img.sprite = Resources.Load<Sprite>("UI/Pics/xd-kh-dtk-ct");
                        wrongNum++;
                    }
                }
                //��Ŀ���
                tag.num.text = qn.num.ToString();
                AnswerItem ai = new AnswerItem(qn);
                AnswerNum an = new AnswerNum(tag, ai, isSelect);
                _answer.Add(an);
                i++;
            }
            UIManager.Instance._ExamUI.record.text = wrongNum.ToString() + "/20";
        }

        internal void SetPaper(Paper _p, int index)
        {
            UIManager.Instance._FaultModUI.Init();
            _answer[index].SetVisible(index, _p);
            rightText.text = "";
            yourText.text = "";
            foreach (string answer in _answer[index].anItem.rightAnswer)
            {
                rightText.text += Manager.Instance.Option(answer);
            }
            if (_answer[index].isSelect)
            {
                foreach (string answer in _answer[index].anItem.yourAnswer)
                {
                    yourText.text += Manager.Instance.Option(answer);
                }
                if (_answer[index].anItem.result)
                {
                    yourText.color = rightText.color;
                }
                else
                {
                    //yourText.color = new Color(255,86,86); //ǳ��ɫ
                    yourText.color = Color.red;
                }
            }
            else
            {
                yourText.text = "";
            }
        }
    }
    #endregion
    #endregion

    /// <summary>
    /// �����ؿ�����ʱ����[�ؿ�����,������ID,������ID,�Ƿ��Զ�ģʽ]
    /// </summary>
    [Serializable]
    public class TmpSceneData
    {
        public EventSystemType _StateType;
        public string SceneDataType;
        public int MainSceneDataID;
        public bool AutoNext;

        public TmpSceneData(EventSystemType _t, string _name, int mainid, bool _auto = false)
        {
            _StateType = _t;
            SceneDataType = _name; MainSceneDataID = mainid; AutoNext = _auto;
        }
    }

    [Serializable]
    public class TmpAnimObj
    {
        public GameObject obj;
        public Vector3 pos;
        public Quaternion rot;
        public Quaternion[] drot;

        /// <summary>
        /// ��ʱ�洢ִ�ж���ǰģ�͵�λ�ü���ת�Ƕ�
        /// </summary>
        /// <param name="_o"></param>
        internal void SetValue(GameObject _o)
        {
            obj = _o;
            pos = _o.transform.position;
            rot = _o.transform.rotation;
            //��ģ���ǵ����ʱ����Ҫ��ȡ�������������ת�Ƕ�
            if (_o.name.Contains("daodianbi"))
            {
                Transform[] daodianbi = _o.GetComponentsInChildren<Transform>();
                drot = new Quaternion[daodianbi.Length];
                for(int i = 0; i < daodianbi.Length; i++)
                {
                    drot[i] = daodianbi[i].rotation;
                }
            }
        }

        /// <summary>
        /// �ָ�ģ����ִ�ж���ǰ��λ�ü���ת�Ƕ�
        /// </summary>
        internal void Rest()
        {
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            if (obj.name.Contains("daodianbi"))
            {
                Transform[] daodianbi = obj.GetComponentsInChildren<Transform>();
                for (int i = 0; i < daodianbi.Length; i++)
                {
                    daodianbi[i].rotation = drot[i];
                }
            }
        }
    }

    [Serializable]
    public class QuestionItemDatas
    {
        //20221129�������������ԣ���Ŀ����Լ��Ƿ���Ҫ���ù���
        public int questionID;
        public int existFault;
        public bool showFault = true;

        public int type;
        public ProblemType proScore; //��ֵ
        public string topic;  //��Ŀ����
        public List<string> item = new List<string>();  //��ѡ������
        public List<string> answer = new List<string>();   //��

        internal string normalAnswer = "0";   //�޹���ʱ�Ĵ�
        internal string faultAnswer = "";     //��¼�й���ʱ�Ĵ�

        public QuestionItemDatas(XunShiExam xs)
        {
            questionID = xs.ID;
            existFault = int.Parse(xs.Fault);
            switch (xs.Type)
            {
                //��ѡ��
                case "1":
                    type = 1;
                    proScore = ProblemType.Single;
                    topic = xs.Problem;
                    AddItem(xs.Item);
                    answer.Add(xs.Answer);
                    //20221130���������ڹ���ʱ����¼��Ŀѡ����Ϻ�Ĵ�
                    if(existFault == 1)
                    {
                        faultAnswer = xs.Answer;
                    }
                    break;
                //��ѡ��
                case "2":
                    type = 2;
                    proScore = ProblemType.Multiple;
                    topic = xs.Problem;
                    AddItem(xs.Item);
                    if (xs.Answer.Contains(","))
                    {
                        string[] _s = xs.Answer.Split(',');
                        foreach (string _ss in _s)
                            answer.Add(_ss);
                    }
                    break;
                //�ж���
                case "3":
                    type = 3;
                    proScore = ProblemType.Judgement;
                    topic = xs.Problem;
                    AddItem(xs.Item);
                    answer.Add(xs.Answer);
                    break;
            }
        }

        public QuestionItemDatas(DaoZhaExam ds)
        {
            questionID = ds.ID;
            existFault = int.Parse(ds.Fault);
            switch (ds.Type)
            {
                //��ѡ��
                case "1":
                    type = 1;
                    proScore = ProblemType.Single;
                    topic = ds.Problem;
                    AddItem(ds.Item);
                    answer.Add(ds.Answer);
                    break;
                //��ѡ��
                case "2":
                    type = 2;
                    proScore = ProblemType.Multiple;
                    topic = ds.Problem;
                    AddItem(ds.Item);
                    if (ds.Answer.Contains(","))
                    {
                        string[] _s = ds.Answer.Split(',');
                        foreach (string _ss in _s)
                            answer.Add(_ss);
                    }
                    break;
                //�ж���
                case "3":
                    type = 3;
                    proScore = ProblemType.Judgement;
                    topic = ds.Problem;
                    AddItem(ds.Item);
                    answer.Add(ds.Answer);
                    break;
            }
        }

        /// <summary>
        /// Ϊ��Ŀ��Ӷ�Ӧ�Ĵ���ѡ��
        /// </summary>
        /// <param name="it"></param>
        private void AddItem(string it)
        {
            if (it.Contains("|"))
            {
                string[] _s = it.Split('|');
                foreach (string _ss in _s)
                {
                    item.Add(_ss);
                }
            }
        }

        internal void ResetAnswer()
        {
            if(existFault == 1)
            {
                if (showFault)
                {
                    answer[0] = faultAnswer;
                }
                else
                {
                    answer[0] = normalAnswer;
                }
            }
        }
    }

    #region ��������
    /// <summary>
    /// ��Ŀ������Ϣ
    /// </summary>
    public class QuestionItem
    {
        public string bg;
        public QuestionItemDatas qItemData;
    }

    /// <summary>
    /// ���������ť����Ŀ����
    /// </summary>
    public class QuestionNum
    {
        public int num;    //��Ŀ���
        public QuestionBtnItem bitem; //��ť
        public QuestionItem qitem;    //��Ŀ����
        private ToggleGroup group;
        internal List<string> selected = new List<string>();
        public bool answerResult = false;
        internal List<ToggleItem> toggle = new List<ToggleItem>();

        public QuestionNum(int _n, QuestionBtnItem _b, QuestionItem _q)
        {
            num = _n;
            bitem = _b;
            qitem = _q;
        }

        /// <summary>
        /// ��ʾ��ť��Ӧ��Ŀ����
        /// </summary>
        /// <param name="_p"></param>
        public void SetVisible(int num, Paper _p)
        {
            _p.bg.sprite = Resources.Load<Sprite>(qitem.bg);
            //��Ŀ�ı�
            _p.problemText.text = (num + 1).ToString() + "." + qitem.qItemData.topic;
            //���������Ѿ�����Ԥ���壬��ȫ������
            Toggle[] temp = _p.answer.gameObject.GetComponentsInChildren<Toggle>();
            if (temp.Length != 0)
            {
                foreach (Toggle _t in temp)
                {
                    GameObject.Destroy(_t.gameObject);
                }
            }
            //���������ж��Ƿ���Ҫ����Ԥ�������ToggleGroup
            if (qitem.qItemData.type == 1 || qitem.qItemData.type == 3)
            {
                //Ϊ�˱�֤��Ĭ����ѡ����Inspector�Ϲ�ѡToggleGroup��Allow Switch Off
                group = _p.answer.GetComponent<ToggleGroup>();
            }
            else
            {
                group = null;
            }
            //��ѡ��toggle��Ŵ�0��ʼ
            int id = 0;
            toggle.Clear();
            //������Ŀ��Ӧ�Ĵ�ѡ����Ϣ
            foreach (string _s in qitem.qItemData.item)
            {
                //���ش�ѡ��Ԥ����
                ToggleItem ti = Manager.Instance.InitResObj(_p.answer, "UI/Prefabs/Toggle").GetComponent<ToggleItem>();
                //��ʼ����ѡ������
                ti.Init(id, qitem.qItemData, _s, bitem);
                ti.thisTog.group = group;
                //�жϵ�ǰ��Ŀ�Ƿ��Ѿ���ѡ����ˣ������жϸ�ѡ���Ƿ��Ǳ�ѡ�е�
                if (bitem.isSelectAnswer && selected.Contains(id.ToString()))
                {
                    ti.thisTog.isOn = true;
                }
                toggle.Add(ti);
                id++;
            }
        }

        /// <summary>
        /// ����ѡ�ѡ�У����������Ϣ
        /// </summary>
        internal void SelectToggle()
        {
            //�������д�ѡ��
            foreach(ToggleItem _t in toggle)
            {
                //����ѡ�ѡ����ѡ���б����޸ô�ѡ����
                if (_t.thisTog.isOn && !selected.Contains(_t.ID.ToString()))
                {
                    selected.Add(_t.ID.ToString()); //��ѡ����ӵ�ѡ���б���
                }
                else if (!_t.thisTog.isOn && selected.Contains(_t.ID.ToString()))
                {
                    //����ѡ��δ��ѡ�е���ѡ���б��У���ѡ���Ƴ�ѡ���б�
                    selected.Remove(_t.ID.ToString());
                }
            }
            //�ж�ѡ�еĴ�ѡ���Ƿ�����ȷ��
            if(selected.Count == qitem.qItemData.answer.Count)
            {
                foreach(string sl in selected)
                {
                    if (!qitem.qItemData.answer.Contains(sl))
                    {
                        answerResult = false;
                        return;
                    }
                }
                answerResult = true;
            }
            else
            {
                answerResult = false;
            }
        }
    }

    /// <summary>
    /// ����Ŀ������Ϣ
    /// </summary>
    public class AnswerItem
    {
        public string bg;
        public string topic;  //��Ŀ
        public List<string> item = new List<string>(); //ѡ��
        public List<string> yourAnswer;
        public List<string> rightAnswer;
        public bool result;
        public int score;

        /// <summary>
        /// ���ù��캯��������������
        /// </summary>
        /// <param name="qn"></param>
        public AnswerItem(QuestionNum qn)
        {
            bg = qn.qitem.bg;
            topic = qn.qitem.qItemData.topic;
            item = qn.qitem.qItemData.item;
            yourAnswer = qn.selected;
            rightAnswer = qn.qitem.qItemData.answer;
            result = qn.answerResult;
            if (result)
            {
                score = (int)qn.qitem.qItemData.proScore;
            }
            else
            {
                score = 0;
            }
        }
    }

    /// <summary>
    /// �󶨴�����ť����Ŀ����
    /// </summary>
    public class AnswerNum
    {
        public TagBtnItem tag;
        public AnswerItem anItem;
        internal bool isSelect;

        public AnswerNum(TagBtnItem _t, AnswerItem _aitem, bool _select)
        {
            tag = _t;
            anItem = _aitem;
            isSelect = _select;
        }

        /// <summary>
        /// ���õ�ǰ�ɼ���Ŀ��UI��Ϣ
        /// </summary>
        /// <param name="num"></param>
        /// <param name="_p"></param>
        internal void SetVisible(int num, Paper _p)
        {
            _p.bg.sprite = Resources.Load<Sprite>(anItem.bg);
            //������Ŀ
            _p.problemText.text = (num + 1).ToString() + "." + anItem.topic;
            //���ô�ѡ��
            Toggle[] temp = _p.answer.gameObject.GetComponentsInChildren<Toggle>();
            if (temp.Length != 0)
            {
                foreach (Toggle _t in temp)
                {
                    GameObject.Destroy(_t.gameObject);
                }
            }
            int id = 0;
            foreach (string _s in anItem.item)
            {
                ToggleItem ti = Manager.Instance.InitResObj(_p.answer, "UI/Prefabs/Toggle").GetComponent<ToggleItem>();
                ti.gameObject.GetComponent<Toggle>().interactable = false;
                ti.optionText.text = _s;
                if (isSelect)
                {
                    if (anItem.yourAnswer.Contains(id.ToString()))
                    {
                        ti.gameObject.GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        ti.gameObject.GetComponent<Toggle>().isOn = false;
                    }
                }
                id++;
            }
        }
    }
    #endregion
}
