Shader "Hidden/URPHighlighted/StencilOpaqueZ"
{
	Properties
	{
		_Outline("Outline Color", Color) = (0,0,0,1)
	}

	SubShader
	{
		Pass
		{
			ZWrite On
			ZTest LEqual
			Lighting Off
			Fog { Mode Off }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			// Opaque
			uniform fixed4 _Outline;

			struct appdata_vert
			{
				float4 vertex : POSITION;
			};

			float4 vert(appdata_vert v) : POSITION
			{
				return UnityObjectToClipPos(v.vertex);
			}

			fixed4 frag() : COLOR
			{
				return _Outline;
			}
			ENDCG
		}
	}
	
	Fallback Off
}
