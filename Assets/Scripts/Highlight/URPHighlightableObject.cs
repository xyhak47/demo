using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XD.TheManager;
using XD.GameStatic;

public class URPHighlightableObject : MonoBehaviour
{
    [HideInInspector]
    public Material material;

    void OnEnable()
    {
        URPHighlightingSystemRenderPass.AddTarget(this);
    }

    void OnDisable()
    {
        URPHighlightingSystemRenderPass.RemoveTarget(this);
    }

    public void DestoryMaterial() {
        DestroyImmediate(material);
    }

    #region 鼠标回调
    /// <summary>
    /// 鼠标点击抬起
    /// </summary>
    private void OnMouseUpAsButton()
    {
        this.enabled = false;
        Manager.Instance.ClearData(gameObject);

        if (GameUnitManager.Instance._SceneType == SceneType.xunshi)
        {
            if (GameUnitManager.Instance._EventSystemType == EventSystemType.Study)
            {
                UIManager.Instance._ModelUI.InitStart();
            }
            else if (GameUnitManager.Instance._EventSystemType == EventSystemType.Practice)
            {
                GameUnitManager.Instance.Next();
            }
        }
        else if (GameUnitManager.Instance._SceneType == SceneType.daozha)
        {
            UIManager.Instance._DaoZhaUI.IsAlpha = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            if (GameUnitManager.Instance._Scene2.isClkSound)
            {
                GameUnitManager.Instance._Scene2.isClkSound = false;
                AudioPlay.Instance.audioSource.loop = false;
                GameUnitManager.Instance._Scene2.isStop = true;
                AudioPlay.Instance.audioClip = Manager.Instance.DzAudio[GameUnitManager.Instance._Scene2.Tmpstep];
                AudioPlay.Instance.PlayAudio();
                AudioPlay.Instance.WaitAudio();
            }
            else
            {
                if (!GameUnitManager.Instance._Scene2.isStop)
                {
                    GameUnitManager.Instance._Scene2.isStop = true;
                }
                GameUnitManager.Instance.Next();
            }
        }
    }
    #endregion
}