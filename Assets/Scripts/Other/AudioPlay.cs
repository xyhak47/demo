using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XD.TheManager;

public class AudioPlay : MonoBehaviour
{
    public static AudioPlay Instance;
    internal AudioClip audioClip;
    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ��������
    /// </summary>
    internal void PlayAudio()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// �ر�����
    /// </summary>
    internal void StopAudio()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// �ȴ�������������ٽ�����һ��
    /// </summary>
    internal void WaitAudio()
    {
        StartCoroutine(WaitOver(GameUnitManager.Instance.Next));
    }

    IEnumerator WaitOver(UnityAction unityAction = null)
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        if (unityAction != null)
        {
            unityAction.Invoke();
        }
    }
}
