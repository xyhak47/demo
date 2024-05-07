using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplitModel : MonoBehaviour
{
    public static SplitModel Instance;
    public Transform m_ParObj;//中心点
    public List<GameObject> m_Child = new List<GameObject>();//所有子对象
    private List<Vector3> m_InitPoint = new List<Vector3>();//初始位置

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    { 
        for (int i = 0; i < m_Child.Count; i++)
        {
            m_InitPoint.Add(m_Child[i].transform.position);
        }
    }

    public Vector3 SplitObjTest(Transform m_ParObj, Transform _TargetObj)
    {
        Vector3 tempV3;
        /*tempV3.x = (_TargetObj.position.x - m_ParObj.position.x + m_ParObj.localPosition.x) * 2;
        tempV3.y = (_TargetObj.position.y - m_ParObj.position.y + m_ParObj.localPosition.y) * 2;
        tempV3.z = (_TargetObj.position.z - m_ParObj.position.z + m_ParObj.localPosition.z) * 2.5f;*/
        tempV3.x = (_TargetObj.position.x - m_ParObj.position.x) * 2;
        tempV3.y = (_TargetObj.position.y - m_ParObj.position.y) * 2;
        tempV3.z = (_TargetObj.position.z - m_ParObj.position.z) * 2;
        return tempV3;
    }

    public void SplitObject()
    {
        for (int i = 0; i < m_Child.Count; i++)
        {
            Vector3 tempV3 = SplitObjTest(m_ParObj, m_Child[i].transform);
            m_Child[i].transform.DOMove(tempV3, 3f, false);
        }
    }

    public void MergeObject()
    {
        for (int i = 0; i < m_InitPoint.Count; i++)
        {
            m_Child[i].transform.DOMove(m_InitPoint[i], 3f, false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.W))
        {
            //拆分
            SplitObject();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //合并
            MergeObject();
        }*/
    }
}
