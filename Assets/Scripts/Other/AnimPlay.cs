using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD.TheManager;

public class AnimPlay : MonoBehaviour
{
    internal Animation _anim;
    private bool startPlay = false;
    
    /// <summary>
    /// 设置动画
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clip">动画片段名称</param>
    internal void Init(Animation anim,string clip)
    {
        _anim = anim;
        startPlay = true;
        _anim.Play(clip); //播放动画片段
    }

    // Update is called once per frame
    void Update()
    {
        //判断是否开始播放动画
        if (startPlay)
        {
            //判断动画是否播放完成
            if (!_anim.isPlaying)
            {
                startPlay = false;
                GameUnitManager.Instance.Next();
            }
        }
    }
}
