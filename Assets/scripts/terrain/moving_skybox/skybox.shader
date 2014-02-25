Shader "Custom/Skybox" {
Properties {
	_Color ("Color Tint", COLOR) = (1, 1, 1, 1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
Category {
	Lighting Off
	ZWrite Off
	Cull back
	Blend SrcAlpha OneMinusSrcAlpha
	Tags { Queue=Transparent }
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				ConstantColor [_Color]
				combine texture * constant
			} 
		}
	}
}
}
