using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD.UI;

public class Param : MonoBehaviour
{
	public Ui3DModel _ui3DModel;

	/* ����Ŀ��λ��
	 * targetPos[0]:��ʼλ��
	 * targetPos[1]:������λ��
	 * targetPos[2]:������λ��
	 */
	public List<Transform> targetPos = new List<Transform>();

	/* ��������ײ��
	 * pDoor[0]:�ֳ��ŵ���ײ��
	 * pDoor[1]:�����ҵ���ײ��
	 * pDoor[2]:�����ҵ���ײ��
	 */
	public List<Collider> pDoor = new List<Collider>();

	//�����������
	public LineRenderer line;

	//����͸����Ǵ�������ⲿģ��
	public GameObject[] ZhubianModels;


	public GameObject[] ZhuBianTransModels;

	//�����ߵĸ�����
	public Transform ArrowP;

	//20221117����������ģ��
	public GameObject[] Fault;

	//20221129�������洢����������޸�volume mask
	//public Camera URPCam;
}
