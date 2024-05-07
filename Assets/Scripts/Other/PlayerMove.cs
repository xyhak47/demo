using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XD.GameStatic;
using XD.TheManager;
using XD.UI;

public class PlayerMove : Singleton<PlayerMove>
{
	public float _speed = 5.0f;
	public float _roateSpeed = 5.0f;

	internal bool IsMoveControl = false;
	internal bool IsRotationControl = false;

	internal Camera mainCamera;
	public Param _param;
	internal Ui3DModel _ui3DModel;
	internal NavMeshAgent navMeshAgent; //导航代理
	internal NavMeshPath navMeshPath;
	public float tileSpacing = 0.5f;
	public LineRenderer line;
	//private Material lineMat;
	public GameObject directionPrefab; //箭头的预制体
	public Transform ArrowP; //箭头预制体的父物体
	internal List<GameObject> arrowList = new List<GameObject>();
	internal bool togOn;
	internal bool IsMove; //允许移动
	internal bool IsPortal; //允许传送
	internal int area;    //人物移动的区域编号
	private Portal portal;

	/* 传送目标位置
	 * targetPos[0]:初始位置
	 * targetPos[1]:控制室位置
	 * targetPos[2]:工具室位置
	 */
	internal List<Transform> targetPos;

	/* 传送门碰撞体
	 * pDoor[0]:现场门的碰撞体
	 * pDoor[1]:控制室的碰撞体
	 * pDoor[2]:工具室的碰撞体
	 */
	internal List<Collider> pDoor;
	private GameObject door;
	internal int targetArea;  //传送区域标识

	private void Start()
    {
		mainCamera = Camera.main;
		IsMoveControl = true;
		IsRotationControl = true;
		togOn = false;
		IsMove = false;
		IsPortal = false;
		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		navMeshPath = new NavMeshPath();
		//lineMat = Resources.Load<Material>("Material/Transparent");
		//DestroyLine();
		//ResetCollider();
		//line = gameObject.GetComponent<LineRenderer>();
	}

	/// <summary>
	/// 设置参数，_ui3DModel，targetPos，pDoor
	/// </summary>
	internal void Init()
    {
		_param = GameObject.Find("Param").GetComponent<Param>();
		_ui3DModel = _param._ui3DModel;
		targetPos = _param.targetPos;
		line = _param.line;
		pDoor = _param.pDoor;
		ArrowP = _param.ArrowP;
		ResetCollider();
		DisActiveFault();
		gameObject.GetComponent<NavMeshAgent>().enabled = false;
		SetPosAndRot();
		gameObject.GetComponent<NavMeshAgent>().enabled = true;
	}

    private void FixedUpdate()
    {
		if (IsMoveControl)
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				transform.position += _speed * Time.deltaTime * transform.forward;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				transform.position += -_speed * Time.deltaTime * transform.forward;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				transform.position += -_speed * Time.deltaTime * transform.right;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				transform.position += _speed * Time.deltaTime * transform.right;
			}
		}

		if (IsRotationControl)
		{
			if (Input.GetMouseButton(0))
			{
				float x = Input.GetAxis("Mouse X");     //控制人物转向
				float y = Input.GetAxis("Mouse Y");     //控制镜头上下
				transform.Rotate(Vector3.up, x * _roateSpeed * Time.deltaTime);
				mainCamera.transform.Rotate(Vector3.right * -y * _roateSpeed * Time.deltaTime);
			}
		}
	}
    private void Update()
    {
		if (IsMove)
		{
			//移动引导期间，任务栏及帮助无法显示点击
			if (togOn)
            {
				UIManager.Instance._Title.toggles[1].isOn = false;
			}
			UIManager.Instance._Title.toggles[2].interactable = false;
			UIManager.Instance._Title.toggles[1].interactable = false;
			if (IsDestination())
            {
				//DestroyLine();
				IsMove = false;
				//清除最后的箭头
				ResetArrows();
				//StopCoroutine(ClearArrows(arrowList));
				UIManager.Instance._GuideTips.SetActive(false);
				if (!IsPortal)
                {
					GameUnitManager.Instance.Next();  //到达目的地，执行下一步
				}
                else
                {
					IsPortal = false;
                    switch (area)
                    {
						//控制室
						case 1:
							if(targetArea == 2)
                            {
								pDoor[2].enabled = true;
								door = pDoor[2].gameObject;
							}
							else if(targetArea == 3)
                            {
								pDoor[1].enabled = true;
								door = pDoor[1].gameObject;
							}
							break;
						//工具室
						case 2:
							pDoor[3].enabled = true;
							door = pDoor[3].gameObject;
							break;
						//现场
						case 3:
							pDoor[0].enabled = true;
							door = pDoor[0].gameObject;
							break;
                    }
					SetPortalTarget(targetArea);
				}
			}
			NavMesh.CalculatePath(transform.position, navMeshAgent.destination, NavMesh.AllAreas, navMeshPath);
			DrawPath(navMeshPath);
			//DrawLine();
		}
	}

	/// <summary>
	/// 设置人物位置
	/// </summary>
	/// <param name="_transform"></param>
	internal void SetPosAndRot()
    {
		transform.position = targetPos[0].position;
		transform.rotation = targetPos[0].rotation;
		transform.localScale = targetPos[0].localScale;
	}

	/// <summary>
	/// 相机归位
	/// </summary>
	internal void ResetCamera()
    {
		mainCamera.transform.localPosition = Vector3.zero;
    }

	/// <summary>
	/// 重置门上碰撞体
	/// </summary>
	internal void ResetCollider()
    {
		foreach (Collider _c in pDoor)
			_c.enabled = false;
	}

	/// <summary>
	/// 路径画线
	/// </summary>
	/*private void DrawLine()
	{
		//如果导航里面的点大于1
		if (navMeshAgent.path.corners.Length > 1)
		{
			//画线的点位等于导航的点位
			line.positionCount = navMeshAgent.path.corners.Length;
			line.SetPositions(navMeshAgent.path.corners);
		}
	}*/

	private void DrawPath(NavMeshPath navPath)
    {
		List<GameObject> arrows = arrowList;
		StartCoroutine(ClearArrows(arrows));
		arrowList.Clear();
		//If the path has 1 or no corners, there is no need to draw the line

		if (navPath.corners.Length < 2)
		{
			print("navPath.corners.Length < 2");
			return;
		}
		// Set the array of positions to the amount of corners...
		line.positionCount = navPath.corners.Length;
		Quaternion planerot = Quaternion.identity;
		for (int i = 0; i < navPath.corners.Length; i++)
		{
			// Go through each corner and set that to the line renderer's position...

			line.SetPosition(i, navPath.corners[i]);
			float distance = 0;
			Vector3 offsetVector = Vector3.zero;
			if (i < navPath.corners.Length - 1)
			{
				//plane rotation calculation
				offsetVector = navPath.corners[i + 1] - navPath.corners[i];
				planerot = Quaternion.LookRotation(offsetVector);
				distance = Vector3.Distance(navPath.corners[i + 1], navPath.corners[i]);
				if (distance < tileSpacing)
					continue;

				planerot = Quaternion.Euler(90, planerot.eulerAngles.y, planerot.eulerAngles.z);
				//plane position calculation
				float newSpacing = 0;
				for (int j = 0; j < distance / tileSpacing; j++)
				{
					newSpacing += tileSpacing;
					var normalizedVector = offsetVector.normalized;
					var position = navPath.corners[i] + newSpacing * normalizedVector;
					//GameObject go = Instantiate(directionPrefab, position + Vector3.up, planerot);
					GameObject go = Instantiate(directionPrefab, position, planerot, ArrowP);
					arrowList.Add(go);
				}
			}
			else
			{
				//GameObject go = Instantiate(directionPrefab, navPath.corners[i] + Vector3.up, planerot);
				GameObject go = Instantiate(directionPrefab, navPath.corners[i], planerot, ArrowP);
				arrowList.Add(go);
			}
		}
	}

	private IEnumerator ClearArrows(List<GameObject> arrows)
    {
		if (arrowList.Count == 0)
			yield break;
		foreach (var arrow in arrows)
			Destroy(arrow);
	}

	private void ResetArrows()
    {
		if (ArrowP.childCount > 0)
		{
			ArrowP.gameObject.SetActive(false);
			/*for(int i = 0; i < ArrowP.childCount; i++)
            {
				Destroy(ArrowP.GetChild(i).gameObject);
				ArrowP.gameObject.SetActive(false);
            }*/
		}
		else return;
	}

	/// <summary>
	/// 销毁线
	/// </summary>
	/*internal void DestroyLine()
    {
        IsMove = false;
        if (gameObject.GetComponent<LineRenderer>())
            Destroy(gameObject.GetComponent<LineRenderer>());  //销毁画线组件
    }*/

	/// <summary>
	/// LineRenderer组件参数设置
	/// </summary>
	/*internal void SetLine()
	{
		//设置材质
		//line.material = lineMat;
		//设置颜色
		line.startColor = Color.yellow;
		line.endColor = Color.red;
		//设置宽度
		line.startWidth = 0.1f;
		line.endWidth = 0.1f;
	}*/

	/// <summary>
	/// 判断是否到达目的地
	/// </summary>
	/// <returns></returns>
	private bool IsDestination()
    {
		Vector3 _pos = transform.position - navMeshAgent.destination;
		float f = _pos.magnitude;
        if (f < 1.5f)
        {
			UIManager.Instance._Title.toggles[1].isOn = togOn;
			UIManager.Instance._Title.toggles[1].interactable = true;
			if(GameUnitManager.Instance._EventSystemType == EventSystemType.Study)
				UIManager.Instance._Title.toggles[2].interactable = true;
			return true;
        }
		return false;
    }

	/// <summary>
	/// 设置传送脚本中的目的地
	/// </summary>
	/// <param name="_t">需要传送到的目的地标识</param>
	internal void SetPortalTarget(int _t)
    {
		portal = door.GetComponent<Portal>();
		switch (_t)
        {
			//前往控制室
			case 1:
				if(area == 3)
                {
					portal.InitTarget(targetPos[1]);
				}
				else if(area == 2)
                {
					portal.InitTarget(targetPos[2]);
				}
				break;
			//前往工具室
			case 2:
				portal.InitTarget(targetPos[3]);
				break;
			//前往现场
			case 3:
				portal.InitTarget(targetPos[0]);
				break;
        }
    }

	/// <summary>
	/// 重置人物初始位置
	/// </summary>
	/// <param name="area"></param>
	internal void ResetPosAndRot(int area)
    {
		gameObject.GetComponent<NavMeshAgent>().enabled = false;
        switch (area)
        {
			//控制室初始位置
			case 1:
				transform.position = targetPos[1].position;
				transform.rotation = targetPos[1].rotation;
				transform.localScale = targetPos[1].localScale;
				break;
			//工具室初始位置
			case 2:
				transform.position = targetPos[3].position;
				transform.rotation = targetPos[3].rotation;
				transform.localScale = targetPos[3].localScale;
				break;
			//现场初始位置
			case 3:
				transform.position = targetPos[0].position;
				transform.rotation = targetPos[0].rotation;
				transform.localScale = targetPos[0].localScale;
				break;
        }
		gameObject.GetComponent<NavMeshAgent>().enabled = true;
	}


	internal void SetFaultArgs(bool showFault, string faultModName, string location)
    {
		DisActiveFault();
		//定位
		if (location != "null")
		{
			gameObject.GetComponent<NavMeshAgent>().enabled = false;
			if (location.Contains("|"))
			{
				string[] args = location.Split('|');
				foreach (string arg in args)
				{
					if (arg != "null")
					{
						if (arg.Contains(":"))
						{
							string[] _s = arg.Split(':');
							switch (_s[0])
							{
								case "pos":
									transform.position = Manager.Instance.Vector3Parse(_s[1]);
									break;
								case "rot":
									transform.eulerAngles = Manager.Instance.Vector3Parse(_s[1]);
									break;
							}
						}
					}
				}
			}
			gameObject.GetComponent<NavMeshAgent>().enabled = true;
		}
		//是否显示故障模型
		if (showFault)
        {
			if (faultModName != "null")
			{
				foreach (GameObject _o in _param.Fault)
				{
					if (_o.name == faultModName)
					{
						_o.SetActive(true);
					}
				}
			}
		}
    }

	private void DisActiveFault()
    {
		foreach (GameObject _o in _param.Fault)
		{
			_o.SetActive(false);
		}
	}
}
