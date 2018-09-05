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
			Material {
				Emission [_Color]
			}
			Pass {
				SetTexture [_MainTex] {
					Combine Texture * Primary, Texture * Primary
				}
			}
		}	
	}
}