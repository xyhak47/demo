using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XD.TheManager;
using XD.GameStatic;
using XD.Utils;
using System;

namespace XD.Tasks
{
	[Serializable]
	public class Task : MonoBehaviour
	{
		//public int id;
		internal List<TaskItem> taskTogList = new List<TaskItem>();
		//public Sprite Default; //任务未完成状态的ui图
		//public Sprite Finished;//任务完成状态的ui图

		/// <summary>
		/// 初始化任务栏，默认选中第一项
		/// </summary>
		internal void Init()
        {
			
			switch (GameUnitManager.Instance._SceneType)
			{
				case SceneType.xunshi:
					SetToggle(Manager.Instance.XunshiTasks);
					break;
				case SceneType.daozha:
					SetToggle(Manager.Instance.DaozhaTasks);
					break;
            }
        }

		private void SetToggle(List<TaskList> task)
        {
			ClearToggle();
			for (int i = 0; i < task.Count; i++)
			{
				TaskItem Toggle = Manager.Instance.InitResObj(UIManager.Instance._Title._taskList.transform, "UI/Prefabs/taskToggle").GetComponent<TaskItem>();
				if (task[i].TaskSprite.Contains("|"))
				{
					string[] s = task[i].TaskSprite.Split('|');
					if (GameUnitManager.Instance._EventSystemType == EventSystemType.Study)
					{
						Toggle.Init(i, s[0], s[1], task[i].StudyStep);
					}
					else if (GameUnitManager.Instance._EventSystemType == EventSystemType.Practice)
					{
						Toggle.Init(i, s[0], s[1], task[i].PracticeStep);
					}
					taskTogList.Add(Toggle);
				}
			}
		}

		/// <summary>
		/// 切换到下一步骤的任务，并将上一步的框勾选
		/// </summary>
		/// <param name="togID"></param>
		internal void TaskDone(int togID)
        {
			taskTogList[togID].img.sprite = taskTogList[togID].enterBefore;
			taskTogList[togID].thisTog.isOn = true;
		}

		/// <summary>
		/// 任务被选中
		/// </summary>
		/// <param name="togID"></param>
		internal void TaskSelect(int togID)
        {
			taskTogList[togID].img.sprite = taskTogList[togID].enterAfter;
		}

		/// <summary>
		/// 任务被取消选中
		/// </summary>
		/// <param name="togID"></param>
		internal void TaskCancel(int togID)
        {
			taskTogList[togID].img.sprite = taskTogList[togID].enterBefore;
		}

		internal void SetTaskList(int step, ref int id)
		{
			if (step > taskTogList[id].stepEnd)
			{
				TaskDone(id);
				id++;
				if (id < taskTogList.Count)
                {
					TaskSelect(id);
				}
                else
                {
					id--;
                }
			}
		}

		private void ClearToggle()
        {
			Toggle[] toggle = UIManager.Instance._Title._taskList.GetComponentsInChildren<Toggle>();
            if (toggle.Length > 0)
            {
				foreach(Toggle _t in toggle)
                {
					Destroy(_t.gameObject);
                }
            }
			taskTogList.Clear();
		}
	}
}
