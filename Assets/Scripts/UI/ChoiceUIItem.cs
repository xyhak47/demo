using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������޸����ѡ����Ϣ
/// </summary>
namespace XD.UI
{
    public class ChoiceUIItem : MonoBehaviour
    {
        public Text itemCnt;
        public Toggle isKaoHeBtn;
        public Toggle[] choiceBtn;
        public GameObject faultChoiceTggl;
        public GameObject faultChoiceText;

        internal void Init(string cnt)
        {
            itemCnt.text = cnt;
            isKaoHeBtn.isOn = false;
            choiceBtn[0].isOn = true;
        }

        internal bool JudgFaultChoice()
        {
            if (faultChoiceTggl.activeSelf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
