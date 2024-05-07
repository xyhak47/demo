using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uTools;
using XD.GameStatic;
using XD.TheManager;

namespace XD.Tasks
{
    public class TaskItem : MonoBehaviour, uIPointHandler
    {
        internal int ID;
        public Toggle thisTog;
        public Image img;
        internal int stepStart;
        internal int stepEnd;
        internal string startArea;
        internal Sprite enterBefore;
        internal Sprite enterAfter;

        internal void Init(int id, string pic1, string pic2, string step)
        {
            ID = id;
            enterBefore = Resources.Load<Sprite>(pic1);
            enterAfter = Resources.Load<Sprite>(pic2);
            if (step.Contains(","))
            {
                string[] _s = step.Split(',');
                stepStart = int.Parse(_s[0]);
                stepEnd = int.Parse(_s[1]);
                if (_s.Length > 2)
                {
                    startArea = _s[2];
                }
            }
            if(ID == 0)
            {
                img.sprite = enterAfter;
            }
            else
            {
                img.sprite = enterBefore;
            }
            thisTog.isOn = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!PlayerMove.Instance.IsRotationControl)
            {
                PlayerMove.Instance.IsRotationControl = true;
            }
            switch (GameUnitManager.Instance._SceneType)
            {
                case SceneType.xunshi:
                    if(GameUnitManager.Instance._EventSystemType == EventSystemType.Study)
                    {
                        if (UIManager.Instance._IntroUI.uiobj.activeSelf)
                            UIManager.Instance._IntroUI.uiobj.SetActive(false);
                        if (UIManager.Instance._ModelUI.uiobj.activeSelf)
                        {
                            UIManager.Instance._ModelUI.uiobj.SetActive(false);
                            UIManager.Instance._ModelUI.ModelRawImg.SetActive(false);
                        }
                        if(GameUnitManager.Instance._Scene1._obj && GameUnitManager.Instance._Scene1._obj.GetComponent<URPHighlightableObject>())
                        {
                            GameUnitManager.Instance._Scene1._obj.GetComponent<URPHighlightableObject>().enabled = false;
                            Manager.Instance.ClearData(GameUnitManager.Instance._Scene1._obj);
                        }
                        UIManager.Instance._Title.task.TaskCancel(GameUnitManager.Instance._Scene1.togID);
                        GameUnitManager.Instance._Scene1.Tmpstep = stepStart;
                        GameUnitManager.Instance._Scene1.togID = ID;
                        UIManager.Instance._Title.task.TaskSelect(ID);
                        GameUnitManager.Instance._Scene1.ExOperate();
                    }
                    break;
                case SceneType.daozha:
                    UIManager.Instance.RestAllUI();
                    UIManager.Instance._Title.uiobj.SetActive(true);
                    if(GameUnitManager.Instance._Scene2.togID < UIManager.Instance._Title.task.taskTogList.Count)
                    {
                        UIManager.Instance._Title.task.TaskCancel(GameUnitManager.Instance._Scene2.togID);
                    }
                    //重置人物所在场景位置
                    if (!Manager.Instance.AllDzSteps[GameUnitManager.Instance._Scene2.Tmpstep].Area.Equals(startArea))
                    {
                        int area = int.Parse(startArea);
                        PlayerMove.Instance.ResetPosAndRot(area);
                    }
                    //重置动画
                    if (GameUnitManager.Instance._Scene2.tmpAnimObj.Count > 0)
                    {
                        List<TmpAnimObj> tmp = GameUnitManager.Instance._Scene2.tmpAnimObj;
                        foreach (TmpAnimObj tp in tmp)
                        {
                            tp.Rest();
                        }
                        GameUnitManager.Instance._Scene2.tmpAnimObj.Clear();
                    }
                    //重置点击物体上的高亮
                    if (GameUnitManager.Instance._Scene2.clickobj && GameUnitManager.Instance._Scene2.clickobj.GetComponent<URPHighlightableObject>())
                    {
                        GameUnitManager.Instance._Scene2.clickobj.GetComponent<URPHighlightableObject>().enabled = false;
                        Manager.Instance.ClearData(GameUnitManager.Instance._Scene2.clickobj);
                    }
                    GameUnitManager.Instance._Scene2.Tmpstep = stepStart;
                    GameUnitManager.Instance._Scene2.togID = ID;
                    UIManager.Instance._Title.task.TaskSelect(ID);
                    GameUnitManager.Instance._Scene2.ExOperate();
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
    }
}