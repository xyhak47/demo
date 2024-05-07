using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPic : MonoBehaviour
{
    public Image image;
    public CanvasGroup canvasGroup;
    //��˸�ٶ�
    public float alphaSpeed = 10f;
    //���͸����
    public float alpha = 0f;
    //����������˸������
    private bool isShow = true;

    // Update is called once per frame
    void Update()
    {
        if (image.gameObject.activeSelf)
        {
            if (isShow)
            {
                if (canvasGroup.alpha != alpha)
                {
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, alpha, alphaSpeed * Time.deltaTime);
                    if (Mathf.Abs(canvasGroup.alpha - alpha) <= 0.01)
                    {
                        canvasGroup.alpha = alpha;
                        isShow = false;
                    }
                }
            }
            else
            {
                if (canvasGroup.alpha != 1)
                {
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, alphaSpeed * Time.deltaTime);
                    if (Mathf.Abs(1 - canvasGroup.alpha) <= 0.01)
                    {
                        canvasGroup.alpha = 1;
                        isShow = true;
                    }
                }
            }
        }
        else
        {
            canvasGroup.alpha = 1f;
            isShow = true;
        }
    }
}