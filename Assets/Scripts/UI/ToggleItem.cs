using UnityEngine;
using UnityEngine.UI;
using XD.GameStatic;
using XD.TheManager;

namespace XD.UI { 
    public class ToggleItem : MonoBehaviour
    {
        public int ID;
        public Toggle thisTog;
        public Text optionText;//选项文本
        public float Score;//选项对应分值

        /// <summary>
        /// 初始化设置答案选项按钮信息
        /// </summary>
        /// <param name="id">答案选项编号</param>
        /// <param name="questionItemData">答案选项对应的题目</param>
        /// <param name="text">答案选项对应的文本内容</param>
        /// <param name="btnItem">答案选项对应题目的答题卡标签按钮</param>
        public void Init(int id, QuestionItemDatas questionItemData, string text, QuestionBtnItem btnItem)
        {
            ID = id;
            Score = (int)questionItemData.proScore;
            optionText.text = text;
            thisTog.onValueChanged.AddListener((isSelect) =>
            {
                UIManager.Instance._ExamUI._question[UIManager.Instance._ExamUI.questionCount].SelectToggle();
                btnItem.ChangeQuestionState(QuestionState.Finished);
                btnItem.QuestionScore = Score;//将选择的选项分值赋值给当前题目
            });
        }
    }
}
