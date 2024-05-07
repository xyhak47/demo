using System.Collections;
using System.Collections.Generic;
using TheGameCore;
using UnityEngine;
using XD.TheManager;

public class ClickHelp : MonoBehaviour
{
    public static ClickHelp Instance;
    internal bool isAllow = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isAllow)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                if (hits.Length != 0)
                {
                    JudgeHit(hits);
                }
            }
        }
    }

    /// <summary>
    /// �жϵ���������������Ƿ������Ҫ�����ۿ���ģ��
    /// </summary>
    /// <param name="myhits"></param>
    private void JudgeHit(RaycastHit[] myhits)
    {
        foreach(RaycastHit hit in myhits)
        {
            if (Manager.Instance.lookAtClick.ContainsKey(hit.collider.name))
            {
                SetLocation(Manager.Instance.lookAtClick[hit.collider.name]);
                return;
            }
        }
    }

    /// <summary>
    /// ���������λ�ã���������
    /// </summary>
    private void SetLocation(Vector3[] location)
    {
        GameObject _obj = GameObject.Find("ExAnimCamera");
        //ʵʱ�����ǹر�״̬
        if (!UIManager.Instance._ExAnimWin.isShow)
        {
            _obj.transform.position = location[0];
            _obj.transform.eulerAngles = location[1];
            UIManager.Instance._ExAnimWin.isShow = true;
            UIManager.Instance._ExAnimWin.ShowWin();
        }
        _obj.transform.eulerAngles = location[3];
        iTween.MoveTo(_obj, location[2], 1f);
    }
}
