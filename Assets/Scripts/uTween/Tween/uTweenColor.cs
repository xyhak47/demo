using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Color(uTools)")]
	public class uTweenColor : uTweener {

		public Color from = Color.white;
		public Color to = Color.white;
		public static bool includeChilds = false;

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
        private RawImage mRawImage;
        private SpriteRenderer mSpriteRender;

		Color mColor = Color.white;
		private static Transform TmpObj;
		public Color colorValue {
			get {
				return mColor;
			}
			set {
				if(TmpObj)
					SetColor(TmpObj, value);
				else
					SetColor(transform, value);
				mColor = value;
			}
		}

		protected override void OnUpdate (float factor, bool isFinished)
		{
			colorValue = Color.Lerp(from, to, factor);
		}

		public static uTweenColor Begin(GameObject go, float duration, float delay, Color from, Color to,bool child=false) {
			TmpObj = go.transform;
			uTweenColor comp = uTweener.Begin<uTweenColor>(go, duration);
			comp.from = from;
			comp.to = to;
			comp.delay = delay;
			includeChilds = child;

			if (duration <=0) {
				comp.Sample(1, true);
				comp.enabled = false;
			}
			return comp;
		}

		void SetColor(Transform _transform, Color _color) {
			mText = _transform.GetComponent<Text> ();
			if (mText != null){
				mText.color = _color;
			}
			mLight = _transform.GetComponent<Light>();
			if (mLight != null){ 
				mLight.color = _color;
			}
			mImage = _transform.GetComponent<Image> ();
			if (mImage != null) {
				mImage.color = _color;
			}
            mRawImage = _transform.GetComponent<RawImage>();
            if (mRawImage != null)
            {
                mRawImage.color = _color;
            }
            mSpriteRender = _transform.GetComponent<SpriteRenderer> ();
			if (mSpriteRender != null) {
				mSpriteRender.color = _color;
			}
			if (_transform.GetComponent<Renderer>() != null) {
				mMat = _transform.GetComponent<Renderer>().material;
				if (mMat != null) {
					mMat.color = _color;
				}
			}
			if (includeChilds) {
				for (int i = 0; i < _transform.childCount; ++i) {
					Transform child = _transform.GetChild(i);
					SetColor(child, _color);
				}
			}
			
		}


	}
}
