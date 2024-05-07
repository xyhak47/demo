using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XD.TheManager;

public class Portal : MonoBehaviour
{
    internal Vector3 _tarPos;
    public Transform _target;

    private Vector3 pos;
    private Quaternion rot;
    private Vector3 scale;

    /// <summary>
    /// ���ô���Ŀ�ĵ�
    /// </summary>
    /// <param name="_t"></param>
    internal void InitTarget(Transform _t)
    {
        _target = _t;
    }

    /// <summary>
    /// ������ײ������ô��ͺ����������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        pos = _target.position;
        rot = _target.rotation;
        scale = _target.localScale;
        other.gameObject.transform.position = pos;
        other.gameObject.transform.rotation = rot;
        other.gameObject.transform.localScale = scale;
        other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        //���͵����ִ����һ������
        Invoke("Next", 0f);
    }

    private void Next()
    {
        GameUnitManager.Instance.Next();
    }
}
