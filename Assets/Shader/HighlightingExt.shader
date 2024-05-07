Shader "Hidden/URPHighlighted/Extend"
{

	Properties
	{
		_Expand("Thickness", Range(1,1.5)) = 1.5
		_Outline("Outline Color", Color) = (1,0,1,1)
	}
	
	SubShader
	{
		Pass
		{
			Name "Mask"
			Tags{"LightMode" = "LightweightForward"}
			ZWrite ON
			ZTest Always
			//Lighting Off
			//Fog { Mode Off }
			
			CGPROGRAM
			//#include "HighlightingInclude.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest


			#include "UnityCG.cginc"

			// Opaque
			uniform fixed4 _Outline;
			float _Expand;

			struct appdata_vert
			{
				float4 vertex : POSITION;
			};

			float4 vert(appdata_vert v) : POSITION
			{
				return UnityObjectToClipPos(v.vertex.xyz * _Expand);
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
