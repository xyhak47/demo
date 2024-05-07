using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD.TheManager;

public class AnimPlay : MonoBehaviour
{
    internal Animation _anim;
    private bool startPlay = false;
    
    /// <summary>
    /// ���ö���
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clip">����Ƭ������</param>
    internal void Init(Animation anim,string clip)
    {
        _anim = anim;
        startPlay = true;
        _anim.Play(clip); //���Ŷ���Ƭ��
    }

    // Update is called once per frame
    void Update()
    {
        //�ж��Ƿ�ʼ���Ŷ���
        if (startPlay)
        {
            //�ж϶����Ƿ񲥷����
            if (!_anim.isPlaying)
            {
                startPlay = false;
                GameUnitManager.Instance.Next();
            }
        }
    }
}
