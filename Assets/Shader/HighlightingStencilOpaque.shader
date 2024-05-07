Shader "Hidden/URPHighlighted/StencilOpaque"
{

	Properties
	{
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
