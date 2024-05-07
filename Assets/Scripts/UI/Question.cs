using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XD.UI;

public class Question : MonoBehaviour
{
    /// <summary>
    /// 左侧题目导航栏
    /// </summary>
    public class QuestionNumber
    {
        /// <summary>
        /// 题目及选项
        /// </summary>
        public class QuestionContent
        {
            private string questionType;
            private QuestionNumber number;
            private List<ToggleItem> options;
            public QuestionContent(QuestionNumber _number, List<ToggleItem> tog)
            {
                number = _number;
                options = tog;
            }
            /// <summary>
            /// 控制选项的显隐
            /// </summary>
            /// <param name="value"></param>
            public void SetVisible(bool value)
            {
                foreach (ToggleItem item in options)
                {
                    item.gameObject.SetActive(value);
                }
            }
        }

        private List<QuestionContent> cacheQuestions = new List<QuestionContent>();
        private QuestionBtnItem BtnItem;
        private GameObject text;

        public QuestionNumber(QuestionBtnItem btnItem, GameObject tex)
        {
            BtnItem = btnItem;
            text = tex;
        }
        /// <summary>
        /// 控制题目显隐
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value)
        {
            text.SetActive(value);
        }
        /// <summary>
        /// 添加题目到题目缓存列表
        /// </summary>
        /// <param name="question"></param>
        public void AddQuestion(QuestionContent question)
        {
            cacheQuestions.Add(question);
        }
        public void RemoveQuestion(QuestionContent question)
        {
            cacheQuestions.Remove(question);
        }
    }
}
