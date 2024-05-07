Shader "Hidden/URPHighlighted/StencilTransparent"
{
	Properties
	{
		_MainTex ("", 2D) = "" {}
		_Cutoff ("", Float) = 0.5
		_Outline("Outline Color", Color) = (0,0,0,1)
	}
	
	SubShader
	{
		Pass
		{
			ZWrite On
			ZTest Always
			Lighting Off
			Fog { Mode Off }
			
			CGPROGRAM
			#pragma vertex vert_alpha
			#pragma fragment frag_alpha
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			// Transparent
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed _Cutoff;
			// Opaque
			uniform fixed4 _Outline;

			struct appdata_vert_tex
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert_alpha(appdata_vert_tex v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag_alpha(v2f i) : COLOR
			{
				clip(tex2D(_MainTex, i.texcoord).a - _Cutoff);
				return _Outline;
			}
			ENDCG
		}
	}
	
	Fallback Off
}
