Shader "Incant/Seal" {
	Properties {
		_Color ("Color Tint", Color) = (1,1,1,1)
		_MainTex ("Texture Color (RGB) Alpha (A)", 2D) = "white"
	}
	Category {
		Tags{ Queue=Transparent }

		Lighting On
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		SubShader {

			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				float4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};
				struct vert2Frag
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				vert2Frag vert( appdata IN )
				{
					vert2Frag OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
					return OUT;
				}
				fixed4 frag( vert2Frag IN ) : SV_Target
				{
					fixed4 color = tex2D(_MainTex, IN.uv) * _Color;
					return color;
				}

				ENDCG
			}
		}	
	}
}