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
        #region 序列化类
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
            /// 初始化UI时，显示两个按钮控件（巡视、倒闸）
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
            /// 清理之前生成的预制体
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
            /// 学练考UI按键显示
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
            /// 清理之前生成的预制体
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
            public GameObject taskBg1; //巡视任务列表的背景
            public GameObject taskBg2; //倒闸任务列表的背景
            public GameObject _taskList; //任务列表父物体
            public Task task = new Task();
            public GameObject ElecImage; //接线图

            internal void InitStart()
            {
                //所有的Toggle都可交互
                for (int i = 0; i < toggles.Count; i++)
                {
                    toggles[i].interactable = true;
                }
                //为三个Toggle添加值改变监听事件
                toggles[0].onValueChanged.AddListener(MapToggleChanged);
                toggles[1].onValueChanged.AddListener(TaskToggleChanged);
                toggles[2].onValueChanged.AddListener(HelpToggleChanged);
                toggles[3].onValueChanged.AddListener(ElecToggleChanged);
                uiobj.SetActive(true);
                task.Init();
            }

            //地图Toggle
            private void MapToggleChanged(bool isOn)
            {
                //获取Main场景中的地图
                GameObject.Find("Maps").GetComponent<MiniMap>().minMap.SetActive(isOn);
            }

            //任务栏Toggle
            private void TaskToggleChanged(bool isOn)
            {
                //task.taskTogList.Clear();
                _taskList.SetActive(isOn);
                //替换巡视与倒闸的任务栏背景
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

            //帮助Toggle
            private void HelpToggleChanged(bool isOn)
            {
                //隐藏当前进行中的ui
                Instance.RestAllUI();
                if(Instance._ModelUI.ModelRawImg)
                    Instance._ModelUI.ModelRawImg.SetActive(false);
                //停止所有在播放的声音
                AudioPlay.Instance.StopAudio();
                //停止主变声音
                AudioSource ZBVoice = GameObject.Find("ZhubianVoice").GetComponent<AudioSource>();
                if (ZBVoice)
                {
                    ZBVoice.Stop();
                }
                Instance._GuideUI.Promuiobj.SetActive(true);
            }

            //接线图Toggle
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
            public Transform TagObj;  //标签父物体
            internal List<BtnUnit> tags = new List<BtnUnit>();
            public List<GameObject> Content = new List<GameObject>();
            public ScrollRect _ScrollRect;
            public GameObject OverBtn;  //查看完毕按钮
            private int index;
            private string stateCode;
            internal bool IsImmediate = true;   //立即销毁
            internal PlayerMove pm;    //用于在查看介绍ui时不会影响到鼠标拖动改变人物视角

            /// <summary>
            /// 初始化IntroUI
            /// </summary>
            /// <param name="prefab">预制体路径</param>
            /// <param name="sprite">IntroUI背景图路径</param>
            internal void InitStart(string prefab, string sprite)
            {
                OverBtn.SetActive(false);
                if (tags != null)
                    ClearData();

                bg.sprite = Resources.Load<Sprite>(sprite);
                index = 0;  //标签编号从0开始计算
                foreach (SubList _d in GameUnitManager.Instance.SubBtn)
                {
                    //分析标签对应的是文本还是图片
                    bool _tag = AnalyzeContents(_d);
                    //加载标签预制体
                    BtnUnit _res = Manager.Instance.InitResObj(TagObj, prefab).GetComponent<BtnUnit>();
                    //设置标签按钮信息
                    _res.SetSubBtn(index, _d, stateCode, _tag);
                    tags.Add(_res);
                    index++;
                }

                Content[2].SetActive(false);
                uiobj.SetActive(true);
                //初始滚动条都在最上面
                if (_ScrollRect.verticalScrollbar.gameObject.activeSelf)
                {
                    _ScrollRect.verticalScrollbar.value = 1;
                }

                //在看IntroUI时关闭人物视角的旋转
                pm = GameObject.Find("Player").GetComponent<PlayerMove>();
                pm.IsRotationControl = false;
            }

            /// <summary>
            /// 分析文本内容（文本/图片）
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
            /// 销毁并清除标签列表信息
            /// </summary>
            private void ClearData()
            {
                foreach (BtnUnit _d in tags)
                    Destroy(_d.gameObject);
                tags.Clear();
            }

            /// <summary>
            /// 重置组件
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
            //标签按钮物体，在Inspector导入
            public List<Transform> TagObj = new List<Transform>();
            public GameObject ModelRawImg;
            public Image ModelIntroImg;
            public List<GameObject> ActiveObj;
            //存储设置已初始化完成的标签信息
            internal List<ModelToggle> toggleList = new List<ModelToggle>();
            //存储所有模型信息
            internal SortedList<int, ModelTable> models = new SortedList<int, ModelTable>();

            private int index;   //标签按钮的ID
            internal int tgid;   //目前正在执行的标签ID
            private int tgmax;   //目前已开发完成的标签最大ID
            internal int tgtmp;  //临时存储目前正在执行的标签ID

            //所有模型步骤计算
            internal int Objstep;
            //标签对应的模型步骤初始结束设置
            internal int stepStart;
            internal int stepEnd;
            //标签对应的模型步骤计算
            internal int step;  
            internal URPHighlightableObject hc;
            internal int toggleType;

            //存放内部结构需显示的模型
            internal GameObject _innerobj;

            //主变声音按钮
            public GameObject HearBtn;
            //下方红字提示
            public GameObject _Tips;

            internal PlayerMove pm;    //用于在查看模型介绍时不会影响到鼠标左键拖动改变人物视角

            /// <summary>
            /// 初始化二级模型展示UI
            /// </summary>
            internal void InitStart()
            {
                switch (GameUnitManager.Instance._SceneType)
                {
                    case SceneType.xunshi:
                        AnalyzeModels();  //解析获取需要展示的模型部件
                        ModelRawImg = GameObject.Find("Canvas").transform.Find("ModelRaw").gameObject;
                        ModelRawImg.SetActive(true);
                        index = 1;   //标签按钮的ID从1开始计算
                        toggleList.Clear();
                        foreach (BtnMenu _d in Instance.menu_3)
                        {
                            ModelToggle _res;
                            if (!TagObj[index - 1].gameObject.GetComponent<ModelToggle>())
                                _res = TagObj[index - 1].gameObject.AddComponent<ModelToggle>();
                            else
                                _res = TagObj[index - 1].gameObject.GetComponent<ModelToggle>();
                            _res._toggle = TagObj[index - 1].gameObject.GetComponent<Toggle>();
                            //设置标签对应的信息
                            _res.SetModelToggle(index, _d.BtnData, _d.BtnRes);
                            toggleList.Add(_res);
                            index++;
                        }
                        uiobj.SetActive(true);
                        /*if(PlayerMove.Instance._param.URPCam != null)
                            //设置主相机看不见outline层
                            PlayerMove.Instance._param.URPCam.GetUniversalAdditionalCameraData().volumeLayerMask &= -(1 << 8);*/
                        break;
                }

                pm = GameObject.Find("Player").GetComponent<PlayerMove>();
                pm.IsRotationControl = false;

                tgid = 1;  //默认初始从第一个标签开始介绍
                SetToggle(1);
            }

            /// <summary>
            /// 解析当前模型的所有部件介绍
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
            /// 设置标签被按下对应的信息
            /// </summary>
            /// <param name="id">被按下的标签ID</param>
            internal void SetToggle(int id)
            {
                SetTogglePic(id);
                IsToggle();
            }

            /// <summary>
            /// 设置标签图案
            /// </summary>
            /// <param name="id">被按下的标签ID</param>
            internal void SetTogglePic(int id)
            {
                //遍历所有标签
                foreach (ModelToggle mt in toggleList)
                {
                    //把所有标签都设置成关闭状态
                    mt._toggle.isOn = false;
                    mt.transform.gameObject.GetComponent<Image>().sprite = mt.clickBefore;
                    //当被遍历到的标签是所需被按下的标签
                    if (mt.ID == id)
                    {
                        //打开该标签
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
            /// 如果标签被按下，设置展示模型的初始值
            /// </summary>
            internal void IsToggle()
            {
                //遍历所有标签
                foreach (ModelToggle mt in toggleList)
                {
                    //判断该标签是否被按下
                    if (mt._toggle.isOn)
                    {
                        DevelopOver(mt.ID);
                    }
                }
            }

            /// <summary>
            /// 根据标签内容开发情况设置相应的内容
            /// </summary>
            /// <param name="ty">标签ID，其值与标签类型一致</param>
            private void DevelopOver(int ty)
            {
                //若该标签已经开发完成
                if (ty <= tgmax)
                {
                    tgtmp = ty;   //记录当前标签编号
                    SetStep(ty);  //设置步骤初始值
                    ActiveModelUI(true);  //显示模型相关UI
                    Instance._TodoUI.SetActive(false);
                    ExDisplay();  //执行模型介绍
                    _Tips.SetActive(true);
                }
                else  //标签对应功能未开发完成
                {
                    ActiveModelUI(false); //关闭模型相关UI
                    AudioPlay.Instance.StopAudio(); //停止播放声音
                    Instance._TodoUI.SetActive(true); //打开未开发提示信息
                    _Tips.SetActive(false);
                }
            }

            /// <summary>
            /// 激活模型介绍相关UI
            /// </summary>
            /// <param name="_bool">是否要激活显示</param>
            private void ActiveModelUI(bool _bool)
            {
                foreach (GameObject _o in ActiveObj)
                {
                    _o.SetActive(_bool);
                }
                ModelRawImg.SetActive(_bool);
            }

            /// <summary>
            /// 设置标签对应模型初始值
            /// </summary>
            /// <param name="toggleType">标签类型编号</param>
            internal void SetStep(int toggleType)
            {
                List<ModelTable> temp = new List<ModelTable>();
                //遍历所有模型部件
                foreach (ModelTable mt in models.Values)
                {
                    if (mt.ImgRes.Contains("|"))
                    {
                        string[] _s = mt.ImgRes.Split('|');
                        //记录符合当前标签的模型部件数
                        if (int.Parse(_s[1]) == toggleType)
                        {
                            temp.Add(mt);
                            //ImgRes第三个参数存储主步骤初始值
                            Objstep = int.Parse(_s[2]);
                        }
                    }
                }
                stepStart = 1;  //标签对应的模型部件初始值
                stepEnd = temp.Count; //标签对应的模型部件总数
                step = 1;  //默认标签对应的模型部件从1开始介绍
            }

            /// <summary>
            /// 二级UI中的模型介绍
            /// </summary>
            internal void ExDisplay()
            {
                if (step >= stepStart && step <= stepEnd)
                {
                    //高亮
                    GameObject _obj = GameObject.Find(models[Objstep].ModelObj);
                    if (_obj)
                    {
                        hc = _obj.AddComponent<URPHighlightableObject>();
                    }
                    //学习主变的内部结构,只显示本身
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
                    //模型介绍图
                    if (models[Objstep].ImgRes.Contains("|"))
                    {
                        string[] _s = models[Objstep].ImgRes.Split('|');
                        ModelIntroImg.sprite = Resources.Load<Sprite>(_s[0]);
                    }
                    //主变声音按钮
                    if (models[Objstep].BtnRes.Contains("Hearing"))
                    {
                        HearBtn.SetActive(true);
                    }
                    else
                    {
                        HearBtn.SetActive(false);
                    }
                    //图片自适应
                    ModelIntroImg.SetNativeSize();
                    //播放模型介绍声音
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
            internal int ActiveArea;   //当前对话区域
            internal bool IsAlpha = false;

            /// <summary>
            /// UI初始设置
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
            /// 设置按钮
            /// </summary>
            /// <param name="pic1">按钮按下前图片</param>
            /// <param name="pic2">按钮进入后的图片</param>
            internal void SetBtn(string pic1, string pic2)
            {
                //设置按钮事件
                cb = btn.GetComponent<ControlBtn>();
                cb.btnType = BtnType.Prompt;
                cb.Init(pic1, pic2);
                //按钮图片获取
                btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(pic1);
                btn.GetComponent<Image>().SetNativeSize();
                //按钮显示
                btn.SetActive(true);
            }

            /// <summary>
            /// 设置透明度
            /// </summary>
            internal void SetAlpha()
            {
                IsAlpha = false;
                //当前对话区域是值班长区域
                if (ActiveArea == 1)
                {
                    uTweenAlpha.Begin(dianHuaUI.bg, 1f, 0f, 1f, 0.5f);
                    uTweenAlpha.Begin(dianHuaUI.content.gameObject, 1f, 0f, 1f, 0.5f);
                }
                else if (ActiveArea == 2)  //当前对话区域是操作人区域
                {
                    uTweenAlpha.Begin(huiFuUI.bg, 1f, 0f, 1f, 0.5f);
                    uTweenAlpha.Begin(huiFuUI.content.gameObject, 1f, 0f, 1f, 0.5f);
                    clickTag.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 倒闸学习模式中每一大步结束后的提示信息
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
            /// 提示UI初始设置
            /// </summary>
            /// <param name="_s">当前环节编号</param>
            /// <param name="start">当前环节的初始步骤</param>
            /// <param name="area">当前环节最后一步所在场景区域编号</param>
            internal void Init(string _s, string start, string area)
            {
                step = int.Parse(_s);
                stepStart = int.Parse(start);
                //遍历所有环节结束提示信息
                foreach (DzStepTip dt in Manager.Instance.dzTips.Values)
                {
                    //获取到当前环节对应的提示信息
                    if (dt.ID == step)
                    {
                        //设置提示信息UI
                        AnalyzeTip(dt, area);
                        uiobj.SetActive(true);
                    }
                }
            }

            /// <summary>
            /// 将数据库中的信息内容设置到UI上
            /// </summary>
            /// <param name="dt">数据库中存储的提示信息相关内容</param>
            /// <param name="area">当前环节最后一步所在场景区域编号</param>
            private void AnalyzeTip(DzStepTip dt, string area)
            {
                //按钮被按下之前的图片
                string[] _s1 = dt.BtnBefore.Split('|');
                //按钮被按下之后的图片
                string[] _s2 = dt.BtnAfter.Split('|');
                //提示信息内容
                tip.sprite = Resources.Load<Sprite>(dt.ImgRes);
                //获取按钮上的TipBtn组件
                TipBtn tb1 = btn1.GetComponent<TipBtn>();
                TipBtn tb2 = btn2.GetComponent<TipBtn>();
                //设置两个按钮的按钮类型
                tb1.btnType = BtnType.TipBtn1;
                tb2.btnType = BtnType.TipBtn2;
                //初始化两个按钮的信息
                tb1.Init(_s1[0], _s2[0], stepStart, area);
                tb2.Init(_s1[1], _s2[1], stepStart, area);
            }
        }

        /// <summary>
        /// 巡检实践模式的选择UI
        /// </summary>
        [Serializable]
        public class SelectPracticeUI
        {
            public GameObject uiobj; //UI本身
            public Transform Master; //需生成按钮预制体的父物体位置
            private SortedList<int, CkPBtn> AllUnit = new SortedList<int, CkPBtn>();
            private int id = 0;

            /// <summary>
            /// 巡视选择UI按钮显示
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
            /// 清理之前生成的预制体
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
            public AudioSource ZhubianVoice; //主变的嗡嗡声

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
            public GameObject Promuiobj;    //是否查看教程ui
            public Button yesBtn;   //是按钮
            public Button noBtn;    //否按钮
            public Button closeBtn; //叉叉

            public GuidanceUI guidanceUI;
            public Transform _ui;
            internal int index = 1;
            private SortedList<int, LightTable> light = new SortedList<int, LightTable>();

            public GameObject Doneuiobj;    //学习教程完毕ui
            public Button againBtn;     //重新体验按钮
            public Button nextBtn;      //自主探索按钮

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
                //隐藏实际的模型
                Instance._ModelUI.uiobj.SetActive(false);
                if(Instance._ModelUI.ModelRawImg)
                    Instance._ModelUI.ModelRawImg.SetActive(false);
                RestUI();
                guidanceUI.Init();
                guidanceUI.target = GameObject.Find(light[index].LightTarget).gameObject.GetComponent<RectTransform>();
                //设置
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
            /// 教程提示信息随着亮处向左或向右偏移一定位置
            /// </summary>
            /// <param name="arrow">引导箭头图标</param>
            /// <param name="m_Target">高亮的区域</param>
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
                            //替换引导箭头的朝向
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
            /// 判断是否是初次引导，是否继续上一步执行的步骤
            /// </summary>
            internal void GoOnTheStep()
            {
                //不是初次引导
                if (GameUnitManager.Instance.isFirst)
                {
                    Instance._Title.InitStart();
                    GameUnitManager.Instance.SelectUnit();
                    GameUnitManager.Instance.isFirst = false;
                }
                else
                {
                    Instance._Title.uiobj.SetActive(true);
                    //根据当前场景继续执行当前步骤
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
            //答题卡
            public GameObject ecMaster;
            //交卷后的结果卡
            public AnswerCard answerCard;
            //交卷未全部完成确认卡
            public GameObject cfMaster;
            //交卷成功
            public GameObject resMaster;
            //得分
            public Text Point;
            //单选题父物体
            public Transform sinBtnContent;
            //多选题父物体
            public Transform mulBtnContent;
            //填空题父物体
            public Transform judgeBtnContent;
            //交卷按钮
            public Button submit;
            public Button previousBtn;//上一题按钮
            public Button nextBtn;//下一题按钮
            //题目内容
            public Paper paper;
            //左侧题目列表数据，存储题目按钮及对应题目内容
            internal List<QuestionNum> _question = new List<QuestionNum>();
            internal int questionCount;//当前题目序号
            internal int answerCount; //当前答案序号
            private int id; //题目计数
            internal int SelectedNum;
            internal bool isSubmit;
            internal int score;  //得分
            private Coroutine stopC;

            //20221117新增：存放要进入现场的题目编号
            internal SortedList<int, FaultState> faultScene = new SortedList<int, FaultState>();

            //20221128新增：选择题目
            public Button modifyBtn;
            public ExamChoiceUI _examChoiceUI = new ExamChoiceUI();

            /// <summary>
            /// 考核UI初始化
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
                time.text = "00分00秒";
                ClearObj();
                faultScene.Clear();
                Instance.StopAllCoroutines();
                //添加按钮点击监听事件
                previousBtn.onClick.AddListener(PreviousClick);
                nextBtn.onClick.AddListener(NextClick);
                submit.onClick.AddListener(SubmitClick);
                switch (GameUnitManager.Instance._SceneType)
                {
                    case SceneType.xunshi:
                        //单选
                        InitPanel(_examChoiceUI.XsSingleTopic, 10, 1);
                        //多选
                        InitPanel(_examChoiceUI.XsDoubleTopic, 5, 2);
                        //判断
                        InitPanel(_examChoiceUI.XsJudgeTopic, 5, 3);
                        break;
                    case SceneType.daozha:
                        //单选
                        InitPanel(_examChoiceUI.DzSingleTopic, 10, 1);
                        //多选
                        InitPanel(_examChoiceUI.DzDoubleTopic, 5, 2);
                        //判断
                        InitPanel(_examChoiceUI.DzJudgeTopic, 5, 3);
                        break;
                }
                record.transform.parent.GetComponent<Text>().text = "已答题：";
                record.text = SelectedNum.ToString() + "/20";
                //初始显示第一题
                SetPaper(questionCount);
                Instance.spendTime = 0;
                stopC = Instance.StartCoroutine(Instance.CountTime());
            }

            /// <summary>
            /// 清空数据
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
                //移除按钮上的点击事件监听
                previousBtn.onClick.RemoveAllListeners();
                nextBtn.onClick.RemoveAllListeners();
                submit.onClick.RemoveAllListeners();
            }

            /// <summary>
            /// 销毁答题卡上的预制体
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
            /// 销毁交卷后结果卡上的预制体
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
            /// 销毁答案选项预制体
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
            /// 初始化答题面板
            /// </summary>
            /// <param name="qd">题库中对应题型的所有题目</param>
            /// <param name="count">设置题型所需题目数量</param>
            /// <param name="type">题型类型</param>
            private void InitPanel(List<QuestionItemDatas> qd, int count, int type)
            {
                //随机获取题目数量
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
                    //20221129新增：绑定题目与故障情况
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
            /// 设置题目内容
            /// </summary>
            /// <param name="i">题目编号</param>
            /// <param name="temp">对应题目编号内容</param>
            /// <param name="pic">题目背景</param>
            /// <param name="btnContent">题目按钮预制体在答题卡上的父物体</param>
            private void SetQuestion(int i, QuestionItemDatas temp, string pic, Transform btnContent)
            {
                //题目内容
                QuestionItem qi = new QuestionItem();
                ToggleGroup BtnGroup;
                //题目对应按钮
                QuestionBtnItem btnItem;
                QuestionNum questionNum;

                qi.bg = pic;
                qi.qItemData = temp;
                BtnGroup = btnContent.transform.parent.GetComponent<ToggleGroup>();
                //实例化答题卡题目Toggle
                btnItem = Manager.Instance.InitResObj(btnContent, "UI/Prefabs/QuestionTogg").GetComponent<QuestionBtnItem>();
                btnItem.Init(id, i + 1);
                btnItem.gameObject.GetComponent<Toggle>().group = BtnGroup;
                //绑定Toggle和题目内容
                questionNum = new QuestionNum(i + 1, btnItem, qi);
                _question.Add(questionNum);
            }

            /// <summary>
            /// 设置当前题目应在页面上显示的内容
            /// </summary>
            /// <param name="index"></param>
            internal void SetPaper(int index)
            {
                //20221117新增：遍历故障点
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
            /// 上一题点击事件
            /// </summary>
            private void PreviousClick()
            {
                //判断是答题卡还是答案卡
                if (!isSubmit)
                {
                    if (questionCount > 0)
                    {
                        questionCount--;
                        CheckOtherQuestion();
                        //修改题目按钮状态
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
            /// 下一题点击事件
            /// </summary>
            private void NextClick()
            {
                //判断是答题卡还是答案卡
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
            /// 检查其他未作答的题目
            /// </summary>
            private void CheckOtherQuestion()
            {
                for (int i = 0; i < _question.Count; i++)
                {
                    if (!_question[i].bitem.isSelectAnswer)
                    {
                        _question[i].bitem.ChangeQuestionState(QuestionState.Default);//未作答恢复默认状态
                    }
                }
            }
            /// <summary>
            /// 提交事件
            /// </summary>
            private void SubmitClick()
            {
                CalculateScore(); //计算答题分数
                for (int i = 0; i < _question.Count; i++)
                {
                    //判断是否所有题目都已答完
                    if (!_question[i].bitem.isSelectAnswer)
                    {
                        cfMaster.SetActive(true);
                        return;
                    }
                }
                ConfirmSubmit(); //所有题目答完，确认交卷
            }

            /// <summary>
            /// 交卷成功
            /// </summary>
            internal void ConfirmSubmit()
            {
                cfMaster.SetActive(false);
                resMaster.SetActive(true);
                Point.text = score.ToString() + "分";
                Instance.StopCoroutine(stopC);  //关闭计时协程
            }

            /// <summary>
            /// 取消交卷
            /// </summary>
            internal void CancelSubmit()
            {
                isSubmit = false;
                cfMaster.SetActive(false);
                resMaster.SetActive(false);
            }

            /// <summary>
            /// 查看答题
            /// </summary>
            internal void LookAnswer()
            {
                isSubmit = true;
                resMaster.SetActive(false);
                ecMaster.SetActive(false);
                record.transform.parent.GetComponent<Text>().text = "错误率：";
                time.text = string.Format("{0:D2}分{1:D2}秒", Instance.minute, Instance.second);
                answerCard.master.SetActive(true);
                answerCard.Init(_question);
                answerCard.SetPaper(paper, answerCount);
            }

            /// <summary>
            /// 再次答题
            /// </summary>
            internal void Again()
            {
                isSubmit = false;
                resMaster.SetActive(false);
                Init();
            }

            /// <summary>
            /// 计算分值
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
            /// 20221128新增：点击修改题目按钮，弹出界面
            /// </summary>
            internal void ModifyClick()
            {
                Instance._FaultModUI.uiobj.SetActive(false);
                _examChoiceUI.SetChoice();
                _examChoiceUI.uiobj.SetActive(true);
            }
        }

        /// <summary>
        /// 20221128新增：考核考题选择
        /// </summary>
        [Serializable]
        public class ExamChoiceUI
        {
            public GameObject uiobj;
            public Transform ContentParent;  //内容预制体父对象
            public Button cancelBtn;         //取消按钮
            public Button confirmBtn;        //确认按钮
            public GameObject tipUI;         //未满足选题条件弹出的提示框
            public Text[] TopicCnt;          //当前选中的题目数量计数显示
            internal bool isFirst = true;    //是否是第一次初始化

            private Coroutine stop;
            internal int sin = 0;
            internal int dou = 0;
            internal int judg = 0;

            //巡视考核题目（单选、多选及判断）
            internal List<QuestionItemDatas> XsSingleTopic;
            internal List<QuestionItemDatas> XsDoubleTopic;
            internal List<QuestionItemDatas> XsJudgeTopic;
            //倒闸考核题目（单选、多选及判断）
            internal List<QuestionItemDatas> DzSingleTopic;
            internal List<QuestionItemDatas> DzDoubleTopic;
            internal List<QuestionItemDatas> DzJudgeTopic;

            /// <summary>
            /// 选项结构体
            /// </summary>
            internal struct Choice
            {
                public QuestionItemDatas qData;
                public bool isKaoHe;
                public FaultType type;
            }
            
            internal SortedList<int, Choice> XsChoice = new SortedList<int, Choice>();   //记录巡视题库相关信息
            internal SortedList<int, Choice> DzChoice = new SortedList<int, Choice>();   //记录倒闸题库相关信息
            internal SortedList<int, ChoiceUIItem> items = new SortedList<int, ChoiceUIItem>();  //存储选项预制体相关信息

            /// <summary>
            /// 初始化修改前内容
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
            /// 打开选择题目面板后设置选项
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
                            s = "【单选】";
                            break;
                        case 2:
                            s = "【多选】";
                            break;
                        case 3:
                            s = "【判断】";
                            break;
                    }
                    //绑定数据并填充内容
                    ChoiceUIItem item = Manager.Instance.InitResObj(ContentParent, "UI/Prefabs/choiceItem").GetComponent<ChoiceUIItem>();
                    item.itemCnt.text = s + c.qData.topic;
                    item.isKaoHeBtn.isOn = c.isKaoHe;
                    //根据是否需要设置故障来确定故障选项是否可交互
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
                    //替换数据或加入数据
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
            /// 清除选项item预制体
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
            /// 清除题库数据
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
            /// 初始化列表数据，只在第一次初始化ChoiceUI时调用
            /// </summary>
            /// <param name="type"></param>
            /// <param name="sin"></param>
            /// <param name="dou"></param>
            /// <param name="judg"></param>
            internal void InitData(string type, List<QuestionItemDatas> sin, List<QuestionItemDatas> dou, List<QuestionItemDatas> judg)
            {
                int i = 1;
                //单选
                foreach(QuestionItemDatas s in sin)
                {
                    Choice c = new Choice();
                    c.qData = s;
                    c.isKaoHe = true;
                    c.type = FaultType.Fault;
                    SetList(type, c, i);
                    i++;
                }
                //多选
                foreach (QuestionItemDatas d in dou)
                {
                    Choice c = new Choice();
                    c.qData = d;
                    c.isKaoHe = true;
                    c.type = FaultType.Fault;
                    SetList(type, c, i);
                    i++;
                }
                //判断
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
            /// 将数据添加到列表中
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
            /// 点击取消按钮
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
            /// 点击确认按钮
            /// </summary>
            internal void ConfirmClick()
            {
                //未满足条件
                if (sin < 10 || dou < 5 || judg < 5)
                {
                    tipUI.SetActive(true);
                    return;
                }
                //清除题库数据
                if(GameUnitManager.Instance._SceneType == SceneType.xunshi)
                    ClearData(XsSingleTopic, XsDoubleTopic, XsJudgeTopic);
                else if(GameUnitManager.Instance._SceneType == SceneType.daozha)
                    ClearData(DzSingleTopic, DzDoubleTopic, DzJudgeTopic);
                for (int i = 1;i <= items.Count; i++)
                {
                    bool isFix = false;   //该题是否被修改过参数
                    switch (GameUnitManager.Instance._SceneType)
                    {
                        case SceneType.xunshi:
                            //更新修改数据
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
                            //若是考核，将题目放入到题库中
                            if (XsChoice[i].isKaoHe)
                            {
                                //若设置成故障类型，则该题的showFault为TRUE
                                if (XsChoice[i].qData.existFault == 1 && XsChoice[i].type == FaultType.Fault)
                                {
                                    XsChoice[i].qData.showFault = true;
                                }
                                else
                                {
                                    XsChoice[i].qData.showFault = false;
                                }
                                XsChoice[i].qData.ResetAnswer();   //重置答案
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
                            //更新修改数据
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
                            //若是考核，将题目放入到题库中
                            if (DzChoice[i].isKaoHe)
                            {
                                //若设置成故障类型，则该题的showFault为TRUE
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
            /// 获取故障类型
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
        /// 动画执行显示窗口
        /// </summary>
        [Serializable]
        public class ExAnimWin
        {
            public GameObject uiobj;
            public bool isShow = false;

            /// <summary>
            /// 窗口是否显示
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
        /// 20221117新增：故障UI
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

        //20230208新增：存储用于右上角菜单显示的文字的字体
        public GUISkin guiSkin;

        internal List<BtnMenu> menu_1 = new List<BtnMenu>();
        internal List<BtnMenu> menu_2 = new List<BtnMenu>();
        internal List<BtnMenu> menu_3 = new List<BtnMenu>();
        internal List<BtnMenu> menu_4 = new List<BtnMenu>();
        internal List<CheckPracticeTable> checkPracticeData = new List<CheckPracticeTable>();

        //20221123新添：存储需要替换字符串的键值对数据
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

        #region 数据表字段解析
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

        #region 功能
        /// <summary>
        /// 切换UI
        /// scene表示显示选择场景UI
        /// mode表示显示模式选择UI
        /// main表示主场景中需要一直显示的UI
        /// modelintro表示模型介绍左侧显示的UI
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
                    //禁用标题栏中除返回主标题外的按钮
                    for(int i = 0; i < _Title.toggles.Count; i++)
                    {
                        _Title.toggles[i].interactable = false;
                    }
                    _ExamUI.Init();
                    break;
            }
        }

        /// <summary>
        /// 返回初始界面的功能实现
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
        /// 巡视中二级UI的"下一个"按钮功能
        /// 在Unity中绑定
        /// </summary>
        public void NextObjBtn()
        {
            //停止播放音乐
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
                //当所有部件展示完后，前往下一个地点
                if (Instance._ModelUI.Objstep > Instance._ModelUI.models.Count)
                {
                    _ModelUI.uiobj.SetActive(false);
                    _ModelUI.ModelRawImg.SetActive(false);
                    _ModelUI.pm.IsRotationControl = true;
                    GameUnitManager.Instance._Scene1.NextStep();
                }
                else
                {
                    Instance._ModelUI.tgid++;  //前一个标签结束介绍，设置下一个标签
                    Instance._ModelUI.SetToggle(Instance._ModelUI.tgid);
                }
            }
            else
            {
                Instance._ModelUI.ExDisplay();
            }
        }

        /// <summary>
        /// 取消未开发提示信息，恢复到之前的模型介绍上
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
        /// 考核中交卷提交提示的确认键
        /// </summary>
        public void ExamConfirmSub()
        {
            _ExamUI.ConfirmSubmit();
        }

        /// <summary>
        /// 考核中交卷提交提示的取消键
        /// </summary>
        public void ExamConfirmCan()
        {
            _ExamUI.CancelSubmit();
        }

        /// <summary>
        /// 考核中查看答卷按钮
        /// </summary>
        public void ExamLookAnswer()
        {
            _ExamUI.LookAnswer();
        }

        /// <summary>
        /// 考核中再次答题
        /// </summary>
        public void ExamAgain()
        {
            _ExamUI.Again();
        }

        /// <summary>
        /// 计算考试用时的协程
        /// </summary>
        /// <returns></returns>
        public IEnumerator CountTime()
        {
            while (true)
            {
                spendTime += Time.deltaTime;  //将时间转化为时分秒

                hour = (int)spendTime / 3600;
                minute = (int)(spendTime - hour * 3600) / 60;
                second = (int)(spendTime - hour * 3600 - minute * 60);
                _ExamUI.time.text = string.Format("{0:D2}分{1:D2}秒", minute, second);
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
                        if (it.itemCnt.text.Contains("【单选】"))
                            sin++;
                        else if (it.itemCnt.text.Contains("【多选】"))
                            dou++;
                        else if (it.itemCnt.text.Contains("【判断】"))
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
            //当Intro UI打开时，监测按键是否是最后一个且滑动条在底部
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
        /// 部分按钮的点击事件处理
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
        /// 替换文本中的时间串
        /// </summary>
        public string ReplaceCnt(string cnt, int type)
        {
            string tmpStr = cnt;
            if (tmpStr.Contains("x年x月x日x时x分"))
            {
                //当是监护人说话时，记录当前时间；操作人进行复诵，重复监护人记录的时间
                if(type == 1)
                {
                    StrDic.Clear();
                    string year = DateTime.Now.Year.ToString() + "年";
                    string month = DateTime.Now.Month.ToString() + "月";
                    string day = DateTime.Now.Day.ToString() + "日";
                    string hour = DateTime.Now.Hour.ToString() + "时";
                    string minute = DateTime.Now.Minute.ToString() + "分";
                    StrDic.Add("x年", year);
                    StrDic.Add("x月", month);
                    StrDic.Add("x日", day);
                    StrDic.Add("x时", hour);
                    StrDic.Add("x分", minute);
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
