using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD.UI;

public class Param : MonoBehaviour
{
	public Ui3DModel _ui3DModel;

	/* 传送目标位置
	 * targetPos[0]:初始位置
	 * targetPos[1]:控制室位置
	 * targetPos[2]:工具室位置
	 */
	public List<Transform> targetPos = new List<Transform>();

	/* 传送门碰撞体
	 * pDoor[0]:现场门的碰撞体
	 * pDoor[1]:控制室的碰撞体
	 * pDoor[2]:工具室的碰撞体
	 */
	public List<Collider> pDoor = new List<Collider>();

	//画引导线组件
	public LineRenderer line;

	//用于透明外壳存放主变外部模型
	public GameObject[] ZhubianModels;


	public GameObject[] ZhuBianTransModels;

	//引导线的父物体
	public Transform ArrowP;

	//20221117新增：故障模型
	public GameObject[] Fault;

	//20221129新增：存储主相机，以修改volume mask
	//public Camera URPCam;
}
