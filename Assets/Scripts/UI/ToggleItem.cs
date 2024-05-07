using UnityEngine;
using UnityEngine.UI;
using XD.GameStatic;
using XD.TheManager;

namespace XD.UI { 
    public class ToggleItem : MonoBehaviour
    {
        public int ID;
        public Toggle thisTog;
        public Text optionText;//ѡ���ı�
        public float Score;//ѡ���Ӧ��ֵ

        /// <summary>
        /// ��ʼ�����ô�ѡ�ť��Ϣ
        /// </summary>
        /// <param name="id">��ѡ����</param>
        /// <param name="questionItemData">��ѡ���Ӧ����Ŀ</param>
        /// <param name="text">��ѡ���Ӧ���ı�����</param>
        /// <param name="btnItem">��ѡ���Ӧ��Ŀ�Ĵ��⿨��ǩ��ť</param>
        public void Init(int id, QuestionItemDatas questionItemData, string text, QuestionBtnItem btnItem)
        {
            ID = id;
            Score = (int)questionItemData.proScore;
            optionText.text = text;
            thisTog.onValueChanged.AddListener((isSelect) =>
            {
                UIManager.Instance._ExamUI._question[UIManager.Instance._ExamUI.questionCount].SelectToggle();
                btnItem.ChangeQuestionState(QuestionState.Finished);
                btnItem.QuestionScore = Score;//��ѡ���ѡ���ֵ��ֵ����ǰ��Ŀ
            });
        }
    }
}
