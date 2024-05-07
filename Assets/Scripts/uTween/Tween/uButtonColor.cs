using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace uTools
{
    public enum Utype
    {
        RawImage,
        Image,
    }

    [AddComponentMenu("uTools/Tween/Button Color(uTools)")]
    public class uButtonColor : MonoBehaviour, uIPointHandler
    {
        public Utype utype = Utype.RawImage;
        public RawImage tweenTarget_1;
        public Image tweenTarget_2;
        public Color Normal = new Color(255, 255, 255, 255);
        public Color enter = new Color32(255, 255, 255,255);
        public Color down = new Color32(100,100,100,255);
        public float duration = .2f;

        Color mColor;

        // Use this for initialization
        void Start()
        {

            if (utype == Utype.RawImage)
            {
                //tweenTarget_1 = GetComponent<RawImage>();
                tweenTarget_1.color = Normal;
                mColor = tweenTarget_1.color;
            }
            else if (utype == Utype.Image)
            {
                //tweenTarget_2 = GetComponent<Image>();
                tweenTarget_2.color = Normal;
                mColor = tweenTarget_2.color;
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Color(enter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Color(mColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Color(down);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Color(mColor);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        void Color(Color to)
        {
            if (utype == Utype.RawImage)
                uTweenColor.Begin(tweenTarget_1.gameObject, duration,0f, tweenTarget_1.color, to);
            else if (utype == Utype.Image)
                uTweenColor.Begin(tweenTarget_2.gameObject, duration, 0f, tweenTarget_2.color, to);
        }
    }
}
