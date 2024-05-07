using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XD.UI;

public class Question : MonoBehaviour
{
    /// <summary>
    /// �����Ŀ������
    /// </summary>
    public class QuestionNumber
    {
        /// <summary>
        /// ��Ŀ��ѡ��
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
            /// ����ѡ�������
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
        /// ������Ŀ����
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value)
        {
            text.SetActive(value);
        }
        /// <summary>
        /// �����Ŀ����Ŀ�����б�
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
