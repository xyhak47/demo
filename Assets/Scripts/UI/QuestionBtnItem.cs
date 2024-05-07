using UnityEngine;
using UnityEngine.UI;
using XD.TheManager;

namespace XD.UI
{ 
    /// <summary>
    /// ��ĿĬ�ϡ�ѡ�С������������״̬
    /// </summary>
    public enum QuestionState
    {
        Default,
        Select,
        Finished
    }

    /// <summary>
    /// ��Ŀ��Ӧ��ť
    /// </summary>
    public class QuestionBtnItem : MonoBehaviour
    {
        public int Id;
        public Image img;
        public Toggle thisBtn;
        public Sprite Default;//Ĭ�ϻ�ɫͼ
        public Sprite Finished;//�����ɫͼ
        public Sprite Select;//ѡ����ɫͼ
        private System.Action<int> callBack;
        private QuestionState questionState = QuestionState.Default;//��ĿĬ��δѡ��״̬
        public bool isSelectAnswer = false;//��ǰ��Ŀ�Ƿ���ѡ���
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
                ChangeQuestionState(QuestionState.Select);//Ĭ��ѡ�е�һ����
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
        /// �޸���Ŀ״̬
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
