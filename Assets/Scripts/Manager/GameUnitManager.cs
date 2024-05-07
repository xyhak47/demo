using System;
using System.Collections;
using System.Collections.Generic;
using XD.GameStatic;
using UnityEngine;
using XD.UI;
using uTools;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;
using XD.Tasks;
using UnityEngine.Rendering.Universal;

namespace XD.TheManager
{
    public class GameUnitManager : MonoBehaviour
    {
        #region 序列化类
        /// <summary>
        /// 例行巡视步骤功能处理
        /// </summary>
        public class Scene1
        {
            internal int Tmpstep;   //CheckGuideTable中的大步骤
            private string _moveObj;  //需高亮的物体的名称
            private Vector3 _destination; //人物移动的目的地

            internal string _modelType; //模型类别
            internal GameObject _obj;  //存放需高亮的物体
            internal int togID; //任务列表的任务ID

            /// <summary>
            /// 初始化
            /// </summary>
            internal void InitStart()
            {
                Tmpstep = 1;
                togID = 0;
                ExOperate();
            }

            /// <summary>
            /// 执行步骤
            /// </summary>
            internal void ExOperate()
            {
                CheckGuideTable _d = Manager.Instance.AllChgSteps[Tmpstep];
                //PlayerMove.Instance.SetPosAndRot();
                if (_d.Operate.Contains(":"))
                {
                    string[] _s = _d.Operate.Split(':');
                    switch (_s[0])
                    {
                        case "ui":
                            //初始引导文字显示
                            UIManager.Instance._StartTipsUI.SetActive(true);
                            if (_s[1].Contains(","))
                            {
                                string[] _ss = _s[1].Split(',');
                                Instance.GetSubBtn(_ss[0]);
                                //显示初始介绍ui
                                UIManager.Instance._IntroUI.InitStart(_ss[1], _ss[2]);
                            }
                            break;
                        case "move":
                            //初始引导文字结束
                            UIManager.Instance._StartTipsUI.SetActive(false);
                            _destination = Manager.Instance.Vector3Parse(_d.Position);
                            //添加LineRenderer并设置参数
                            //PlayerMove.Instance.line = PlayerMove.Instance.transform.gameObject.AddComponent<LineRenderer>();
                            //PlayerMove.Instance.SetLine();
                            //显示引导提示
                            UIManager.Instance._GuideTips.SetActive(true);
                            //显示引导线
                            PlayerMove.Instance.ArrowP.gameObject.SetActive(true);
                            //允许人物移动
                            PlayerMove.Instance.IsMove = true;
                            //设置导航目标地
                            PlayerMove.Instance.navMeshAgent.destination = _destination;
                            //设置高亮模型名称
                            _moveObj = _s[1];
                            //存储当前任务栏的显示状态
                            PlayerMove.Instance.togOn = UIManager.Instance._Title.toggles[1].isOn;
                            break;
                        case "click":
                            if (_s[1].Contains(","))
                            {
                                string[] _ss = _s[1].Split(',');
                                _modelType = _ss[1];
                                //激活RawImage对应展示的3D模型
                                PlayerMove.Instance._ui3DModel.ActiveMaster(_ss[0]);
                            }
                            //根据物体名称找到需高亮的物体
                            _obj = GameObject.Find(_moveObj);
                            if (_obj)
                            {
                                //高亮物体
                                _obj.AddComponent<URPHighlightableObject>();
                            }
                            break;
                    }
                }
            }

            /// <summary>
            /// 下一步
            /// </summary>
            internal void NextStep()
            {
                if (Tmpstep < Manager.Instance.AllChgSteps.Count)
                {
                    Tmpstep++;
                    //更新任务栏列表状态
                    UIManager.Instance._Title.task.SetTaskList(Tmpstep, ref togID);
                    //继续执行步骤
                    ExOperate();
                }
                else
                {
                    //隐藏模型介绍ui
                    UIManager.Instance._ModelUI.uiobj.SetActive(false);
                    //设置主相机看得见outline层
                    //PlayerMove.Instance._param.URPCam.GetUniversalAdditionalCameraData().volumeLayerMask |= -(1 << 8);
                    //返回主界面
                    UIManager.Instance.Back();
                }
            }

            /// <summary>
            /// 学习内部模型时，透明外壳
            /// </summary>
            internal void SetTransparent(GameObject innerObj)
            {
                foreach(GameObject _model in PlayerMove.Instance._param.ZhubianModels)
                {
                    if(!_model.name.Equals(innerObj.name))
                    {
                        _model.SetActive(false);
                    }
                    else
                    {
                        _model.SetActive(true);
                    }
                }
            }

            /// <summary>
            /// 恢复显示隐藏的模型
            /// </summary>
            internal void RestTransparent()
            {
                foreach (GameObject _model in PlayerMove.Instance._param.ZhubianModels)
                {
                    _model.SetActive(true);
                }
            }
        }
        /// <summary>
        /// 例行巡视练习
        /// </summary>
        public class ScenePractice
        {
            internal int Tmpstep; //步骤
            private string _moveObj; //需高亮的物体
            private Vector3 _destination; //人物移动的目的地
            private Vector3 _rotation; //人物旋转方向
            internal int togID; //任务列表的任务ID
            private Coroutine stopZ; //用于储存音频停止播放的协程

            /// <summary>
            /// 初始化
            /// </summary>
            internal void InitStart()
            {
                Tmpstep = 0;
                ExOperate();
            }

            /// <summary>
            /// 执行操作
            /// </summary>
            internal void ExOperate()
            {
                CheckPracticeTable _d = UIManager.Instance.checkPracticeData[Tmpstep];
                if (_d.Operate.Contains(":"))
                {
                    string[] _s = _d.Operate.Split(':');
                    switch (_s[0])
                    {
                        case "at":
                            //从数据表中获取用户的位置及旋转角度
                            _destination = Manager.Instance.Vector3Parse(_d.Position);
                            _rotation = Manager.Instance.Vector3Parse(_d.Rotation);
                            //禁用导航组件，否则会影响到定点传送
                            PlayerMove.Instance.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                            //设置用户的位置及旋转角度
                            PlayerMove.Instance.gameObject.transform.position = _destination;
                            PlayerMove.Instance.gameObject.transform.eulerAngles = _rotation;
                            //将用户传送到指定地点后，直接进入下一步
                            NextStep();
                            break;
                        case "click":
                            //当提示信息不为空时
                            if (!(_d.TextRes.Equals("null")))
                            {
                                //初始化提示巡检练习的信息
                                UIManager.Instance._CheckPracticeUI.InitHint();
                                string s = _d.TextRes;
                                UIManager.Instance._CheckPracticeUI.hintUI.SetUI(s);
                                //加载相应的音频并播放
                                AudioPlay.Instance.audioClip = Resources.Load<AudioClip>(_d.SoundRes);
                                AudioPlay.Instance.PlayAudio();
                            }
                            _moveObj = _s[1];
                            //找到需要高亮的物体
                            GameObject _obj = GameObject.Find(_moveObj);
                            if (_obj)
                            {
                                //禁用所有的碰撞体，否则会影响到点击
                                RestModelCollider();
                                //物体高亮
                                _obj.AddComponent<URPHighlightableObject>();
                                //对应步骤启用物体的碰撞体
                                _obj.GetComponent<BoxCollider>().enabled = true;
                            }
                            break;
                        case "anim":
                            string[] _ss = _s[1].Split('|');
                            GameObject _o = GameObject.Find(_ss[0]);
                            Animation _anim = _o.GetComponent<Animation>();
                            _anim.playAutomatically = false;
                            AnimPlay ap = _o.GetComponent<AnimPlay>();
                            ap.Init(_anim, _ss[1]);
                            break;
                    }
                }
                else
                {
                    string s = _d.Operate;
                    switch (s)
                    {
                        //听主变声音
                        case "hear":
                            if (!(_d.TextRes.Equals("null")))
                            {
                                //初始化提示巡检练习的信息
                                UIManager.Instance._CheckPracticeUI.InitHint();
                                string ts = _d.TextRes;
                                UIManager.Instance._CheckPracticeUI.hintUI.SetUI(ts);
                                //加载相应的音频并播放
                                AudioPlay.Instance.audioClip = Resources.Load<AudioClip>(_d.SoundRes);
                                AudioPlay.Instance.PlayAudio();
                                //同时获取主变的声音并播放
                                UIManager.Instance._CheckPracticeUI.ZhubianVoice.Play();
                            }
                            //记录当主变声音播放完成的协程，播放完成后进入下一步
                            stopZ = Instance.StartCoroutine(Instance.WaitZhubianPlayOver(NextStep));
                            break;
                        default:
                            //其他步骤主变声音都不播放
                            UIManager.Instance._CheckPracticeUI.ZhubianVoice.Stop();
                            Instance.StopCoroutine(stopZ);
                            break;
                    }
                }
            }

            /// <summary>
            /// 下一步
            /// </summary>
            internal void NextStep()
            {
                //停止播放上一步的音频
                AudioPlay.Instance.StopAudio();
                if (Tmpstep < UIManager.Instance.checkPracticeData.Count - 1)
                {
                    Tmpstep++;
                    //继续执行步骤
                    ExOperate();
                }
                else
                {
                    //隐藏巡视练习提示信息ui
                    UIManager.Instance._CheckPracticeUI.hintUI.Rest();
                    //显示巡视练习每一个阶段完成的下一阶段提示ui
                    UIManager.Instance._CheckPracticeUI.nextUI.Init();
                }
            }


            /// <summary>
            /// 替换下一阶段的提示ui背景图
            /// </summary>
            internal void AnalyzeNextPracModel()
            {
                CheckPracticeTable _d = UIManager.Instance.checkPracticeData[Tmpstep];
                if (!(_d.SpriteRes.Equals("null")))
                {
                    UIManager.Instance._CheckPracticeUI.nextUI.Bg.GetComponent<Image>().sprite = Resources.Load<Sprite>(_d.SpriteRes);
                }
            }

            /// <summary>
            /// 禁用所有的碰撞体
            /// </summary>
            internal void RestModelCollider()
            {
                GameObject master = GameObject.Find("Models");
                BoxCollider[] _bcs = master.GetComponentsInChildren<BoxCollider>();
                foreach (BoxCollider bc in _bcs)
                {
                    bc.enabled = false;
                }
            }
        }
        /// <summary>
        /// 倒闸操作步骤功能处理
        /// </summary>
        public class Scene2
        {
            internal int Tmpstep; //步骤
            private Vector3 _destination; //移动目的地
            internal GameObject clickobj;
            internal List<TmpAnimObj> tmpAnimObj = new List<TmpAnimObj>();
            private bool practiceOver;
            internal int togID;
            internal bool isStop;
            internal bool isClkSound;
            internal Transform exAnimCamera;

            /// <summary>
            /// 初始化
            /// </summary>
            internal void InitStart()
            {
                Instance.RestModelCollider();  //初始化模型碰撞体
                InitAniObj();
                exAnimCamera = GameObject.Find("ExAnimCamera").GetComponent<Transform>();
                Tmpstep = 1;
                togID = 0;
                practiceOver = false;  //练习是否结束
                isStop = true;   //声音是否结束
                isClkSound = false; //点击模型是否需要发出声音
                IsPractice(Manager.Instance.AllDzSteps[Tmpstep]);
                ExOperate();
            }

            /// <summary>
            /// 判断是否是练习模式，且当前步骤是否需要被跳过
            /// </summary>
            /// <param name="_d"></param>
            internal void IsPractice(DaoZhaTable _d)
            {
                if (Instance._EventSystemType == EventSystemType.Practice)
                {
                    if (int.Parse(_d.Practice) == 0)
                    {
                        Tmpstep++;
                        //设置任务栏
                        UIManager.Instance._Title.task.SetTaskList(Tmpstep, ref togID);
                        //练习结束返回主标题
                        if (Tmpstep > Manager.Instance.AllDzSteps.Count)
                        {
                            practiceOver = true;
                            UIManager.Instance.Back();
                        }
                        else  //练习未结束，继续判断当前步骤是否需要跳过
                        {
                            IsPractice(Manager.Instance.AllDzSteps[Tmpstep]);
                        }
                    }
                }
            }

            /// <summary>
            /// 初始化需要激活操作的模型，先全部隐藏
            /// </summary>
            internal void InitAniObj()
            {
                foreach (string _s in Manager.Instance.daozhaAni.Values)
                {
                    string[] _ss = _s.Split('|');
                    GameObject _o = GameObject.Find(_ss[0]);
                    if (_o)
                        _o.SetActive(false);
                }
            }

            /// <summary>
            /// 执行操作
            /// </summary>
            internal void ExOperate()
            {
                DaoZhaTable _d = Manager.Instance.AllDzSteps[Tmpstep];
                JudgeShowAnim(_d);
                if (_d.Operate.Contains(":"))
                {
                    string[] _s = _d.Operate.Split(':');
                    switch (_s[0])
                    {
                        case "ui":
                            ShowUI(_s[1]);
                            break;
                        case "move":
                            //显示引导提示
                            UIManager.Instance._GuideTips.SetActive(true);
                            //显示引导线
                            PlayerMove.Instance.ArrowP.gameObject.SetActive(true);
                            Move(_s[1], _d);
                            break;
                        case "commui":
                            if (_s[1].Contains("|"))
                            {
                                string[] _ss = _s[1].Split('|');
                                UIManager.Instance._DaoZhaUI.queRenUI.Init(_ss);
                                UIManager.Instance._DaoZhaUI.uiobj.SetActive(true);
                            }
                            break;
                        case "audio":
                            SetAudio(_s[1]);
                            break;
                        case "btn":
                            SetBtn(_s[1]);
                            break;
                        case "photo":
                            SetPhoto(_s[1]);
                            break;
                        case "sound":
                            Communicate(_s[1], _d);
                            break;
                        case "click":
                            ClickObj(_s[1], _d);
                            break;
                        case "toolintro":
                            ShowToolIntro(_s[1]);
                            break;
                        case "popup":
                            Popup(_s[1], _d);
                            break;
                        case "active":
                            ActiveObj(_s[1]);
                            break;
                        case "anim":
                            AnimPlay(_s[1]);
                            break;
                        case "tip":
                            ShowTip(_s[1], _d);
                            break;
                    }
                }
            }

            /// <summary>
            /// 判断是否要显示动画执行显示窗口
            /// </summary>
            /// <param name="_d"></param>
            internal void JudgeShowAnim(DaoZhaTable _d)
            {
                if (_d.ShowAnim.Equals("null"))
                {
                    return;
                }
                if (_d.ShowAnim.Contains("|"))
                {
                    string[] _ds = _d.ShowAnim.Split('|');
                    foreach(string _s in _ds)
                    {
                        if (_s.Equals(""))
                        {
                            break;
                        }
                        if (_s.Contains(":"))
                        {
                            string[] _dss = _s.Split(':');
                            switch (_dss[0])
                            {
                                case "show":
                                    bool isShow = bool.Parse(_dss[1]);
                                    UIManager.Instance._ExAnimWin.isShow = isShow;
                                    break;
                                case "pos":
                                    Vector3 pos = Manager.Instance.Vector3Parse(_dss[1]);
                                    exAnimCamera.localPosition = pos;
                                    break;
                                case "rot":
                                    Vector3 rot = Manager.Instance.Vector3Parse(_dss[1]);
                                    exAnimCamera.localEulerAngles = rot;
                                    break;
                                case "clkhelp":
                                    bool isAllow = bool.Parse(_dss[1]);
                                    ClickHelp.Instance.isAllow = isAllow;
                                    break;
                            }
                        }
                    }
                    UIManager.Instance._ExAnimWin.ShowWin();
                }
            }

            /// <summary>
            /// 下一步
            /// </summary>
            internal void NextStep()
            {
                PlayerMove.Instance.ResetCollider();
                //是否需要停止播放
                if (isStop)
                {
                    AudioPlay.Instance.StopAudio();
                }
                //是否需要进行透明设置
                if (UIManager.Instance._DaoZhaUI.IsAlpha)
                    UIManager.Instance._DaoZhaUI.SetAlpha();
                if (Tmpstep < Manager.Instance.AllDzSteps.Count)
                {
                    Tmpstep++;
                    UIManager.Instance._Title.task.SetTaskList(Tmpstep, ref togID);
                    IsPractice(Manager.Instance.AllDzSteps[Tmpstep]);
                    if (!practiceOver)
                        ExOperate();
                }
                else
                {
                    UIManager.Instance.Back();
                }
            }

            /// <summary>
            /// 显示介绍UI
            /// </summary>
            /// <param name="_s"></param>
            private void ShowUI(string _s)
            {
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    //当有4个参数时，表示要先把对话相关UI关闭
                    if (_ss.Length > 3 && _ss[3].Equals("false"))
                    {
                        UIManager.Instance._DaoZhaUI.RestUI();
                        UIManager.Instance._DaoZhaUI.uiobj.SetActive(false);
                    }
                    Instance.GetSubBtn(_ss[0]);
                    UIManager.Instance._IntroUI.InitStart(_ss[1], _ss[2]);
                }
            }

            /// <summary>
            /// 人物移动到某地的设置
            /// </summary>
            /// <param name="_s">目的地坐标</param>
            /// <param name="_d"></param>
            private void Move(string _s, DaoZhaTable _d)
            {
                //字符串中有两个参数及以上，表示移动到位置后要传送或者关闭对话UI
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    _destination = Manager.Instance.Vector3Parse(_ss[0]);
                    //第2个参数是portal表示要传送，是false表示要关闭对话UI
                    if (_ss[1].Equals("portal"))
                    {
                        //查看第4个参数是否为false，若是则表示要关闭对话UI
                        if (_ss.Length > 3 && _ss[3].Equals("false"))
                        {
                            UIManager.Instance._DaoZhaUI.RestUI();
                            //UIManager.Instance._DaoZhaUI.uiobj.SetActive(false);
                        }
                        //允许人物传送
                        PlayerMove.Instance.IsPortal = true;
                        //当前人物所在移动区域编号
                        PlayerMove.Instance.area = int.Parse(_d.Area);
                        //传送区域设置
                        PlayerMove.Instance.targetArea = int.Parse(_ss[2]);
                    }
                    else if (_ss[1].Equals("false"))
                    {
                        UIManager.Instance._DaoZhaUI.RestUI();
                        //UIManager.Instance._DaoZhaUI.uiobj.SetActive(false);
                    }
                }
                else
                {
                    _destination = Manager.Instance.Vector3Parse(_s);
                }
                //添加LineRenderer并设置参数
                //PlayerMove.Instance.line = PlayerMove.Instance.transform.gameObject.AddComponent<LineRenderer>();
                //PlayerMove.Instance.SetLine();
                //允许人物移动
                PlayerMove.Instance.IsMove = true;
                //设置导航目标地
                PlayerMove.Instance.navMeshAgent.destination = _destination;
                //存储当前任务栏的显示状态
                PlayerMove.Instance.togOn = UIManager.Instance._Title.toggles[1].isOn;
            }

            /// <summary>
            /// 播放声音
            /// </summary>
            /// <param name="_s"></param>
            private void SetAudio(string _s)
            {
                if (_s.Equals("loop"))
                {
                    AudioPlay.Instance.audioClip = Manager.Instance.DzAudio[Tmpstep];
                    AudioPlay.Instance.audioSource.loop = true;  //循环播放
                    isStop = false;  //等待有模型点击后再关闭声音
                    AudioPlay.Instance.PlayAudio();
                    NextStep();
                }
            }

            /// <summary>
            /// 右下角按钮的激活
            /// </summary>
            /// <param name="_s"></param>
            private void SetBtn(string _s)
            {
                //当对话开始时，确保UI父物体被激活
                if (!UIManager.Instance._DaoZhaUI.uiobj.activeSelf)
                {
                    UIManager.Instance._DaoZhaUI.uiobj.SetActive(true);
                }
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    UIManager.Instance._DaoZhaUI.SetBtn(_ss[0], _ss[1]);
                    UIManager.Instance._DaoZhaUI.dianHuaUI.master.SetActive(true);
                    //关闭回复对话的任意键
                    if (_ss.Length > 2 && _ss[2].Equals("false"))
                    {
                        bool active = bool.Parse(_ss[2]);
                        UIManager.Instance._DaoZhaUI.huiFuUI.anykey.SetActive(active);
                    }
                }
            }

            /// <summary>
            /// 设置电话头像图标
            /// </summary>
            /// <param name="_s"></param>
            private void SetPhoto(string _s)
            {
                UIManager.Instance._DaoZhaUI.dianHuaUI.SetIcon(_s);
                NextStep();
            }

            /// <summary>
            /// 对话信息显示
            /// </summary>
            /// <param name="_s"></param>
            /// <param name="_d"></param>
            private void Communicate(string _s, DaoZhaTable _d)
            {
                //当对话开始时，确保UI父物体被激活
                if (!UIManager.Instance._DaoZhaUI.uiobj.activeSelf)
                {
                    UIManager.Instance._DaoZhaUI.uiobj.SetActive(true);
                }
                AudioPlay.Instance.audioClip = Manager.Instance.DzAudio[Tmpstep];
                AudioPlay.Instance.PlayAudio();
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    int speaker = int.Parse(_ss[0]);
                    UIManager.Instance._DaoZhaUI.ActiveArea = speaker;
                    if (speaker == 1)
                    {
                        UIManager.Instance._DaoZhaUI.dianHuaUI.SetUI(_d.TextData);
                        //参数多于2的时候，需要显示按钮
                        if (_ss.Length > 2)
                        {
                            UIManager.Instance._DaoZhaUI.SetBtn(_ss[1], _ss[2]);
                            //UIManager.Instance._DaoZhaUI.IsAlpha = true;
                        }
                        if (_ss[1].Equals("next"))
                        {
                            isStop = false;
                            NextStep();
                            //AudioPlay.Instance.WaitAudio();
                        }
                    }
                    else if (speaker == 2)
                    {
                        UIManager.Instance._DaoZhaUI.btn.SetActive(false);
                        //确保该区域是激活状态
                        if (!UIManager.Instance._DaoZhaUI.huiFuUI.master.activeSelf)
                            UIManager.Instance._DaoZhaUI.huiFuUI.master.SetActive(true);
                        //回复内容区域显示信息
                        UIManager.Instance._DaoZhaUI.huiFuUI.SetUI(_ss[1], _d.TextData);
                        //点击提示图标显示
                        UIManager.Instance._DaoZhaUI.clickTag.SetActive(true);
                        if (_ss.Length > 2 && _ss[2].Equals("next"))
                        {
                            UIManager.Instance._DaoZhaUI.clickTag.SetActive(false);
                            UIManager.Instance._DaoZhaUI.huiFuUI.anykey.GetComponent<ControlBtn>().enabled = false;
                            isStop = false;
                            NextStep();
                            //AudioPlay.Instance.WaitAudio();
                        }
                    }
                }
                else
                {
                    int speaker = int.Parse(_s);
                    UIManager.Instance._DaoZhaUI.ActiveArea = speaker;
                    if (speaker == 1)
                    {
                        //设置任意点击区域
                        GameObject anykey = UIManager.Instance._DaoZhaUI.dianHuaUI.anyKey;
                        anykey.SetActive(true);
                        ControlBtn cb = anykey.GetComponent<ControlBtn>();
                        cb.btnType = BtnType.AnyArea;
                        UIManager.Instance._DaoZhaUI.dianHuaUI.SetUI(_d.TextData);
                    }
                }
            }

            /// <summary>
            /// 点击物体
            /// </summary>
            /// <param name="_s"></param>
            /// <param name="_d"></param>
            private void ClickObj(string _s, DaoZhaTable _d)
            {
                if (UIManager.Instance._DaoZhaUI.huiFuUI.master.activeSelf)
                    UIManager.Instance._DaoZhaUI.huiFuUI.master.SetActive(false);
                clickobj = GameObject.Find(Manager.Instance.clickObj[Tmpstep]);
                if (!_d.SoundRes.Equals("null"))
                {
                    isClkSound = true;
                }
                if (clickobj)
                {
                    if (clickobj.GetComponent<BoxCollider>())
                    {
                        clickobj.GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        clickobj.AddComponent<BoxCollider>();
                    }
                    URPHighlightableObject hc = clickobj.AddComponent<URPHighlightableObject>();
                }
            }

            /// <summary>
            /// 显示工具介绍信息
            /// </summary>
            /// <param name="_s"></param>
            private void ShowToolIntro(string _s)
            {
                UIManager.Instance._DaoZhaUI.toolIntro.Init(_s);
            }

            /// <summary>
            /// 弹窗处理
            /// </summary>
            /// <param name="_s"></param>
            /// <param name="_d"></param>
            private void Popup(string _s, DaoZhaTable _d)
            {
                if (!UIManager.Instance._DaoZhaUI.popUpUI.master.activeSelf)
                {
                    UIManager.Instance._DaoZhaUI.popUpUI.master.SetActive(true);
                }
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    switch (_ss[0])
                    {
                        case "init":
                            UIManager.Instance._DaoZhaUI.popUpUI.Init(_ss[1], _ss[2], _ss[3]);
                            NextStep();
                            break;
                        case "click":
                            if (_ss.Length > 2 && _ss[2].Equals("false"))
                            {
                                bool active = bool.Parse(_ss[2]);
                                UIManager.Instance._DaoZhaUI.dianHuaUI.anyKey.SetActive(active);
                            }
                            UIManager.Instance._DaoZhaUI.popUpUI.ActiveBtn(_ss[1]);
                            UIManager.Instance._DaoZhaUI.popUpUI.SetBtn();
                            break;
                        case "close":
                            UIManager.Instance._DaoZhaUI.popUpUI.ClosePop(_ss[1]);
                            if (_ss.Length > 2 && _ss[2].Equals("false"))
                            {
                                UIManager.Instance._DaoZhaUI.RestUI();
                                UIManager.Instance._DaoZhaUI.uiobj.SetActive(false);
                            }
                            NextStep();
                            break;
                    }
                }
            }

            /// <summary>
            /// 激活或隐藏操作工具并初设工具位置
            /// </summary>
            /// <param name="_s"></param>
            private void ActiveObj(string _s)
            {
                string[] _ss = _s.Split('|');
                GameObject _o = GameObject.Find(_ss[0]);
                bool act = bool.Parse(_ss[2]);
                if (act)
                {
                    _o.transform.position = Manager.Instance.Vector3Parse(_ss[1]);
                }
                _o.SetActive(act);
                NextStep();
            }

            /// <summary>
            /// 播放动画
            /// </summary>
            /// <param name="_s"></param>
            private void AnimPlay(string _s)
            {
                string[] _ss = _s.Split('|');
                GameObject _o = GameObject.Find(_ss[0]);
                //存储需要进行记录初始位置的动画
                if (_ss.Length > 2 && _ss[2].Equals("record"))
                {
                    TmpAnimObj rec = new TmpAnimObj();
                    rec.SetValue(_o);
                    tmpAnimObj.Add(rec);
                }
                Animation _anim = _o.GetComponent<Animation>();
                _anim.playAutomatically = false; //关闭自动播放
                AnimPlay ap = _o.GetComponent<AnimPlay>();
                ap.Init(_anim, _ss[1]);
            }

            /// <summary>
            /// 显示每一大步结束后的提示信息
            /// </summary>
            /// <param name="_s"></param>
            private void ShowTip(string _s, DaoZhaTable _d)
            {
                UIManager.Instance._DaoZhaUI.uiobj.SetActive(false);
                string[] _ss = _s.Split('|');
                UIManager.Instance._DzTipUI.Init(_ss[0], _ss[1], _d.Area);
            }
        }

        #endregion

        #region 字段
        public static GameUnitManager Instance;
        public SceneType _SceneType = SceneType.None;
        public EventSystemType _EventSystemType = EventSystemType.None;
        internal List<SubList> SubBtn = new List<SubList>();
        internal bool isGuide = true;   //是否引导标记
        internal bool isFirst = true;   //是否是初次引导标记

        public Scene1 _Scene1 = new Scene1();
        public Scene2 _Scene2 = new Scene2();
        public ScenePractice _ScenePractice = new ScenePractice();

        private AudioClip[] AllAudioClip;
        public AudioSource _AudioSource = new AudioSource();
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        #region 功能实现
        /// <summary>
        /// 场景加载完成后，选择场景单元处理
        /// </summary>
        internal void SelectUnit()
        {
            PlayerMove.Instance.Init(); //人物参数初始化
            switch (_SceneType)
            {
                case SceneType.xunshi:
                    switch (_EventSystemType)
                    {
                        case EventSystemType.Study:
                            if (isGuide && isFirst)
                            {
                                //初始化引导ui
                                UIManager.Instance._GuideUI.Init();
                                isGuide = false;
                            }
                            else
                            {
                                _Scene1.InitStart();
                            }
                            break;
                        case EventSystemType.Practice:
                            //if (isGuide)
                            //{
                            //    UIManager.Instance._GuideUI.Init();
                            //    isGuide = false;
                            //}
                            //else
                            //{
                            //    _ScenePractice.InitStart();
                            //}
                            //关闭引导界面，否则会出现跳步的问题
                            isGuide = false;
                            //初始化上方的Title ui
                            UIManager.Instance._Title.InitStart();
                            //禁用任务栏
                            UIManager.Instance._Title.toggles[1].interactable = false;
                            //禁用引导按钮
                            UIManager.Instance._Title.toggles[2].interactable = false;
                            _ScenePractice.InitStart();
                            break;
                        case EventSystemType.Exam:
                            UIManager.Instance.SetSelectUI("exam");
                            break;
                    }
                    break;
                case SceneType.daozha:
                    switch (_EventSystemType)
                    {
                        case EventSystemType.Study:
                            if (isGuide && isFirst)
                            {
                                UIManager.Instance._GuideUI.Init();
                                isGuide = false;
                            }
                            else
                            {
                                _Scene2.InitStart();
                            }
                            break;
                        case EventSystemType.Practice:
                            isGuide = false;
                            //初始化上方的Title ui
                            UIManager.Instance._Title.InitStart();
                            //禁用引导按钮
                            UIManager.Instance._Title.toggles[2].interactable = false;
                            _Scene2.InitStart();
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 获取对应的SubList数据
        /// </summary>
        /// <param name="_s"></param>
        public void GetSubBtn(string _s)
        {
            Instance.SubBtn.Clear();
            foreach (SubList _d in Manager.Instance.AllSubList.Values)
            {
                if (_d.SubType.Equals(_s))
                    Instance.SubBtn.Add(_d);
            }
        }

        /// <summary>
        /// 了解完毕按钮
        /// </summary>
        public void OverBtn()
        {
            UIManager.Instance._IntroUI.uiobj.SetActive(false);
            UIManager.Instance._StartTipsUI.SetActive(false);
            UIManager.Instance._IntroUI.pm.IsRotationControl = true;
            Next();
        }

        /// <summary>
        /// 下一步
        /// </summary>
        internal void Next()
        {
            switch (_SceneType)
            {
                case SceneType.xunshi:
                    switch (_EventSystemType)
                    {
                        case EventSystemType.Study:
                            _Scene1.NextStep();
                            break;
                        case EventSystemType.Practice:
                            _ScenePractice.NextStep();
                            break;
                    }
                    break;
                case SceneType.daozha:
                    _Scene2.NextStep();
                    break;
            }
        }

        /// <summary>
        /// 禁用所有的碰撞体
        /// </summary>
        internal void RestModelCollider()
        {
            GameObject master = GameObject.Find("Models/Model/changjing/thefirst");
            BoxCollider[] _bcs = master.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider bc in _bcs)
            {
                bc.enabled = false;
            }
        }

        /// <summary>
        /// 判断主变声音是否播放完毕
        /// </summary>
        internal void WaitZhubianVoice()
        {
            StartCoroutine(WaitZhubianPlayOver(Instance.Next));
        }

        IEnumerator WaitZhubianPlayOver(UnityAction unityAction = null)
        {
            while(UIManager.Instance._CheckPracticeUI.ZhubianVoice.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }
            if (unityAction != null)
            {
                unityAction.Invoke();
            }
        }

        #endregion
    }
}