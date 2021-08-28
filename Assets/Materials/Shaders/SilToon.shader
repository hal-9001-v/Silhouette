Shader "Custom/SilToon"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0


		_AmbientLight("AmbientLight", Color) = (0.4,0.4,0.4,1)

		_RimColor("RimColor", Color) = (0.4,0.4,0.4,1)
		_RimAmmount("RimAmmount", Range(0,1)) = 0.0
		_RimDifference("RimDifference", Range(0,0.5)) = 0.0
		_RimThreshold("RimThreshold", Range(0,10)) = 0.0

		_InAttenuationOffset("InAttenuation", Range(0, 1)) = 1
		_OutAttenuationOffset("OutAttenuation", Range(0, 1)) = 1
		_Threshold("Threshold", Range(0, 1)) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Toon fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			fixed4 _AmbientLight;

			fixed4 _RimColor;
			half _RimAmmount;
			half _RimDifference;
			half _RimThreshold;


			half _InAttenuationOffset;
			half _OutAttenuationOffset;
			half _Threshold;

			half4 LightingToon(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {

				half NdotL = dot(s.Normal, lightDir);
				half rimDot = 1 - dot(s.Normal, viewDir);

				half4 c;
				half rimIntensity = smoothstep(_RimAmmount - _RimDifference, _RimAmmount + _RimDifference, rimDot) * pow(NdotL, _RimThreshold);

				c.rgb = s.Albedo * _LightColor0.rgb * NdotL + rimDot * (_RimColor * rimIntensity);


				if (atten > _Threshold) {
					c.rgb *= pow(atten ,_InAttenuationOffset);
				}
				else {
					c.rgb *= pow(atten , _OutAttenuationOffset);
				}
				c.rgb += _AmbientLight;
				c.a = s.Alpha;
				return c;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				//o.Metallic = _Metallic;
				//o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
