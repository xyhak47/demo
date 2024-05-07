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
        #region ���л���
        /// <summary>
        /// ����Ѳ�Ӳ��蹦�ܴ���
        /// </summary>
        public class Scene1
        {
            internal int Tmpstep;   //CheckGuideTable�еĴ���
            private string _moveObj;  //����������������
            private Vector3 _destination; //�����ƶ���Ŀ�ĵ�

            internal string _modelType; //ģ�����
            internal GameObject _obj;  //��������������
            internal int togID; //�����б������ID

            /// <summary>
            /// ��ʼ��
            /// </summary>
            internal void InitStart()
            {
                Tmpstep = 1;
                togID = 0;
                ExOperate();
            }

            /// <summary>
            /// ִ�в���
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
                            //��ʼ����������ʾ
                            UIManager.Instance._StartTipsUI.SetActive(true);
                            if (_s[1].Contains(","))
                            {
                                string[] _ss = _s[1].Split(',');
                                Instance.GetSubBtn(_ss[0]);
                                //��ʾ��ʼ����ui
                                UIManager.Instance._IntroUI.InitStart(_ss[1], _ss[2]);
                            }
                            break;
                        case "move":
                            //��ʼ�������ֽ���
                            UIManager.Instance._StartTipsUI.SetActive(false);
                            _destination = Manager.Instance.Vector3Parse(_d.Position);
                            //���LineRenderer�����ò���
                            //PlayerMove.Instance.line = PlayerMove.Instance.transform.gameObject.AddComponent<LineRenderer>();
                            //PlayerMove.Instance.SetLine();
                            //��ʾ������ʾ
                            UIManager.Instance._GuideTips.SetActive(true);
                            //��ʾ������
                            PlayerMove.Instance.ArrowP.gameObject.SetActive(true);
                            //���������ƶ�
                            PlayerMove.Instance.IsMove = true;
                            //���õ���Ŀ���
                            PlayerMove.Instance.navMeshAgent.destination = _destination;
                            //���ø���ģ������
                            _moveObj = _s[1];
                            //�洢��ǰ����������ʾ״̬
                            PlayerMove.Instance.togOn = UIManager.Instance._Title.toggles[1].isOn;
                            break;
                        case "click":
                            if (_s[1].Contains(","))
                            {
                                string[] _ss = _s[1].Split(',');
                                _modelType = _ss[1];
                                //����RawImage��Ӧչʾ��3Dģ��
                                PlayerMove.Instance._ui3DModel.ActiveMaster(_ss[0]);
                            }
                            //�������������ҵ������������
                            _obj = GameObject.Find(_moveObj);
                            if (_obj)
                            {
                                //��������
                                _obj.AddComponent<URPHighlightableObject>();
                            }
                            break;
                    }
                }
            }

            /// <summary>
            /// ��һ��
            /// </summary>
            internal void NextStep()
            {
                if (Tmpstep < Manager.Instance.AllChgSteps.Count)
                {
                    Tmpstep++;
                    //�����������б�״̬
                    UIManager.Instance._Title.task.SetTaskList(Tmpstep, ref togID);
                    //����ִ�в���
                    ExOperate();
                }
                else
                {
                    //����ģ�ͽ���ui
                    UIManager.Instance._ModelUI.uiobj.SetActive(false);
                    //������������ü�outline��
                    //PlayerMove.Instance._param.URPCam.GetUniversalAdditionalCameraData().volumeLayerMask |= -(1 << 8);
                    //����������
                    UIManager.Instance.Back();
                }
            }

            /// <summary>
            /// ѧϰ�ڲ�ģ��ʱ��͸�����
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
            /// �ָ���ʾ���ص�ģ��
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
        /// ����Ѳ����ϰ
        /// </summary>
        public class ScenePractice
        {
            internal int Tmpstep; //����
            private string _moveObj; //�����������
            private Vector3 _destination; //�����ƶ���Ŀ�ĵ�
            private Vector3 _rotation; //������ת����
            internal int togID; //�����б������ID
            private Coroutine stopZ; //���ڴ�����Ƶֹͣ���ŵ�Э��

            /// <summary>
            /// ��ʼ��
            /// </summary>
            internal void InitStart()
            {
                Tmpstep = 0;
                ExOperate();
            }

            /// <summary>
            /// ִ�в���
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
                            //�����ݱ��л�ȡ�û���λ�ü���ת�Ƕ�
                            _destination = Manager.Instance.Vector3Parse(_d.Position);
                            _rotation = Manager.Instance.Vector3Parse(_d.Rotation);
                            //���õ�������������Ӱ�쵽���㴫��
                            PlayerMove.Instance.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                            //�����û���λ�ü���ת�Ƕ�
                            PlayerMove.Instance.gameObject.transform.position = _destination;
                            PlayerMove.Instance.gameObject.transform.eulerAngles = _rotation;
                            //���û����͵�ָ���ص��ֱ�ӽ�����һ��
                            NextStep();
                            break;
                        case "click":
                            //����ʾ��Ϣ��Ϊ��ʱ
                            if (!(_d.TextRes.Equals("null")))
                            {
                                //��ʼ����ʾѲ����ϰ����Ϣ
                                UIManager.Instance._CheckPracticeUI.InitHint();
                                string s = _d.TextRes;
                                UIManager.Instance._CheckPracticeUI.hintUI.SetUI(s);
                                //������Ӧ����Ƶ������
                                AudioPlay.Instance.audioClip = Resources.Load<AudioClip>(_d.SoundRes);
                                AudioPlay.Instance.PlayAudio();
                            }
                            _moveObj = _s[1];
                            //�ҵ���Ҫ����������
                            GameObject _obj = GameObject.Find(_moveObj);
                            if (_obj)
                            {
                                //�������е���ײ�壬�����Ӱ�쵽���
                                RestModelCollider();
                                //�������
                                _obj.AddComponent<URPHighlightableObject>();
                                //��Ӧ���������������ײ��
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
                        //����������
                        case "hear":
                            if (!(_d.TextRes.Equals("null")))
                            {
                                //��ʼ����ʾѲ����ϰ����Ϣ
                                UIManager.Instance._CheckPracticeUI.InitHint();
                                string ts = _d.TextRes;
                                UIManager.Instance._CheckPracticeUI.hintUI.SetUI(ts);
                                //������Ӧ����Ƶ������
                                AudioPlay.Instance.audioClip = Resources.Load<AudioClip>(_d.SoundRes);
                                AudioPlay.Instance.PlayAudio();
                                //ͬʱ��ȡ���������������
                                UIManager.Instance._CheckPracticeUI.ZhubianVoice.Play();
                            }
                            //��¼����������������ɵ�Э�̣�������ɺ������һ��
                            stopZ = Instance.StartCoroutine(Instance.WaitZhubianPlayOver(NextStep));
                            break;
                        default:
                            //������������������������
                            UIManager.Instance._CheckPracticeUI.ZhubianVoice.Stop();
                            Instance.StopCoroutine(stopZ);
                            break;
                    }
                }
            }

            /// <summary>
            /// ��һ��
            /// </summary>
            internal void NextStep()
            {
                //ֹͣ������һ������Ƶ
                AudioPlay.Instance.StopAudio();
                if (Tmpstep < UIManager.Instance.checkPracticeData.Count - 1)
                {
                    Tmpstep++;
                    //����ִ�в���
                    ExOperate();
                }
                else
                {
                    //����Ѳ����ϰ��ʾ��Ϣui
                    UIManager.Instance._CheckPracticeUI.hintUI.Rest();
                    //��ʾѲ����ϰÿһ���׶���ɵ���һ�׶���ʾui
                    UIManager.Instance._CheckPracticeUI.nextUI.Init();
                }
            }


            /// <summary>
            /// �滻��һ�׶ε���ʾui����ͼ
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
            /// �������е���ײ��
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
        /// ��բ�������蹦�ܴ���
        /// </summary>
        public class Scene2
        {
            internal int Tmpstep; //����
            private Vector3 _destination; //�ƶ�Ŀ�ĵ�
            internal GameObject clickobj;
            internal List<TmpAnimObj> tmpAnimObj = new List<TmpAnimObj>();
            private bool practiceOver;
            internal int togID;
            internal bool isStop;
            internal bool isClkSound;
            internal Transform exAnimCamera;

            /// <summary>
            /// ��ʼ��
            /// </summary>
            internal void InitStart()
            {
                Instance.RestModelCollider();  //��ʼ��ģ����ײ��
                InitAniObj();
                exAnimCamera = GameObject.Find("ExAnimCamera").GetComponent<Transform>();
                Tmpstep = 1;
                togID = 0;
                practiceOver = false;  //��ϰ�Ƿ����
                isStop = true;   //�����Ƿ����
                isClkSound = false; //���ģ���Ƿ���Ҫ��������
                IsPractice(Manager.Instance.AllDzSteps[Tmpstep]);
                ExOperate();
            }

            /// <summary>
            /// �ж��Ƿ�����ϰģʽ���ҵ�ǰ�����Ƿ���Ҫ������
            /// </summary>
            /// <param name="_d"></param>
            internal void IsPractice(DaoZhaTable _d)
            {
                if (Instance._EventSystemType == EventSystemType.Practice)
                {
                    if (int.Parse(_d.Practice) == 0)
                    {
                        Tmpstep++;
                        //����������
                        UIManager.Instance._Title.task.SetTaskList(Tmpstep, ref togID);
                        //��ϰ��������������
                        if (Tmpstep > Manager.Instance.AllDzSteps.Count)
                        {
                            practiceOver = true;
                            UIManager.Instance.Back();
                        }
                        else  //��ϰδ�����������жϵ�ǰ�����Ƿ���Ҫ����
                        {
                            IsPractice(Manager.Instance.AllDzSteps[Tmpstep]);
                        }
                    }
                }
            }

            /// <summary>
            /// ��ʼ����Ҫ���������ģ�ͣ���ȫ������
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
            /// ִ�в���
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
                            //��ʾ������ʾ
                            UIManager.Instance._GuideTips.SetActive(true);
                            //��ʾ������
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
            /// �ж��Ƿ�Ҫ��ʾ����ִ����ʾ����
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
            /// ��һ��
            /// </summary>
            internal void NextStep()
            {
                PlayerMove.Instance.ResetCollider();
                //�Ƿ���Ҫֹͣ����
                if (isStop)
                {
                    AudioPlay.Instance.StopAudio();
                }
                //�Ƿ���Ҫ����͸������
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
            /// ��ʾ����UI
            /// </summary>
            /// <param name="_s"></param>
            private void ShowUI(string _s)
            {
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    //����4������ʱ����ʾҪ�ȰѶԻ����UI�ر�
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
            /// �����ƶ���ĳ�ص�����
            /// </summary>
            /// <param name="_s">Ŀ�ĵ�����</param>
            /// <param name="_d"></param>
            private void Move(string _s, DaoZhaTable _d)
            {
                //�ַ��������������������ϣ���ʾ�ƶ���λ�ú�Ҫ���ͻ��߹رնԻ�UI
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    _destination = Manager.Instance.Vector3Parse(_ss[0]);
                    //��2��������portal��ʾҪ���ͣ���false��ʾҪ�رնԻ�UI
                    if (_ss[1].Equals("portal"))
                    {
                        //�鿴��4�������Ƿ�Ϊfalse���������ʾҪ�رնԻ�UI
                        if (_ss.Length > 3 && _ss[3].Equals("false"))
                        {
                            UIManager.Instance._DaoZhaUI.RestUI();
                            //UIManager.Instance._DaoZhaUI.uiobj.SetActive(false);
                        }
                        //�������ﴫ��
                        PlayerMove.Instance.IsPortal = true;
                        //��ǰ���������ƶ�������
                        PlayerMove.Instance.area = int.Parse(_d.Area);
                        //������������
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
                //���LineRenderer�����ò���
                //PlayerMove.Instance.line = PlayerMove.Instance.transform.gameObject.AddComponent<LineRenderer>();
                //PlayerMove.Instance.SetLine();
                //���������ƶ�
                PlayerMove.Instance.IsMove = true;
                //���õ���Ŀ���
                PlayerMove.Instance.navMeshAgent.destination = _destination;
                //�洢��ǰ����������ʾ״̬
                PlayerMove.Instance.togOn = UIManager.Instance._Title.toggles[1].isOn;
            }

            /// <summary>
            /// ��������
            /// </summary>
            /// <param name="_s"></param>
            private void SetAudio(string _s)
            {
                if (_s.Equals("loop"))
                {
                    AudioPlay.Instance.audioClip = Manager.Instance.DzAudio[Tmpstep];
                    AudioPlay.Instance.audioSource.loop = true;  //ѭ������
                    isStop = false;  //�ȴ���ģ�͵�����ٹر�����
                    AudioPlay.Instance.PlayAudio();
                    NextStep();
                }
            }

            /// <summary>
            /// ���½ǰ�ť�ļ���
            /// </summary>
            /// <param name="_s"></param>
            private void SetBtn(string _s)
            {
                //���Ի���ʼʱ��ȷ��UI�����屻����
                if (!UIManager.Instance._DaoZhaUI.uiobj.activeSelf)
                {
                    UIManager.Instance._DaoZhaUI.uiobj.SetActive(true);
                }
                if (_s.Contains("|"))
                {
                    string[] _ss = _s.Split('|');
                    UIManager.Instance._DaoZhaUI.SetBtn(_ss[0], _ss[1]);
                    UIManager.Instance._DaoZhaUI.dianHuaUI.master.SetActive(true);
                    //�رջظ��Ի��������
                    if (_ss.Length > 2 && _ss[2].Equals("false"))
                    {
                        bool active = bool.Parse(_ss[2]);
                        UIManager.Instance._DaoZhaUI.huiFuUI.anykey.SetActive(active);
                    }
                }
            }

            /// <summary>
            /// ���õ绰ͷ��ͼ��
            /// </summary>
            /// <param name="_s"></param>
            private void SetPhoto(string _s)
            {
                UIManager.Instance._DaoZhaUI.dianHuaUI.SetIcon(_s);
                NextStep();
            }

            /// <summary>
            /// �Ի���Ϣ��ʾ
            /// </summary>
            /// <param name="_s"></param>
            /// <param name="_d"></param>
            private void Communicate(string _s, DaoZhaTable _d)
            {
                //���Ի���ʼʱ��ȷ��UI�����屻����
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
                        //��������2��ʱ����Ҫ��ʾ��ť
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
                        //ȷ���������Ǽ���״̬
                        if (!UIManager.Instance._DaoZhaUI.huiFuUI.master.activeSelf)
                            UIManager.Instance._DaoZhaUI.huiFuUI.master.SetActive(true);
                        //�ظ�����������ʾ��Ϣ
                        UIManager.Instance._DaoZhaUI.huiFuUI.SetUI(_ss[1], _d.TextData);
                        //�����ʾͼ����ʾ
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
                        //��������������
                        GameObject anykey = UIManager.Instance._DaoZhaUI.dianHuaUI.anyKey;
                        anykey.SetActive(true);
                        ControlBtn cb = anykey.GetComponent<ControlBtn>();
                        cb.btnType = BtnType.AnyArea;
                        UIManager.Instance._DaoZhaUI.dianHuaUI.SetUI(_d.TextData);
                    }
                }
            }

            /// <summary>
            /// �������
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
            /// ��ʾ���߽�����Ϣ
            /// </summary>
            /// <param name="_s"></param>
            private void ShowToolIntro(string _s)
            {
                UIManager.Instance._DaoZhaUI.toolIntro.Init(_s);
            }

            /// <summary>
            /// ��������
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
            /// ��������ز������߲����蹤��λ��
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
            /// ���Ŷ���
            /// </summary>
            /// <param name="_s"></param>
            private void AnimPlay(string _s)
            {
                string[] _ss = _s.Split('|');
                GameObject _o = GameObject.Find(_ss[0]);
                //�洢��Ҫ���м�¼��ʼλ�õĶ���
                if (_ss.Length > 2 && _ss[2].Equals("record"))
                {
                    TmpAnimObj rec = new TmpAnimObj();
                    rec.SetValue(_o);
                    tmpAnimObj.Add(rec);
                }
                Animation _anim = _o.GetComponent<Animation>();
                _anim.playAutomatically = false; //�ر��Զ�����
                AnimPlay ap = _o.GetComponent<AnimPlay>();
                ap.Init(_anim, _ss[1]);
            }

            /// <summary>
            /// ��ʾÿһ�󲽽��������ʾ��Ϣ
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

        #region �ֶ�
        public static GameUnitManager Instance;
        public SceneType _SceneType = SceneType.None;
        public EventSystemType _EventSystemType = EventSystemType.None;
        internal List<SubList> SubBtn = new List<SubList>();
        internal bool isGuide = true;   //�Ƿ��������
        internal bool isFirst = true;   //�Ƿ��ǳ����������

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

        #region ����ʵ��
        /// <summary>
        /// ����������ɺ�ѡ�񳡾���Ԫ����
        /// </summary>
        internal void SelectUnit()
        {
            PlayerMove.Instance.Init(); //���������ʼ��
            switch (_SceneType)
            {
                case SceneType.xunshi:
                    switch (_EventSystemType)
                    {
                        case EventSystemType.Study:
                            if (isGuide && isFirst)
                            {
                                //��ʼ������ui
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
                            //�ر��������棬������������������
                            isGuide = false;
                            //��ʼ���Ϸ���Title ui
                            UIManager.Instance._Title.InitStart();
                            //����������
                            UIManager.Instance._Title.toggles[1].interactable = false;
                            //����������ť
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
                            //��ʼ���Ϸ���Title ui
                            UIManager.Instance._Title.InitStart();
                            //����������ť
                            UIManager.Instance._Title.toggles[2].interactable = false;
                            _Scene2.InitStart();
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ��SubList����
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
        /// �˽���ϰ�ť
        /// </summary>
        public void OverBtn()
        {
            UIManager.Instance._IntroUI.uiobj.SetActive(false);
            UIManager.Instance._StartTipsUI.SetActive(false);
            UIManager.Instance._IntroUI.pm.IsRotationControl = true;
            Next();
        }

        /// <summary>
        /// ��һ��
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
        /// �������е���ײ��
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
        /// �ж����������Ƿ񲥷����
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