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
        /// 初始化提示信息按钮的信息
        /// </summary>
        /// <param name="pic1">按钮按下前的图片路径</param>
        /// <param name="pic2">按钮按下后的图片路径</param>
        /// <param name="st">环节的初始步骤值</param>
        /// <param name="area">环节最后一步所在场景区域编号</param>
        internal void Init(string pic1, string pic2, int st, string area)
        {
            enterBefore = pic1;
            enterAfter = pic2;
            step = UIManager.Instance._DzTipUI.step;  //当前环节编号
            stepStart = st;
            currArea = area;
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(enterBefore);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //当前环节还未到最后一个环节
            if (step < Manager.Instance.dzTips.Count)
            {
                //重新体验
                if (btnType == BtnType.TipBtn1)
                {
                    UIManager.Instance.RestAllUI();
                    UIManager.Instance._Title.uiobj.SetActive(true);
                    //将步骤恢复到当前环节开始步骤，减1是由于后续还有加1
                    GameUnitManager.Instance._Scene2.Tmpstep = stepStart - 1;
                    //任务栏的标签恢复到当前环节
                    UIManager.Instance._Title.task.TaskCancel(GameUnitManager.Instance._Scene2.togID);
                    GameUnitManager.Instance._Scene2.togID--;
                    UIManager.Instance._Title.task.TaskSelect(GameUnitManager.Instance._Scene2.togID);
                    //重新设置人物所在场景
                    if (!currArea.Equals(Manager.Instance.AllDzSteps[stepStart].Area))
                    {
                        int area = int.Parse(Manager.Instance.AllDzSteps[stepStart].Area);
                        PlayerMove.Instance.ResetPosAndRot(area);
                    }
                    //重置已经执行过的动画
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
                //继续学习
                UIManager.Instance._DzTipUI.uiobj.SetActive(false);
                GameUnitManager.Instance.Next();
            }
            else if (step == Manager.Instance.dzTips.Count)  //最后一个环节结束后的提示信息按钮
            {
                //自主探索
                if (btnType == BtnType.TipBtn1)
                {
                    UIManager.Instance._DzTipUI.uiobj.SetActive(false);
                }
                else if(btnType == BtnType.TipBtn2)  //返回主标题
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