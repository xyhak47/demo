using UnityEngine;
using UnityEngine.UI;
using XD.TheManager;

namespace XD.UI
{ 
    /// <summary>
    /// 题目默认、选中、完成作答三个状态
    /// </summary>
    public enum QuestionState
    {
        Default,
        Select,
        Finished
    }

    /// <summary>
    /// 题目对应按钮
    /// </summary>
    public class QuestionBtnItem : MonoBehaviour
    {
        public int Id;
        public Image img;
        public Toggle thisBtn;
        public Sprite Default;//默认灰色图
        public Sprite Finished;//完成深色图
        public Sprite Select;//选中颜色图
        private System.Action<int> callBack;
        private QuestionState questionState = QuestionState.Default;//题目默认未选中状态
        public bool isSelectAnswer = false;//当前题目是否已选择答案
        private float questionScore = 0;
        public float QuestionScore
        {
            get { return questionScore; }
            set { questionScore = value; }
        }

        public void Init(int i, int num)
        {
            thisBtn = gameObject.GetComponent<Toggle>();
            if (i == 0)
            {
                ChangeQuestionState(QuestionState.Select);//默认选中第一道题
            }
            Id = i;
            thisBtn.name = num.ToString();
            thisBtn.GetComponentInChildren<Text>().text = thisBtn.name;
            thisBtn.onValueChanged.AddListener((isSelect) =>
            {
                UIManager.Instance._ExamUI.questionCount = Id;
                UIManager.Instance._ExamUI.SetPaper(UIManager.Instance._ExamUI.questionCount);
            });
        }
        /// <summary>
        /// 修改题目状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeQuestionState(QuestionState state)
        {
            switch (state)
            {
                case QuestionState.Default:
                    img.sprite = Default;
                    break;
                case QuestionState.Select:
                    thisBtn.isOn = true;
                    break;
                case QuestionState.Finished:
                    if (!isSelectAnswer)
                    {
                        isSelectAnswer = true;
                        img.sprite = Finished;
                        UIManager.Instance._ExamUI.SelectedNum++;
                        UIManager.Instance._ExamUI.record.text = UIManager.Instance._ExamUI.SelectedNum.ToString() + "/20";
                    }
                    break;
            }
        }
    }
}
