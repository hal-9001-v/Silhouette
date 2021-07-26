Shader "CustomShaders/Unlit/Edges"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_XAmplitude("XDistance", Float) = 1
		_YAmplitude("YDistance", Float) = 1
		_Speed("Speed", Float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;
				float _XAmplitude;
				float _YAmplitude;
				float _Speed;

				v2f vert(appdata v)
				{
					v2f o;

					v.vertex.y += sin(_Time.x * _Speed + v.vertex.x * _XAmplitude) * _YAmplitude;

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = _Color;
					return col;
				}
				ENDCG
		}
		}
}
