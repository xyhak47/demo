using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Alpha(uTools)")]
	public class uTweenAlpha : uTweenValue {

		public bool includeChilds = false;

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
        private RawImage mRawImage;
        private SpriteRenderer mSpriteRender;

		float mAlpha = 0f;

		public float alpha {
			get {
				return mAlpha;
			}
			set {
				SetAlpha(transform, value);
				mAlpha = value;
			}
		}

		protected override void ValueUpdate (float value, bool isFinished)
		{
			alpha = value;
		}

		void SetAlpha(Transform _transform, float _alpha) {
			Color c = Color.white;
			mText = _transform.GetComponent<Text> ();
			if (mText != null){
				c = mText.color;
				c.a = _alpha;
				mText.color = c;
			}
			mLight = _transform.GetComponent<Light>();
			if (mLight != null){ 
				c = mLight.color;
				c.a = _alpha;
				mLight.color = c;
			}
			mImage = _transform.GetComponent<Image> ();
			if (mImage != null) {
				c = mImage.color;
				c.a = _alpha;
				mImage.color = c;
			}
            mRawImage = _transform.GetComponent<RawImage>();
            if (mRawImage != null)
            {
                c = mRawImage.color;
                c.a = _alpha;
                mRawImage.color = c;
            }
            mSpriteRender = _transform.GetComponent<SpriteRenderer> ();
			if (mSpriteRender != null) {
				c = mSpriteRender.color;
				c.a = _alpha;
				mSpriteRender.color = c;
			}
			if (_transform.GetComponent<Renderer>() != null) {
				mMat = _transform.GetComponent<Renderer>().material;
				if (mMat != null) {
					c = mMat.color;
					c.a = _alpha;
					mMat.color = c;
				}
			}
			if (includeChilds) {
				for (int i = 0; i < _transform.childCount; ++i) {
					Transform child = _transform.GetChild(i);
					SetAlpha(child, _alpha);
				}
			}

		}

        public static uTweenAlpha Begin(GameObject go, float duration, float delay, float from, float to,bool childs=false, UnityAction _UnityAction = null)
        {
            uTweenAlpha comp = uTweener.Begin<uTweenAlpha>(go, duration);
            comp.from = from;
            comp.to = to;
            comp.delay = delay;
            comp.includeChilds = childs;

			if(_UnityAction!=null)
            {
				comp.onFinished = new UnityEvent();
				comp.onFinished.AddListener(_UnityAction);
			}

            if (duration <= 0)
            {
                comp.Sample(1, true);
                comp.enabled = false;
            }
            return comp;
        }

    }

}