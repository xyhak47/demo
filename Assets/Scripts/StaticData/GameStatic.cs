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
    #region 枚举类
    //场景类型
    public enum SceneType
    {
        None,
        xunshi,
        daozha,
    }

    //模式类型
    public enum EventSystemType
    {
        None,
        Study,
        Practice,
        Exam,
    }

    //高亮位置类型
    public enum HighlightType
    {
        None,
        main,
        sub,
    }

    //绑定的按钮使用类型
    public enum BtnType
    {
        None,
        Scene,   //场景选择
        Mode,    //模式选择
        Sub,     //标签列表选择
        ModelIntro, //模型介绍UI中的标签
        ChkBtn,     //巡检练习UI的选择按钮
        Prompt,     //倒闸UI中的提示按钮
        AnyArea,    //倒闸UI中的回复区域任意点击
        Queren,     //倒闸UI中的确认UI按钮
        PopKg,      //倒闸UI中的弹窗开关按钮
        PopDl,      //倒闸UI中的弹窗电流指示按钮
        TipBtn1,    //倒闸步骤结束提示按钮1
        TipBtn2,    //倒闸步骤结束提示按钮2
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

    //考核模式各题型的分值
    public enum ProblemType
    {
        Single = 5,
        Multiple = 5,
        Judgement = 5,
    }

    //考核中故障设置类型
    public enum FaultType
    {
        None,
        Normal,   //正常
        Fault,    //故障
    }
    #endregion

    #region 结构体
    //考核题目的故障选择状态
    public struct FaultState
    {
        public int qID;
        public bool showFault;
    }
    #endregion

    #region 对象类
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

    #region UI子对象类
    /// <summary>
    /// 倒闸UI中的电话UI
    /// </summary>
    [Serializable]
    public class DianHuaUI
    {
        public GameObject master;
        public Image icon;
        public GameObject nameType; //提示监护人的文本
        public GameObject info;   //文字显示父对象
        public GameObject bg;
        public Text content;
        public GameObject anyKey;

        /// <summary>
        /// 恢复UI
        /// </summary>
        internal void Rest()
        {
            icon.gameObject.SetActive(true);
            info.SetActive(false);
            anyKey.SetActive(false);
            master.SetActive(false);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="cnt">文字内容</param>
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
        /// 设置头像图片
        /// </summary>
        /// <param name="pic">图片路径</param>
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
    /// 倒闸UI中的回复信息UI
    /// </summary>
    [Serializable]
    public class HuiFuUI
    {
        public GameObject master;  //文字显示父对象
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
            //文字特效播放完后，允许点击
            anykey.GetComponent<ControlBtn>().enabled = true;
        }
    }

    /// <summary>
    /// 倒闸UI中的确认信息UI
    /// </summary>
    [Serializable]
    public class QueRenUI
    {
        public GameObject master;
        public Image info;
        public Image btn;

        internal void Init(string[] pic)
        {
            //加载图片
            info.sprite = Resources.Load<Sprite>(pic[0]);
            btn.sprite = Resources.Load<Sprite>(pic[1]);
            //图片自适应
            info.SetNativeSize();
            btn.SetNativeSize();
            //绑定按钮监听事件
            ControlBtn cb =  btn.gameObject.GetComponent<ControlBtn>();
            cb.btnType = BtnType.Queren;
            cb.Init(pic[1], pic[2]);

            master.SetActive(true);
        }

    }

    /// <summary>
    /// 倒闸UI中的弹窗UI
    /// </summary>
    [Serializable]
    public class PopUpUI
    {
        public GameObject master;
        public List<GameObject> popup = new List<GameObject>(); //Inspector中导入所有弹窗
        public List<GameObject> btn = new List<GameObject>(); //Inspector中导入所有按钮
        internal string _pic1, _pic2;
        private GameObject _b;

        /// <summary>
        /// 隐藏弹窗UI
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
        /// 初始化弹窗
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
        /// 设置弹窗按钮
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
        /// 根据弹窗名称获取要激活的弹窗UI主体
        /// </summary>
        /// <param name="uiname">弹窗名称</param>
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
        /// 根据按钮名称激活弹窗按钮
        /// </summary>
        /// <param name="btnname">按钮名称</param>
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
        /// 根据弹窗名称关闭弹窗
        /// </summary>
        /// <param name="uiname">弹窗名称</param>
        internal void ClosePop(string uiname)
        {
            foreach (GameObject _o in popup)
            {
                if (_o.name.Equals(uiname))
                    _o.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏所有弹窗按钮
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
    /// 倒闸UI中的工具介绍UI
    /// </summary>
    [Serializable]
    public class ToolIntro
    {
        public GameObject master;
        public Image info;
        public Button tuichu;
        public Button next;

        /// <summary>
        /// 初始化工具介绍UI信息
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
        /// 退出工具介绍，进入下一步
        /// </summary>
        private void Tuichu()
        {
            master.SetActive(false);
            GameUnitManager.Instance.Next();
        }
    }

    /// <summary>
    /// 巡视练习每一阶段完成后进入下一阶段的ui提示
    /// </summary>
    [Serializable]
    public class NextUI
    {
        public GameObject uiobj;
        public GameObject Bg;
        public Button OwnWayBtn;    //自主探索按钮
        public Button GoOnBtn;      //继续巡视按钮

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
    /// 自由巡视的ui,目前尚未部署
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
    /// 巡视练习中左侧的提示练习ui
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

        //填充提示信息文本
        internal void SetUI(string ss)
        {
            content.text = ss;
            uiobj.SetActive(true);
        }

    }

    /// <summary>
    /// 引导界面ui
    /// </summary>
    [Serializable]
    public class GuidanceUI
    {
        public GameObject uiobj;
        public GameObject TaskList;
        public GuideController guideController;
        public Button skip; //跳过教程按钮
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

    #region 考核模式UI
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
                //设置答题栏按钮
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
                //答题栏按钮图标
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
                //题目编号
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
                    //yourText.color = new Color(255,86,86); //浅红色
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
    /// 需带入关卡的临时数据[关卡类型,主序列ID,子序列ID,是否自动模式]
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
        /// 临时存储执行动画前模型的位置及旋转角度
        /// </summary>
        /// <param name="_o"></param>
        internal void SetValue(GameObject _o)
        {
            obj = _o;
            pos = _o.transform.position;
            rot = _o.transform.rotation;
            //当模型是导电臂时，需要获取其下子物体的旋转角度
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
        /// 恢复模型在执行动画前的位置及旋转角度
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
        //20221129新增：两个属性，题目编号以及是否需要设置故障
        public int questionID;
        public int existFault;
        public bool showFault = true;

        public int type;
        public ProblemType proScore; //分值
        public string topic;  //题目内容
        public List<string> item = new List<string>();  //答案选项内容
        public List<string> answer = new List<string>();   //答案

        internal string normalAnswer = "0";   //无故障时的答案
        internal string faultAnswer = "";     //记录有故障时的答案

        public QuestionItemDatas(XunShiExam xs)
        {
            questionID = xs.ID;
            existFault = int.Parse(xs.Fault);
            switch (xs.Type)
            {
                //单选题
                case "1":
                    type = 1;
                    proScore = ProblemType.Single;
                    topic = xs.Problem;
                    AddItem(xs.Item);
                    answer.Add(xs.Answer);
                    //20221130新增：存在故障时，记录题目选择故障后的答案
                    if(existFault == 1)
                    {
                        faultAnswer = xs.Answer;
                    }
                    break;
                //多选题
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
                //判断题
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
                //单选题
                case "1":
                    type = 1;
                    proScore = ProblemType.Single;
                    topic = ds.Problem;
                    AddItem(ds.Item);
                    answer.Add(ds.Answer);
                    break;
                //多选题
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
                //判断题
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
        /// 为题目添加对应的答题选项
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

    #region 考核所需
    /// <summary>
    /// 题目内容信息
    /// </summary>
    public class QuestionItem
    {
        public string bg;
        public QuestionItemDatas qItemData;
    }

    /// <summary>
    /// 绑定左侧栏按钮和题目内容
    /// </summary>
    public class QuestionNum
    {
        public int num;    //题目编号
        public QuestionBtnItem bitem; //按钮
        public QuestionItem qitem;    //题目内容
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
        /// 显示按钮对应题目内容
        /// </summary>
        /// <param name="_p"></param>
        public void SetVisible(int num, Paper _p)
        {
            _p.bg.sprite = Resources.Load<Sprite>(qitem.bg);
            //题目文本
            _p.problemText.text = (num + 1).ToString() + "." + qitem.qItemData.topic;
            //当答案区域已经存在预制体，先全部销毁
            Toggle[] temp = _p.answer.gameObject.GetComponentsInChildren<Toggle>();
            if (temp.Length != 0)
            {
                foreach (Toggle _t in temp)
                {
                    GameObject.Destroy(_t.gameObject);
                }
            }
            //根据题型判断是否需要给答案预制体添加ToggleGroup
            if (qitem.qItemData.type == 1 || qitem.qItemData.type == 3)
            {
                //为了保证答案默认无选择，在Inspector上勾选ToggleGroup的Allow Switch Off
                group = _p.answer.GetComponent<ToggleGroup>();
            }
            else
            {
                group = null;
            }
            //答案选项toggle编号从0开始
            int id = 0;
            toggle.Clear();
            //遍历题目对应的答案选项信息
            foreach (string _s in qitem.qItemData.item)
            {
                //加载答案选项预制体
                ToggleItem ti = Manager.Instance.InitResObj(_p.answer, "UI/Prefabs/Toggle").GetComponent<ToggleItem>();
                //初始化答案选项内容
                ti.Init(id, qitem.qItemData, _s, bitem);
                ti.thisTog.group = group;
                //判断当前题目是否已经被选择过了，若是判断该选项是否是被选中的
                if (bitem.isSelectAnswer && selected.Contains(id.ToString()))
                {
                    ti.thisTog.isOn = true;
                }
                toggle.Add(ti);
                id++;
            }
        }

        /// <summary>
        /// 当答案选项被选中，设置相关信息
        /// </summary>
        internal void SelectToggle()
        {
            //遍历所有答案选项
            foreach(ToggleItem _t in toggle)
            {
                //若该选项被选上且选中列表中无该答案选项编号
                if (_t.thisTog.isOn && !selected.Contains(_t.ID.ToString()))
                {
                    selected.Add(_t.ID.ToString()); //将选项添加到选中列表中
                }
                else if (!_t.thisTog.isOn && selected.Contains(_t.ID.ToString()))
                {
                    //若该选项未被选中但在选项列表中，将选项移除选中列表
                    selected.Remove(_t.ID.ToString());
                }
            }
            //判断选中的答案选项是否是正确答案
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
    /// 答案题目内容信息
    /// </summary>
    public class AnswerItem
    {
        public string bg;
        public string topic;  //题目
        public List<string> item = new List<string>(); //选项
        public List<string> yourAnswer;
        public List<string> rightAnswer;
        public bool result;
        public int score;

        /// <summary>
        /// 利用构造函数来创建答案内容
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
    /// 绑定答案栏按钮和题目内容
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
        /// 设置当前可见题目的UI信息
        /// </summary>
        /// <param name="num"></param>
        /// <param name="_p"></param>
        internal void SetVisible(int num, Paper _p)
        {
            _p.bg.sprite = Resources.Load<Sprite>(anItem.bg);
            //设置题目
            _p.problemText.text = (num + 1).ToString() + "." + anItem.topic;
            //设置答案选项
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
