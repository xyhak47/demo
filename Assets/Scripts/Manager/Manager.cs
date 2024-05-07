using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XD.GameStatic;
using XD.UI;

namespace XD.TheManager
{
    public class Manager : Singleton<Manager>
    {
        #region �ֶ�
        internal SortedList<int, BtnMenu> AllBtn = new SortedList<int, BtnMenu>();
        internal SortedList<int, SubList> AllSubList = new SortedList<int, SubList>();
        internal SortedList<int, Contents> AllContents = new SortedList<int, Contents>();
        internal SortedList<int, ModelTable> AllModels = new SortedList<int, ModelTable>();
        internal SortedList<int, CheckGuideTable> AllChgSteps = new SortedList<int, CheckGuideTable>();
        internal SortedList<int, DaoZhaTable> AllDzSteps = new SortedList<int, DaoZhaTable>();
        internal SortedList<int, CheckPracticeTable> AllCkPData = new SortedList<int, CheckPracticeTable>();
        internal SortedList<int, LightTable> AllLightData = new SortedList<int, LightTable>();
        internal SortedList<int, TaskList> AllTaskList = new SortedList<int, TaskList>();
        //��������Ѳ�Ӽ���բ��
        internal List<TaskList> XunshiTasks = new List<TaskList>();
        internal List<TaskList> DaozhaTasks = new List<TaskList>();
        //Ѳ�ӿ�����Ŀ����ѡ����ѡ���жϣ�
        internal List<QuestionItemDatas> XsSingleTopic = new List<QuestionItemDatas>();
        internal List<QuestionItemDatas> XsDoubleTopic = new List<QuestionItemDatas>();
        internal List<QuestionItemDatas> XsJudgeTopic = new List<QuestionItemDatas>();
        //��բ������Ŀ����ѡ����ѡ���жϣ�
        internal List<QuestionItemDatas> DzSingleTopic = new List<QuestionItemDatas>();
        internal List<QuestionItemDatas> DzDoubleTopic = new List<QuestionItemDatas>();
        internal List<QuestionItemDatas> DzJudgeTopic = new List<QuestionItemDatas>();
        //�����л�����
        private string TmpLoadScene;
        //��բ�е���Ƶ��Դ
        internal SortedList<int, AudioClip> DzAudio = new SortedList<int, AudioClip>();
        //��բ������ɺ����ʾ
        internal SortedList<int, DzStepTip> dzTips = new SortedList<int, DzStepTip>();
        //��բ����Ҫ���������
        internal SortedList<int, string> clickObj = new SortedList<int, string>();
        //��բ����Ҫ����/��ʾ������
        internal SortedList<int, string> daozhaAni = new SortedList<int, string>();
        internal Dictionary<int, UIChild> _uiChild = new Dictionary<int, UIChild>();
        internal AudioClip[] allAudioClips;

        /// <summary>
        /// 20221117����
        /// </summary>
        internal SortedList<int, FaultMod> faultMod = new SortedList<int, FaultMod>();
        internal Dictionary<string, Vector3[]> lookAtClick = new Dictionary<string, Vector3[]>();
        #endregion

        #region ��ʼ��
        private void Start()
        {
            StartSub();
        }

        /// <summary>
        /// ����json���ݱ���ʼ��ui������Login����
        /// </summary>
        internal void StartSub()
        {
            LoadBtnMenu();
            LoadAllSubList();
            LoadAllContents();
            LoadAllModel();
            LoadAllCheckGuide();
            LoadAllDaoZha();
            LoadDzTips();
            LoadAllCheckPracticeData();
            LoadAllLightData();
            LoadAllTaskList();
            LoadAllXunShiExam();
            LoadAllDaoZhaExam();
            LoadAllFaultMod();
            LoadAllLookAtClick();
            UIManager.Instance.InitStart();
            UIManager.Instance._ExamUI._examChoiceUI.isFirst = true;
            SpLoadScene("Login");
        }
        #endregion

        #region ���ݱ���
        private void LoadBtnMenu()
        {
            GameJsonManager.Instance.LoadJsonData<BtnMenu>("db/BtnMenu", (List<BtnMenu> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (BtnMenu _d in ds)
                    {
                        AllBtn.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllSubList()
        {
            GameJsonManager.Instance.LoadJsonData<SubList>("db/SubList", (List<SubList> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (SubList _d in ds)
                    {
                        AllSubList.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllContents()
        {
            GameJsonManager.Instance.LoadJsonData<Contents>("db/Contents", (List<Contents> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (Contents _d in ds)
                    {
                        AllContents.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllModel()
        {
            GameJsonManager.Instance.LoadJsonData<ModelTable>("db/ModelTable", (List<ModelTable> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (ModelTable _d in ds)
                    {
                        AllModels.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllCheckGuide()
        {
            GameJsonManager.Instance.LoadJsonData<CheckGuideTable>("db/CheckGuideTable", (List<CheckGuideTable> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (CheckGuideTable _d in ds)
                    {
                        AllChgSteps.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllDaoZha()
        {
            GameJsonManager.Instance.LoadJsonData<DaoZhaTable>("db/DaoZhaTable", (List<DaoZhaTable> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (DaoZhaTable _d in ds)
                    {
                        AllDzSteps.Add(_d.ID, _d);
                        LoadObj(_d);
                        LoadDzAudio(_d);
                    }
                }
            });
        }

        /// <summary>
        /// ����բ��������Ҫ�����ģ�͵Ĳ���·���Ȼ�ȡ����
        /// </summary>
        /// <param name="_d"></param>
        private void LoadObj(DaoZhaTable _d)
        {
            if (_d.Operate.Contains(":"))
            {
                string[] _s = _d.Operate.Split(':');
                if (_s[0].Equals("click"))
                {
                    //�����輰��Ӧ��Ҫ���ģ�͵�Ѱ��·���洢���б���
                    //�ں���ִ���в����Ǵ�1��ʼ����
                    clickObj.Add(_d.ID, _s[1]);
                }
                else if (_s[0].Equals("active"))
                {
                    daozhaAni.Add(_d.ID, _s[1]);
                }
            }
        }

        /// <summary>
        /// ���ص�բ��Ƶ
        /// </summary>
        /// <param name="_d"></param>
        private void LoadDzAudio(DaoZhaTable _d)
        {
            AudioClip audio = Resources.Load<AudioClip>(_d.SoundRes);
            DzAudio.Add(_d.ID, audio);
        }

        /// <summary>
        /// ���ص�բ������ʾ��Ϣ
        /// </summary>
        private void LoadDzTips()
        {
            GameJsonManager.Instance.LoadJsonData<DzStepTip>("db/DzStepTip", (List<DzStepTip> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (DzStepTip _d in ds)
                    {
                        dzTips.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllCheckPracticeData()
        {
            GameJsonManager.Instance.LoadJsonData<CheckPracticeTable>("db/CheckPracticeTable", (List<CheckPracticeTable> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (CheckPracticeTable _d in ds)
                    {
                        AllCkPData.Add(_d.ID, _d);
                    }
                }
            });
        }

        /// <summary>
        /// �����������ֵ���Ϣ
        /// </summary>
        private void LoadAllLightData()
        {
            GameJsonManager.Instance.LoadJsonData<LightTable>("db/LightTable", (List<LightTable> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (LightTable _d in ds)
                    {
                        AllLightData.Add(_d.ID, _d);
                    }
                }
            });
        }

        private void LoadAllTaskList()
        {
            GameJsonManager.Instance.LoadJsonData<TaskList>("db/TaskList", (List<TaskList> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (TaskList _d in ds)
                    {
                        //��Ѳ�Ӻ͵�բ��������벻ͬ��list��
                        switch (_d.Scene)
                        {
                            case "xunshi":
                                XunshiTasks.Add(_d);
                                break;
                            case "daozha":
                                DaozhaTasks.Add(_d);
                                break;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// ����Ѳ�ӿ��˵���Ŀ��Ϣ
        /// </summary>
        private void LoadAllXunShiExam()
        {
            GameJsonManager.Instance.LoadJsonData<XunShiExam>("db/XunShiExam", (List<XunShiExam> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (XunShiExam _d in ds)
                    {
                        QuestionItemDatas ep = new QuestionItemDatas(_d);
                        switch (_d.Type)
                        {
                            //��ѡ
                            case "1":
                                XsSingleTopic.Add(ep);
                                break;
                            //��ѡ
                            case "2":
                                XsDoubleTopic.Add(ep);
                                break;
                            //�ж�
                            case "3":
                                XsJudgeTopic.Add(ep);
                                break;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// ���ص�բ���˵���Ŀ��Ϣ
        /// </summary>
        private void LoadAllDaoZhaExam()
        {
            GameJsonManager.Instance.LoadJsonData<DaoZhaExam>("db/DaoZhaExam", (List<DaoZhaExam> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (DaoZhaExam _d in ds)
                    {
                        QuestionItemDatas ep = new QuestionItemDatas(_d);
                        switch (_d.Type)
                        {
                            //��ѡ
                            case "1":
                                DzSingleTopic.Add(ep);
                                break;
                            //��ѡ
                            case "2":
                                DzDoubleTopic.Add(ep);
                                break;
                            //�ж�
                            case "3":
                                DzJudgeTopic.Add(ep);
                                break;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 20221117����
        /// ���ع���ģ����Ϣ
        /// </summary>
        private void LoadAllFaultMod()
        {
            GameJsonManager.Instance.LoadJsonData<FaultMod>("db/FaultMod", (List<FaultMod> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (FaultMod _d in ds)
                    {
                        faultMod.Add(_d.ID, _d);
                    }
                }
            });
        }

        /// <summary>
        /// 20221123����
        /// ���ص����������鿴��ģ����Ϣ
        /// </summary>
        private void LoadAllLookAtClick()
        {
            GameJsonManager.Instance.LoadJsonData<LookAtClick>("db/LookAtClick", (List<LookAtClick> ds) =>
            {
                if (ds.Count > 0)
                {
                    foreach (LookAtClick _d in ds)
                    {
                        lookAtClick.Add(_d.ModelName, _d.GetLoction());
                    }
                }
            });
        }

        private void LoadAllGuideAudio()
        {
            allAudioClips = Resources.LoadAll<AudioClip>("AllAudioClip");
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        internal AudioClip GetAudioClip(int id)
        {
            for (int i = 0; i < allAudioClips.Length; i++)
            {
                //if (allAudioClips[i].name == CheckGuideData[id][6])
                //{
                //    return allAudioClips[i];
                //}
            }
            return null;
        }
        internal AudioClip GetAudioClip(string audioName)
        {
            for (int i = 0; i < allAudioClips.Length; i++)
            {
                if (allAudioClips[i].name == audioName)
                {
                    return allAudioClips[i];
                }
            }
            return null;
        }
        #endregion

        #region ���س���
        internal void SpLoadScene(string _s)
        {
            TmpLoadScene = _s;
            SceneLoadManager.Instance.StartLoadScene(_s);
        }

        internal void ExLoadLoadScene(EventSystemType _t, int main, string _da)
        {
            //if (SubLoadBool)
            //    return;
            //TmpLoadScene = _da;
            //if (main == 1000)
            //    _TmpSceneData = new TmpSceneData(_t, _da, 0, true);
            //else
            //    _TmpSceneData = new TmpSceneData(_t, _da, main);
            //SceneLoadManager.Instance.StartLoadScene(true, _da);
            //SubLoadBool = true;
        }

        /// <summary>
        /// ���ؽ���
        /// </summary>
        /// <param name="_bool"></param>
        internal void LoadSceneDone()
        {
            switch (TmpLoadScene.ToLower())
            {
                case "login":
                    UIManager.Instance.SetSelectUI("scene");
                    break;
                case "main":
                    UIManager.Instance.SetSelectUI("main");
                    GameUnitManager.Instance.SelectUnit();
                    break;
            }
        }

        #endregion

        private void Update()
        {
            //��Esc�˳�
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            //���³�ʼ����ui
            if (UIManager.Instance)
            {
                UIManager.Instance.UIUpdate();
            }
        }

        #region ���ݴ���
        internal GameObject InitResObj(Transform _t, string _s)
            {
                GameObject _obj = (GameObject)Instantiate(Resources.Load(_s), Vector3.zero, Quaternion.identity);
                if (_obj != null)
                {
                    if (_t != null)
                    {
                        _obj.transform.SetParent(_t, true);
                        _obj.transform.localPosition = Vector3.zero;
                        _obj.transform.localRotation = Quaternion.identity;
                        _obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    return _obj;
                }
                return null;
            }

            internal GameObject InitResObj(string _s, Vector3 _pos, Vector3 _rot)
            {
                GameObject _obj = (GameObject)Instantiate(Resources.Load(_s), _pos, Quaternion.Euler(_rot.x, _rot.y, _rot.z));

                if (_obj != null)
                {
                    _obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    return _obj;
                }
                return null;
            }

            internal GameObject InitCloneObj(Transform _t, GameObject _tobj)
            {
                GameObject _obj = (GameObject)Instantiate(_tobj, Vector3.zero, Quaternion.identity);
                if (_obj != null)
                {
                    _obj.transform.SetParent(_t, true);
                    _obj.transform.localPosition = Vector3.zero;
                    _obj.transform.localRotation = Quaternion.identity;
                    _obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    return _obj;
                }
                return null;
            }
            internal Vector3 Vector3Parse(string _v)
            {
                _v = _v.Replace("(", "").Replace(")", "");
                string[] _s = _v.Split(',');
                return new Vector3(float.Parse(_s[0]), float.Parse(_s[1]), float.Parse(_s[2]));
            }

            internal Color32 ColorParse(string _v)
            {
                _v = _v.Replace("(", "").Replace(")", "");
                string[] _s = _v.Split(',');
                return new Color(int.Parse(_s[0]), int.Parse(_s[1]), int.Parse(_s[2]), int.Parse(_s[3]));
            }

            internal Sprite ToSprite(Texture2D self)
            {
                var rect = new Rect(0, 0, self.width, self.height);
                var pivot = Vector2.one * 0.5f;
                var newSprite = Sprite.Create(self, rect, pivot);
                //Debug.LogWarning("ToSprite:" + self.name);
                return newSprite;
            }

            #endregion

        #region ���ܷ���

        /// <summary>
        /// ��Ӹ߹�
        /// </summary>
        //internal void AddHighlight()
        //{
        //    if (_EventSystemType == EventSystemType.Study)
        //    {
        //        ArrayList _objs = new ArrayList();
        //        foreach (ModelTable _d in AllModels.Values)
        //        {
        //            _objs.Add(_d.ModelObj);
        //            if (_objs.Count != 0)
        //            {
        //                for (int i = 0; i < _objs.Count; i++)
        //                {
        //                    GameObject _obj = GameObject.Find(_objs[i].ToString());
        //                    if (_obj)
        //                    {
        //                        _obj.AddComponent<HighlightController>().SetParameter(true, Convert.ToInt32(_d.ModelType), _d);
        //                        if (i == 0)
        //                        {
        //                            highlights.Add(_obj);   //�߹��Ӧ����
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Debug.LogWarning(highlights[i] + "������!");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //internal void SetHighlight()
        //{
        //    foreach (KeyValuePair<int, UIChild> item in _uiChild)
        //    {
        //        if (item.Key == GameUnitManager.Instance._Scene1.Objstep)
        //        {
        //            item.Value.SelfHighlight();
        //        }
        //    }
        //    if (_EventSystemType == EventSystemType.Study || _EventSystemType == EventSystemType.Practice)
        //    {
        //        //����߹� ���ϵ�ģʽ �����Զ��������
        //        for (int i = 0; i < highlights.Count; i++)
        //        {
        //            HighlightController temp = highlights[i].GetComponent<HighlightController>();
        //            //�ж������ƶ���ģ�͵�󣬶�Ӧģ����˸
        //            if (temp.id == GameUnitManager.Instance._Scene1.Objstep)
        //            {
        //                /*if (_EventSystemType == EventSystemType.Practice)
        //                {
        //                    PlayerMove.Instance.SetPosAndRot(temp.birthPoint.transform);            //��������λ��
        //                }*/
        //                temp.SelfTwinkle(true, .5f);    //������˸
        //                /*if (_EventSystemType == EventSystemType.Study)
        //                {
        //                    _audioSource.clip = GetAudioClip(_stepNum);
        //                }
        //                else if (_EventSystemType == EventSystemType.Practice)
        //                {
        //                    _audioSource.clip = GetAudioClip(_questionUI.choiceQuestions[_stepNum][4]);
        //                }
        //                _audioSource.Play();*/
        //                break;
        //            }
        //        }
        //    }
        //}
        internal string LayerToName(LayerMask layerMask)
        {
            return LayerMask.LayerToName((layerMask.value >> 6) + 5);
        }

        /// <summary>
        /// �������
        /// </summary>
        internal void ClearData(GameObject _o)
        {
            if (_o.GetComponent<URPHighlightableObject>())
            {
                Destroy(_o.GetComponent<URPHighlightableObject>());
            }
            //if(_o.GetComponent<BoxCollider>())
            //{
            //    Destroy(_o.GetComponent<BoxCollider>());
            //}
        }
        //internal void ClearData()
        //{
        //    //�߹���������
        //    if (highlights.Count != 0)
        //    {
        //        foreach (GameObject item in highlights)
        //        {
        //            if (item.GetComponent<HighlightController>())
        //            {
        //                Destroy(item.GetComponent<HighlightController>());
        //                //Destroy(item.GetComponent<HighlightChild>());
        //            }
        //        }
        //        highlights.Clear();
        //    }
        //}

        /// <summary>
        /// ��ż��
        /// </summary>
        /// <param name="n">ֵ</param>
        /// <returns>�Ƿ�������</returns>
        internal bool IsOddThird(int n)
        {
            return Convert.ToBoolean(n % 2);
            // ��:return n % 2 == 1;
        }

        /// <summary>
        /// ���뿼�����ݱ��е�ѡ������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal string Option(string id)
        {
            string option = null;
            switch (id)
            {
                case "0":
                    option = "A";
                    break;
                case "1":
                    option = "B";
                    break;
                case "2":
                    option = "C";
                    break;
                case "3":
                    option = "D";
                    break;
            }
            return option;
        }
        #endregion
    }
}
